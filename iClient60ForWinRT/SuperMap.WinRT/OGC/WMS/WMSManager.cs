using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows;
using System.Xml.Linq;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Resources;
using System.IO;
using System.Threading.Tasks;

namespace SuperMap.WinRT.OGC
{
    internal class WMSManager
    {
        //private string httpGetResource;
        private static Version highestSupportedVersion = new Version(1, 3);

        public event EventHandler<EventArgs> GetCapabilityCompleted;

        public WMSManager(string url, string version, string proxyUrl)
        {
            this.Metadata = new Dictionary<string, string>();
            this.AllLayerList = new ObservableCollection<WMSLayerInfo>();
            this.ProxyUrl = proxyUrl;

            _url = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", url, "?service=WMS&request=GetCapabilities&version=", version);
        }

        public async Task<WMSManager> GetResponse()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(this.PrefixProxy(_url));
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync();
                reader.Dispose();
                XDocument doc = XDocument.Parse(result);
                this.ParseCapabilities(doc);
                return this;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = ex.Response as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    var message = reader.ReadToEnd();
                    throw new WebException(message, ex.InnerException);
                }
                else
                {

                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ParseCapabilities(XDocument document)
        {
            string ns = document.Root.Name.NamespaceName;
            if (document.Root.Attribute("version") != null)
            {
                this.Version = document.Root.Attribute("version").Value;
            }
            var type = (from Service in document.Descendants(XName.Get("Service", ns))
                        select new WMSLayerInfo
                        {
                            Title = (Service.Element(XName.Get("Title", ns)) == null) ? null : Service.Element(XName.Get("Title", ns)).Value,
                            Abstract = (Service.Element(XName.Get("Abstract", ns)) == null) ? null : Service.Element(XName.Get("Abstract", ns)).Value,
                            Name = (Service.Element(XName.Get("Name", ns)) == null) ? null : Service.Element(XName.Get("Name", ns)).Value
                        }).First();

            if (type != null)
            {
                this.Metadata.Add("Name", type.Name);
                this.Metadata.Add("Title", type.Title);
                this.Metadata.Add("Abstract", type.Abstract);
            }

            //获取Layer信息；
            foreach (WMSLayerInfo info in from Layers in document.Descendants(XName.Get("Layer", ns))
                                          where Layers.Descendants(XName.Get("Layer", ns)).Count<XElement>() == 0
                                          select new WMSLayerInfo
                                       {
                                           Name = (Layers.Element(XName.Get("Name", ns)) == null) ? null : Layers.Element(XName.Get("Name", ns)).Value,
                                           Title = (Layers.Element(XName.Get("Title", ns)) == null) ? null : Layers.Element(XName.Get("Title", ns)).Value,
                                           Abstract = (Layers.Element(XName.Get("Abstract", ns)) == null) ? null : Layers.Element(XName.Get("Abstract", ns)).Value,
                                           Bounds = GetBounds(Layers.Element(XName.Get("BoundingBox", ns)))
                                       }
                    )
            {
                this.AllLayerList.Add(info);
            }

            //try
            //{
            //    XElement element = (from c in (from c in document.Descendants(XName.Get("Capability", ns)) select c).First<XElement>().Descendants(XName.Get("Request", ns)) select c).First<XElement>();
            //    //this.httpGetResource = (from c in element.Descendants(XName.Get("OnlineResource", ns)) select c).First<XElement>().Attribute(XName.Get("href", "http://www.w3.org/1999/xlink")).Value;
            //}
            //catch
            //{
            //    //this.Url = this.httpGetResource;
            //}
            IEnumerable<XElement> source = document.Descendants(XName.Get("BoundingBox", ns));
            if (!source.GetEnumerator().MoveNext() && this.LowerThan13Version())
            {
                XElement element = document.Descendants(XName.Get("BoundingBox", ns)).First<XElement>();
                CRS = new CoordinateReferenceSystem(4326);
                this.Bounds = GetBounds(element);
            }

            if (CRS == null)
            {
                string str = this.LowerThan13Version() ? "SRS" : "CRS";

                foreach (XElement element in source)
                {
                    if (element.Attribute((XName)str) != null && element.Attribute((XName)str).Value.StartsWith("EPSG:"))
                    {
                        try
                        {
                            int wkid = int.Parse(element.Attribute((XName)str).Value.Replace("EPSG:", ""), CultureInfo.InvariantCulture);
                            CRS = new CoordinateReferenceSystem(wkid);
                        }
                        catch
                        {
                        }
                        this.Bounds = GetBounds(element);
                        break;
                    }
                }

                if (Rectangle2D.IsNullOrEmpty(this.Bounds))
                {
                    XElement element = source.First<XElement>();
                    if (element.Attribute((XName)str) != null)
                    {
                        string s = element.Attribute((XName)str).Value;
                        int startIndex = s.LastIndexOf(":");
                        if (startIndex > -1)
                        {
                            s = s.Substring(startIndex + 1);
                        }

                        try
                        {
                            int num = int.Parse(s, CultureInfo.InvariantCulture);
                            CRS = new CoordinateReferenceSystem(num);
                        }
                        catch
                        {
                        }
                    }
                    this.Bounds = GetBounds(element);
                }
            }

        }

        private Uri PrefixProxy(string url)
        {
            if (string.IsNullOrEmpty(this.ProxyUrl))
            {
                return new Uri(url, UriKind.RelativeOrAbsolute);
            }
            string _proxyUrl = this.ProxyUrl;
            if (!_proxyUrl.Contains("?"))
            {
                if (!_proxyUrl.EndsWith("?"))
                {
                    _proxyUrl += "?";
                }
            }
            else if (!_proxyUrl.EndsWith("&"))
            {
                _proxyUrl += "&";
            }

            return new UriBuilder(_proxyUrl) { Query = url }.Uri;
        }

        private static Rectangle2D GetBounds(XElement element)
        {
            if (element == null)
                return Rectangle2D.Empty;

            Rectangle2D rect = new Rectangle2D(
                 (element.Attribute("minx") == null) ? double.MinValue : double.Parse(element.Attribute("minx").Value, CultureInfo.InvariantCulture),
                 (element.Attribute("miny") == null) ? double.MinValue : double.Parse(element.Attribute("miny").Value, CultureInfo.InvariantCulture),
                 (element.Attribute("maxx") == null) ? double.MinValue : double.Parse(element.Attribute("maxx").Value, CultureInfo.InvariantCulture),
                 (element.Attribute("maxy") == null) ? double.MinValue : double.Parse(element.Attribute("maxy").Value, CultureInfo.InvariantCulture)
                 );
            return rect;
        }

        private bool LowerThan13Version()
        {
            Version version = new Version(this.Version);
            if (version < highestSupportedVersion)
            {
                return true;
            }
            return false;
        }

        private string _url;

        //public string Url { get; private set; }
        public string ProxyUrl { get; private set; }
        public string Version { get; private set; }

        public Exception Error { get; private set; }
        public Rectangle2D Bounds { get; private set; }
        public CoordinateReferenceSystem CRS { get; private set; }
        public Dictionary<string, string> Metadata { get; private set; }
        public IList<WMSLayerInfo> AllLayerList { get; private set; }
    }
}
