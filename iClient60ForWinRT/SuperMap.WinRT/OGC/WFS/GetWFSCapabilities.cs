using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SuperMap.WinRT.Core;
using System.Threading.Tasks;
using System.Net;
using SuperMap.WinRT.Resources;
using System.IO;

namespace SuperMap.WinRT.OGC
{
    /// <summary>
    /// 	<para>${mapping_GetCapabilitiesService_Tile}</para>
    /// 	<para>${mapping_GetWFSCapabilities_TileDescription}</para>
    /// </summary>
    public class GetWFSCapabilities : WFSServiceBase
    {
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
            if (this.Url.EndsWith("/"))
            {
                this.Url = this.Url.TrimEnd('/');
            }

            string midurl = "?service=wfs&request=GetCapabilities&version=" + this.Version;
            return this.Url + midurl;
        }
        /// <summary>${mapping_GetCapabilitiesService_method_ProcessAsync_D}</summary>
        public async Task<List<WFSFeatureType>> ProcessAsync()
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

        private List<WFSFeatureType> ParseSuccessResult(XDocument document)
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
            return features;
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
