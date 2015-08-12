using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Resources;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace SuperMap.WinRT.OGC
{
    /// <summary>${mapping_GetWFSFeature_Tile}</summary>
    public class GetWFSFeature : WFSServiceBase
    {
        private Dictionary<string, string> nameSpacePair;
        /// <summary>${mapping_GetWFSFeature_constructor_D}</summary>
        public GetWFSFeature()
        {
            nameSpacePair = new Dictionary<string, string>();
            featureDescriptions = new List<WFSFeatureDescription>();
            featureIDs = new List<string>();
            filters = new List<Filter>();
        }
        /// <summary>${mapping_GetWFSFeature_constructor_string_D}</summary>
        public GetWFSFeature(string url)
            : this()
        {
            this.Url = url;
        }
        /// <summary>${mapping_GetWFSFeatureService_method_ProcessAsync_D}</summary>
        public async Task<GetWFSFeatureResult> ProcessAsync()
        {
            string url = GetFinalUrl();

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(ExceptionStrings.ParametersError);
            }

            HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(url + "&t=" + DateTime.Now.Ticks));
            request.Method = "GET";
            try
            {
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = await reader.ReadToEndAsync().ConfigureAwait(false);
            reader.Dispose();
            XDocument document = XDocument.Parse(result);
            this.ThrowResultExecption(document);
            return ParseSuccessResult(document);
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                var message = reader.ReadToEnd();
                throw new WebException(message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        //MAXFEATURES=N,A positive integer indicating the maximum number of features that the WFS
        //should return in response  to a query. If no value is specified then all result instances should be presented.
        //默认值为int.MinValue，返回所有的值
        private int maxFeatures = int.MinValue;
        /// <summary>${mapping_GetWFSFeatureService_attribute_MaxFeatures_D}</summary>
        public int MaxFeatures
        {
            get { return maxFeatures; }
            set { maxFeatures = value; }
        }

        private GetWFSFeatureResult ParseSuccessResult(XDocument document)
        {
            GetWFSFeatureResult featureRes = new GetWFSFeatureResult();

            if (document != null && document.Document != null && document.Document.Root != null)
            {
                foreach (var item in document.Document.Root.Attributes())
                {
                    nameSpacePair.Add(item.Name.LocalName, item.Value);
                }

                //decimal="." cs="," ts=" " (-133.878943,-45.6267275) (540000,5450000) 
                //这里只提供默认的分隔符，自定义的就不考虑了，用户如果有需求那就自己处理吧。
                XElement XBounds = document.Document.Root.Element(XName.Get("boundedBy", nameSpacePair["gml"]));
                XElement pointvalue = XBounds != null ? XBounds.Element(XName.Get("Box", nameSpacePair["gml"])) : null;

                //还有unknown的情况啊
                if (pointvalue != null && !string.IsNullOrEmpty(pointvalue.Value))
                {
                    string[] value = pointvalue.Value.Trim().Split(',', ' ');
                    if (value != null && value.Length == 4)
                    {
                        double x1 = double.Parse(value[0], CultureInfo.InvariantCulture);
                        double y1 = double.Parse(value[1], CultureInfo.InvariantCulture);
                        double x2 = double.Parse(value[2], CultureInfo.InvariantCulture);
                        double y2 = double.Parse(value[3], CultureInfo.InvariantCulture);
                        if (x2 - x1 >= 0 || y2 - y1 >= 0)
                        {
                            featureRes.Bounds = new Rectangle2D(x1, y1, x2, y2);
                        }
                    }
                }
                else //有时候查询出来没有结果，所以就认为是成功了。
                {

                }

                IEnumerable<XElement> featureMembers = document.Document.Root.Elements(XName.Get("featureMember", nameSpacePair["gml"]));
                foreach (var item in featureMembers)
                {
                    ParseFeatureMember(featureRes, item);
                }
            }
            return featureRes;
        }

        private async void ParseFeatureMember(GetWFSFeatureResult featureRes, XElement item)
        {
            if (item != null)
            {
                XElement element = item.FirstNode as XElement;
                string profix = string.Empty;
                string fullEleName = string.Empty;

                if (element != null)
                {
                    profix = element.GetPrefixOfNamespace(element.Name.Namespace);
                    fullEleName = string.IsNullOrEmpty(profix) ? element.Name.LocalName : profix + ":" + element.Name.LocalName;
                }

                string spatialPropName = GetSpatialPropName(fullEleName);

                if (!featureRes.FeaturePair.ContainsKey(fullEleName))
                {
                    featureRes.FeaturePair.Add(fullEleName, new FeatureCollection());
                }

                if (featureRes.FeaturePair[fullEleName] != null)
                {
                    foreach (Feature f in await GetFeatures(element, spatialPropName))
                    {
                        featureRes.FeaturePair[fullEleName].Add(f);
                    }
                }
            }
        }

        private string GetSpatialPropName(string name)
        {
            string propName = string.Empty;
            foreach (var item in this.FeatureDescriptions)
            {
                if (item.TypeName == name && !string.IsNullOrEmpty(name))
                {
                    propName = item.SpatialProperty;
                    break;
                }
            }
            return propName;
        }

        private async Task<FeatureCollection> GetFeatures(XElement ele, string spatialPropName)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            FeatureCollection fs = new FeatureCollection();
            if (ele != null)
            {
                //加属性
                foreach (KeyValuePair<string, string> itemPair in (from subItem in ele.Elements()
                                                                   where (subItem.Name.LocalName != spatialPropName)
                                                                   select new KeyValuePair<string, string>(subItem.Name.LocalName, subItem.Value)
                 ))
                {
                    attributes.Add(itemPair.Key, itemPair.Value);
                }

                XElement geoElement = string.IsNullOrEmpty(spatialPropName) ? null : ele.Element(XName.Get(spatialPropName, this.FeatureNS));

                if (geoElement != null)
                {

                    XElement firstNode = geoElement.FirstNode as XElement;
                    string geometryTypeName = firstNode != null ? firstNode.Name.LocalName : string.Empty;
                    GeometryType type;

                    #region 这段代码主要用来处理混淆问题。如果删除这段代码，SampleCode的相应案例无法正常运行。
                    try
                    {
                        Enum.Parse(typeof(GeometryType), geometryTypeName, true);
                    }
                    catch (Exception e)
                    {
                        throw new OverflowException("转化出错了", e);
                    }
                    #endregion

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if (Enum.TryParse<GeometryType>(geometryTypeName, true, out type))
                        {
                            if (type == GeometryType.Point)
                            {
                                GeoPoint point = this.ParsePointCoordinates((geoElement.FirstNode as XElement).Value);
                                Feature col = new Feature();
                                foreach (var v in attributes)
                                {
                                    col.Attributes.Add(v.Key, v.Value);
                                }
                                col.Geometry = point;
                                fs.Add(col);
                            }
                            else if (type == GeometryType.LineString)
                            {
                                XElement lineStrEle = geoElement.Element(XName.Get("LineString", nameSpacePair["gml"]));
                                GeoLine line = ParseLineCoordinates(lineStrEle);
                                Feature col = new Feature();
                                foreach (var v in attributes)
                                {
                                    col.Attributes.Add(v.Key, v.Value);
                                }
                                col.Geometry = line;
                                fs.Add(col);
                            }
                            else if (type == GeometryType.Polygon)
                            {
                                XElement polygonEle = geoElement.Element(XName.Get("Polygon", nameSpacePair["gml"]));
                                GeoRegion region = ParseRegionCoordinates(polygonEle);
                                Feature col = new Feature();
                                foreach (var v in attributes)
                                {
                                    col.Attributes.Add(v.Key, v.Value);
                                }
                                col.Geometry = region;
                                fs.Add(col);
                            }
                            else if (type == GeometryType.MultiPoint)
                            {
                                //TODO:处理多点。
                                //暂时不做处理。
                            }
                            else if (type == GeometryType.MultiLineString)
                            {
                                IEnumerable<XElement> lineStringMemberEle = geoElement.Descendants(XName.Get("lineStringMember", nameSpacePair["gml"]));
                                foreach (XElement els in lineStringMemberEle)
                                {
                                    GeoLine line = ParseMultiLineCoordinates(els);
                                    Feature col = new Feature();
                                    foreach (var v in attributes)
                                    {
                                        col.Attributes.Add(v.Key, v.Value);
                                    }
                                    col.Geometry = line;
                                    fs.Add(col);
                                }

                            }
                            else if (type == GeometryType.MultiPolygon)
                            {
                                IEnumerable<XElement> polygonMemberEle = geoElement.Descendants(XName.Get("polygonMember", nameSpacePair["gml"]));

                                foreach (XElement els in polygonMemberEle)
                                {
                                    GeoRegion region = ParseMultiRegionCoordinates(els);
                                    Feature col = new Feature();
                                    foreach (var v in attributes)
                                    {
                                        col.Attributes.Add(v.Key, v.Value);
                                    }

                                    col.Geometry = region;
                                    fs.Add(col);
                                }
                            }
                        }
                    });
                }
            }
            return fs;
        }

        private GeoRegion ParseRegionCoordinates(XElement ele)
        {
            GeoRegion region = new GeoRegion();
            if (ele != null && !string.IsNullOrEmpty(ele.Value))
            {
                foreach (var linearRing in ele.Descendants(XName.Get("LinearRing", nameSpacePair["gml"])))
                {
                    string[] values = linearRing.Value.Trim().Split(',', ' ');
                    region.Parts.Add(GetPoint2Ds(values));
                }
            }
            return region;
        }

        private GeoRegion ParseMultiRegionCoordinates(XElement eles)
        {
            GeoRegion region = new GeoRegion();
            if (eles != null)
            {
                foreach (var linearRing in eles.Descendants(XName.Get("LinearRing", nameSpacePair["gml"])))
                {
                    string[] values = linearRing.Value.Trim().Split(',', ' ');
                    region.Parts.Add(GetPoint2Ds(values));
                }
            }
            return region;
        }

        private GeoPoint ParsePointCoordinates(string p)
        {
            GeoPoint point = new GeoPoint();
            if (!string.IsNullOrEmpty(p))
            {
                string[] pointStr = p.Trim().Split(',');
                point.X = double.Parse(pointStr[0], CultureInfo.InvariantCulture);
                point.Y = double.Parse(pointStr[1], CultureInfo.InvariantCulture);
            }
            return point;
        }

        private GeoLine ParseLineCoordinates(XElement ele)
        {
            GeoLine line = new GeoLine();
            if (ele != null && !string.IsNullOrEmpty(ele.Value))
            {
                string[] values = ele.Value.Trim().Split(',', ' ');

                line.Parts.Add(this.GetPoint2Ds(values));
            }
            return line;
        }

        private GeoLine ParseMultiLineCoordinates(XElement eles)
        {
            GeoLine line = new GeoLine();
            if (eles != null)
            {
                if (!string.IsNullOrEmpty(eles.Value))
                {
                    string[] values = eles.Value.Trim().Split(',', ' ');
                    line.Parts.Add(this.GetPoint2Ds(values));
                }
            }
            return line;
        }

        private Point2DCollection GetPoint2Ds(string[] val)
        {
            Point2DCollection col = new Point2DCollection();
            if (val != null)
            {
                for (int i = 0; i < val.Length; i += 2)
                {
                    col.Add(new Point2D(double.Parse(val[i], CultureInfo.InvariantCulture), double.Parse(val[i + 1], CultureInfo.InvariantCulture)));
                }
            }
            return col;
        }

        ///// <summary>${mapping_GetWFSFeatureService_attribute_Filter_D}</summary>
        //public List<Filter> Filter { get; internal set; }
        ///// <summary>${mapping_GetWFSFeatureService_attribute_BBox_D}</summary>
        //public Rectangle2D BBox { get; set; }

        //自定义命名空间名称
        /// <summary>${mapping_GetWFSFeatureService_attribute_FeatureNS_D}</summary>
        public string FeatureNS { get; set; }
        /// <summary>${mapping_GetWFSFeatureService_attribute_LayerType_D}</summary>
        public List<WFSFeatureDescription> FeatureDescriptions
        {
            get { return featureDescriptions; }
            set { featureDescriptions = value; }
        }
        private List<WFSFeatureDescription> featureDescriptions;
        /// <summary>${mapping_GetWFSFeatureService_attribute_Filter_D}</summary>
        public List<Filter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }
        private List<Filter> filters;
        /// <summary>${mapping_GetWFSFeatureService_attribute_FeatureIDs_D}</summary>
        public List<string> FeatureIDs
        {
            get { return featureIDs; }
            set { featureIDs = value; }
        }
        private List<string> featureIDs;
        enum GeometryType
        {
            Point,//pointProperty
            LineString,//lineStringProperty
            Polygon,//polygonProperty
            MultiPoint,//multiPointProperty
            MultiLineString,//multiLineStringProperty
            MultiPolygon,//multiPolygonProperty
        }
        /// <summary>${mapping_GetWFSFeatureService_method_GetBaseUrl_D}</summary>
        protected string GetBaseUrl()
        {
            if (!string.IsNullOrEmpty(this.Url) && this.Url.EndsWith("/"))
            {
                //去除结尾可能的'/'符号
                this.Url = this.Url.TrimEnd('/');
            }

            string url = this.Url;
            return url + "?SERVICE=WFS&VERSION=" + this.Version + "&REQUEST=GetFeature";
        }

        internal override string GetFinalUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                return string.Empty;
            }

            string finalUrl = this.GetBaseUrl();
            List<string> layerList = new List<string>();
            List<string> layerIDs = new List<string>();
            List<string> layerProp = new List<string>();
            string ogc = "http://www.opengis.net/ogc";
            string gml = "http://www.opengis.net/gml";
            string xmlns = "http://www.w3.org/2000/xmlns/";
            //获取typenames
            if (FeatureDescriptions.Count <= 0)
            {
                return finalUrl;
            }
            else
            {
                //Filter参数和LayerType里面的IDs参数冲突，当设置了Filter参数后，LayerType里面的IDs参数将无效
                //如果只想进行ID查询，请不要设置Filter参数。
                foreach (var item in FeatureDescriptions)
                {
                    layerList.Add(item.TypeName);
                    List<string> propertyNames = GetFullPropertyName(item);

                    if (propertyNames.Count > 0)
                    {
                        layerProp.Add("(" + string.Join(",", propertyNames) + ")");
                    }
                    else
                    {
                        layerProp.Add("()");
                    }
                }

                finalUrl += "&TYPENAME=" + string.Join(",", layerList);
                if (this.MaxFeatures != int.MinValue)
                {
                    finalUrl += "&MAXFEATURES=" + this.MaxFeatures;
                }

                //在发送请求GetFeature时，去掉PROPERTYNAME参数，因为吉奥的地图无法解析该参数中的小括号。
                //if (layerProp.Count > 0)
                //{
                //    finalUrl += "&PROPERTYNAME=" + string.Join("", layerProp);
                //}

                if (Filters != null && Filters.Count > 0)
                {
                    List<string> XFilterStr = new List<string>();
                    foreach (var filter in this.Filters)
                    {
                        XElement subValue = filter.ToXML();
                        XElement root = new XElement("{" + ogc + "}Filter", new XAttribute("{" + xmlns + "}gml", gml));

                        if (subValue != null)
                        {
                            root.Add(subValue);
                        }

                        XFilterStr.Add(root.ToString());
                    }

                    string filterStrs = string.Empty;
                    foreach (var item in XFilterStr)
                    {
                        filterStrs += string.Format("({0})", item);
                    }

                    finalUrl += string.Format("&Filter={0}", filterStrs);
                }
                else
                {
                    if (this.FeatureIDs.Count > 0)
                    {
                        finalUrl += "&FEATUREID=" + string.Join(",", this.FeatureIDs);
                    }
                }
                return finalUrl;
            }
        }

        internal List<string> GetFullPropertyName(WFSFeatureDescription layer)
        {
            List<string> newList = new List<string>();
            if (layer.Properties != null && layer.Properties.Count > 0)
            {
                //foreach (string item in layer.Properties)
                //{
                //    newList.Add(layer.TypeName + "/" + item);
                //}
                ////空间数据的属性
                //newList.Add(layer.TypeName + "/" + layer.SpatialProperty);
                foreach (string item in layer.Properties)
                {
                    newList.Add(item);
                }
                //空间数据的属性
                newList.Add(layer.SpatialProperty);
            }
            else
            {

            }

            return newList;
        }
    }
}
