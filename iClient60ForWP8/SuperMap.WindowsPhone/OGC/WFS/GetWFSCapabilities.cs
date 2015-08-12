using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary>
    /// 	<para>${mapping_GetCapabilitiesService_Tile}</para>
    /// 	<para>${mapping_GetWFSCapabilities_TileDescription}</para>
    /// </summary>
    public class GetWFSCapabilities : WFSServiceBase
    {
        /// <summary>${mapping_GetCapabilitiesService_Event_ProcessCompleted_D}</summary>
        public event EventHandler<ResultEventArgs<List<WFSFeatureType>>> ProcessCompleted;
        /// <summary>${mapping_GetCapabilitiesService_constructor_D}</summary>
        public GetWFSCapabilities()
        { }
        /// <summary>${mapping_GetCapabilitiesService_constructor_string_D}</summary>
        public GetWFSCapabilities(string url)
        {
            this.Url = url;
        }

        internal override string GetFinalUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                return string.Empty;
            }

            if (!Url.Contains("http://"))  //相对地址
            {
                var pageUrl = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var localUrl = pageUrl.AbsoluteUri.Substring(0, pageUrl.AbsoluteUri.IndexOf(pageUrl.AbsolutePath));
                this.Url = localUrl + Url;
            }

            string midurl = "?service=wfs&request=GetCapabilities";
            if (!string.IsNullOrEmpty(this.Version))
            {
                midurl += "&version=" + this.Version;
            }
            return this.Url + midurl;
        }
        /// <summary>${mapping_GetCapabilitiesService_method_ProcessAsync_D}</summary>
        public void ProcessAsync()
        {
            this.ProcessAsync(null);
        }

        private void OnProcessComplated(List<WFSFeatureType> result, string originRes, object userState)
        {
            if (ProcessCompleted != null)
            {
                ProcessCompleted(this, new ResultEventArgs<List<WFSFeatureType>>(result, originRes, userState));
            }
        }

        private Rectangle2D GetBounds(IEnumerable<XAttribute> items)
        {
            if (items != null)
            {
                Dictionary<string, string> atts = items.ToDictionary(p => p.Name.LocalName, p => p.Value);

                double minx = double.NaN;
                double miny = double.NaN;
                double maxx = double.NaN;
                double maxy = double.NaN;

                double.TryParse(atts["minx"], NumberStyles.Number, CultureInfo.InvariantCulture, out minx);
                double.TryParse(atts["miny"], NumberStyles.Number, CultureInfo.InvariantCulture, out miny);
                double.TryParse(atts["maxx"], NumberStyles.Number, CultureInfo.InvariantCulture, out maxx);
                double.TryParse(atts["maxy"], NumberStyles.Number, CultureInfo.InvariantCulture, out maxy);
                return new Rectangle2D(minx, miny, maxx, maxy);
            }
            else
            {
                return Rectangle2D.Empty;
            }
        }

        internal override void ParseSuccessResult(XDocument document, string originResult, object userState)
        {
            List<WFSFeatureType> features = new List<WFSFeatureType>();
            if (document != null && document.Root != null)
            {
                string ns = document.Root.Name.NamespaceName;
                foreach (var item in (from itemType in document.Root.Element(XName.Get("FeatureTypeList", ns)).Elements(XName.Get("FeatureType", ns))
                                      select new WFSFeatureType
                                      {
                                          TypeName = itemType.Element(XName.Get("Name", ns)) != null ? itemType.Element(XName.Get("Name", ns)).Value : string.Empty,
                                          Title = itemType.Element(XName.Get("Title", ns)) != null ? itemType.Element(XName.Get("Title", ns)).Value : string.Empty,
                                          SRS = GetCRS(itemType, ns),
                                          Bounds = GetBounds(itemType.Element(XName.Get("LatLongBoundingBox", ns)).Attributes()),
                                      }))
                {
                    features.Add(item);
                }
            }
            OnProcessComplated(features, originResult, userState);
        }

        private CoordinateReferenceSystem GetCRS(XElement item, string ns)
        {
            XElement xe = item.Element(XName.Get("SRS", ns));
            if (xe != null && xe.Value != null)
            {
                string[] values = xe.Value.Trim().Split(':');
                if (values != null && values.Count() > 1)
                {
                    return new CoordinateReferenceSystem(int.Parse(values[1], CultureInfo.InvariantCulture));
                }
            }
            return null;
        }
    }
}
