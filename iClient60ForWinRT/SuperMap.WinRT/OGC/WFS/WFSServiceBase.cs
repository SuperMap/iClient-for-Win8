using System;
using System.Net;
using System.Xml.Linq;
using SuperMap.WinRT.Resources;
using System.IO;
using System.Text;

namespace SuperMap.WinRT.OGC
{
    /// <summary>
    /// 	<para>${mapping_WFSServiceBase_Tile}</para>
    /// 	<para>${mapping_WFSServiceBase_Description}</para>
    /// </summary>
    public abstract class WFSServiceBase
    {
        /// <summary>${mapping_WFSServiceBase_Event_Failed_D}</summary>
        //public event EventHandler<FailedEventArgs> Failed;

        /// <summary>${mapping_WFSServiceBase_constructor_D}</summary>
        protected WFSServiceBase()
        {
            
        }

        protected string GetBaseUrl()
        {
            if (!string.IsNullOrEmpty(this.Url) && this.Url.EndsWith("/"))
            {
                //去除结尾可能的'/'符号
                this.Url = this.Url.TrimEnd('/');
            }

            return this.url + "?SERVICE=WFS&VERSION=" + version + "&REQUEST=GetFeature";
        }

        protected void ThrowResultExecption(XDocument document)
        {
            if (document != null && document.Document != null && document.Document.Root != null)
            {
                if (document.Document.Root.Name.LocalName == "ServiceExceptionReport")
                {
                    string code = string.Empty;
                    string locator = string.Empty;
                    string message = string.Empty;
                    if ((document.Document.Root.FirstNode as XElement).Attribute("code") != null)
                    {
                        code = (document.Document.Root.FirstNode as XElement).Attribute("code").Value;
                    }

                    if ((document.Document.Root.FirstNode as XElement).Attribute("locator") != null)
                    {
                        locator = (document.Document.Root.FirstNode as XElement).Attribute("locator").Value;
                    }

                    message = (document.Document.Root.FirstNode as XElement).Value;
                    throw new RequestException(code, locator, message);
                }
                else if (document.Document.Root.Name.LocalName == "ExceptionReport") //geoServer的错误用的是“OGC Web Services Common Specification”标准。
                {
                    string code = string.Empty;
                    string locator = string.Empty;
                    string message = string.Empty;
                    if ((document.Document.Root.FirstNode as XElement).Attribute("exceptionCode") != null)
                    {
                        code = (document.Document.Root.FirstNode as XElement).Attribute("exceptionCode").Value;
                    }

                    if ((document.Document.Root.FirstNode as XElement).Attribute("locator") != null)
                    {
                        locator = (document.Document.Root.FirstNode as XElement).Attribute("locator").Value;
                    }

                    message = (document.Document.Root.FirstNode as XElement).Value;
                    throw new RequestException(code, locator, message);
                }
            }
        }
        internal abstract string GetFinalUrl();

        private string url;
        /// <summary>${mapping_WFSServiceBase_attribute_Url_D}</summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        //默认是1.0.0
        private string version = "1.0.0";
        /// <summary>${mapping_WFSServiceBase_attribute_Version_D}</summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
    }
}
