using System;
using System.Net;
using System.Xml.Linq;
using SuperMap.Web.Resources;

namespace SuperMap.Web.OGC
{
    /// <summary>
    /// 	<para>${mapping_WFSServiceBase_Tile}</para>
    /// 	<para>${mapping_WFSServiceBase_Description}</para>
    /// </summary>
    public abstract class WFSServiceBase
    {
        /// <summary>${mapping_WFSServiceBase_Event_Failed_D}</summary>
        public event EventHandler<FailedEventArgs> Failed;

        private WebClient wc;
        /// <summary>${mapping_WFSServiceBase_constructor_D}</summary>
        protected WFSServiceBase()
        {
            wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
        }
        /// <summary>${mapping_WFSServiceBase_method_ProcessAsync_D}</summary>
        protected void ProcessAsync(object state)
        {
            string url = GetFinalUrl();

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(ExceptionStrings.ParametersError);
            }

            wc.DownloadStringAsync(new Uri(url + "&t=" + DateTime.Now.Ticks), state);//避免浏览器不发送同样的get请求
        }

        protected void ProcessAsync(string postbody, object state)
        {
            wc.Headers["Content-Type"] = "application/x-www-form-urlencoded;charset=UTF-8";
            string url = GetFinalUrl();

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(ExceptionStrings.ParametersError);
            }
            wc.UploadStringAsync(new Uri(url), "POST", postbody, state);
        }

        internal abstract string GetFinalUrl();

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!CheckFault(e))
            {
                this.ParseResult(e.Result, e.UserState);
            }
        }

        private void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (!CheckFault(e))
            {
                this.ParseResult(e.Result, e.UserState);
            }
        }

        private void ParseResult(string result, object userState)
        {
            XDocument document = XDocument.Parse(result);
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
                    OnFailed(new RequestException(code, locator, message));
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
                    OnFailed(new RequestException(code, locator, message));
                }
                else
                {
                    ParseSuccessResult(document, result, userState);
                }
            }
        }

        internal abstract void ParseSuccessResult(XDocument document, string originResult, object userState);

        internal bool CheckFault(DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return true;
            }

            if (e.Error == null)
            {
                return false;
            }

            OnFailed(new RequestException(e.Error));

            return true;
        }

        internal bool CheckFault(UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return true;
            }

            if (e.Error == null)
            {
                return false;
            }

            OnFailed(new RequestException(e.Error));

            return true;
        }

        private void OnFailed(RequestException errorArgs)
        {
            if (Failed != null)
            {
                this.Failed(this, new FailedEventArgs(errorArgs));
            }
        }

        private string GetBaseUrl()
        {
            if (!string.IsNullOrEmpty(this.Url) && this.Url.EndsWith("/"))
            {
                //去除结尾可能的'/'符号
                this.Url = this.Url.TrimEnd('/');
            }

            return this.url + "?SERVICE=WFS&VERSION=" + version + "&REQUEST=GetFeature";
        }

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

        //默认值为int.MinValue,这时候取回最新版本的值，0值时取回所有版本的值(All)，>0 值时返回设置值的版本
        //The featureVersion attribute is included in order to accommodate systems that support feature versioning. 
        //A value of ALL indicates that all versions of a feature should be fetched. Otherwise, an integer, n, 
        //can be specified to return the nth version of a feature. The version numbers start at 1, which is the oldest version. 
        //If a version value larger than the largest version number is specified, then the latest version is returned. 
        //The default action shall be for the query to return the latest version. 
        //Systems that do not support versioning can ignore the parameter and return the only version that they have.

        //private int featureVersion = int.MinValue;
        ///// <summary>${mapping_WFSServiceBase_attribute_FeatureVersion_D}</summary>
        //public int FeatureVersion
        //{
        //    get { return featureVersion; }
        //    set { featureVersion = value; }
        //}
    }
}
