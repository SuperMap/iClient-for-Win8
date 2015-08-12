using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows;
using SuperMap.WinRT.Resources;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml;
using System.IO;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Service
{
    /// <summary>
    /// 	<para>${mapping_ServiceRequest_Tile}</para>
    /// 	<para>${mapping_ServiceRequest_Description}</para>
    /// </summary>
    public class ServiceRequest
    {
        /// <summary>${mapping_ServiceRequest_event_Completed_D}</summary>
        //public event EventHandler<RequestEventArgs> Completed;
        /// <summary>${mapping_ServiceRequest_event_Failed_D}</summary>
        //public event EventHandler<RequestEventArgs> Failed;

        internal ServiceRequest()
        {
        }

        /// <summary>${mapping_ServiceRequest_method_BeginRequest_Object_D}</summary>
        /// <param name="state">${mapping_ServiceRequest_method_BeginRequest_Object_param_state}</param>
        public async Task<string> BeginRequest()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(ProxyEncodeUrl(Url));

            if (this.ForcePost)//iServerJava6R的POST
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                //request.Headers["Content-Type"] = "application/x-www-form-urlencoded;charset=UTF-8";
                //由于REST的Edit请求体和其他REST的POST请求体不一样，所以这里需要单独判断一下！
                //此外，部分服务的请求体可能并不是键值对构成的Json，而是一个普通的字符串，因此，如果PostBody属性不为null，
                //则认为此时的请求体是PostBody，而不是键值对。
                string postBody = string.Empty;
                if (!string.IsNullOrEmpty(PostBody))
                {
                    postBody = PostBody;
                }
                else
                {
                    if (Parameters == null)
                    {
                        Parameters = new Dictionary<string, string>();
                    }
                    if (IsEditable)//编辑
                    {
                        postBody = EditGetString(Parameters);
                    }
                    else if (IsTempLayersSet)//制作专题图。
                    {
                        postBody = POSTGetStringFromDictionaryinSixthR(Parameters);
                    }

                    else//非编辑、非子图层控制
                    {
                        postBody = POSTGetStringFromDictionary(Parameters);
                    }
                }
                request.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(postBody);
                Stream stream = await request.GetRequestStreamAsync();
                await stream.WriteAsync(data, 0, data.Length);
                stream.Dispose();
            }
            else
            {
                if (Parameters == null)
                {
                    Parameters = new Dictionary<string, string>();
                }
                string queryString = GetStringFromDictionary(Parameters);
                string uriString = CreateUrl(this.Url, queryString, this.ProxyUrl);

                if (uriString.Length <= MagicNumber.GET_MAX_LEGTH)//iServer2、IS6、iServerJava6R的GET
                {
                    request.Method = "GET";
                }
                else//iServer2、IS6的POST
                {
                    request.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    byte[] data = Encoding.UTF8.GetBytes(queryString);
                    Stream stream = await request.GetRequestStreamAsync();
                    await stream.WriteAsync(data, 0, data.Length);
                    stream.Dispose();
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync();
                reader.Dispose();
                return result;
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

        private Uri ProxyEncodeUrl(string url)
        {
            Uri uri = null;
            if (string.IsNullOrEmpty(this.ProxyUrl))
            {
                uri = new Uri(url);
            }
            else
            {
                uri = new Uri(ProxyEncodeUrl(url, this.ProxyUrl), UriKind.RelativeOrAbsolute);
            }
            return uri;
        }

        private static string ProxyEncodeUrl(string url, string proxyUrl)
        {
            if (string.IsNullOrEmpty(proxyUrl))
            {
                return url;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}?{1}", proxyUrl, WebUtility.UrlEncode(url));
        }

        private static string CreateUrl(string url, string queryString, string proxyUrl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            builder.Append(queryString);
            return ProxyEncodeUrl(builder.ToString(), proxyUrl);
        }

        private static string EditGetString(Dictionary<string, string> parameters)
        {
            string json = "[";
            List<string> list = new List<string>();
            foreach (string value in parameters.Values)
            {
                list.Add(CheckString(value));
            }
            json += string.Join(",", list.ToArray());
            json += "]";
            return json;
        }
        //private static string SetLayerStatusGetString(Dictionary<string, string> parameters)
        //{
        //    string json = "";
        //    List<string> list = new List<string>();
        //    foreach (string value in parameters.Values)
        //    {
        //        list.Add(value);
        //    }
        //    json += string.Join(",", list.ToArray());
        //    return json;
        //}

        private static string POSTGetStringFromDictionary(Dictionary<string, string> parameters)
        {
            string json = "{";
            List<string> list = new List<string>();
            foreach (string key in parameters.Keys)
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"{0}\":{1}", new object[] { key, CheckString(parameters[key]) }));
            }
            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        private static string POSTGetStringFromDictionaryinSixthR(Dictionary<string, string> parameters)
        {
            string json = "[{";
            List<string> list = new List<string>();
            foreach (string key in parameters.Keys)
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"{0}\":{1}", new object[] { key, CheckString(parameters[key]) }));
            }
            json += string.Join(",", list.ToArray());
            json += "}]";

            return json;
        }

        private static string CheckString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "\"\"";
            }
            if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                return str;
            }
            if (str.StartsWith("[") && str.EndsWith("]"))
            {
                return str;
            }
            if (str.StartsWith("{") && str.EndsWith("}"))
            {
                return str;
            }
            double temp = 0;

            if (double.TryParse(str, out temp))
            {
                return str;
            }
            else
                return "\"" + str + "\"";
        }

        private static string GetStringFromDictionary(Dictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in parameters.Keys)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}&", new object[] { key, WebUtility.UrlEncode(parameters[key]) });
            }
            return builder.ToString();
        }

        //private void OnCompleted(RequestEventArgs args)
        //{
        //    if (this.Completed != null)
        //    {
        //        this.Completed(this, args);
        //    }
        //}

        //private void OnFailed(RequestEventArgs args)
        //{
        //    if (this.Failed != null)
        //    {
        //        this.Failed(this, args);
        //    }
        //}

        /// <summary>${mapping_ServiceRequest_attribute_forcePost_D}</summary>
        internal bool ForcePost { get; set; }

        /// <summary>${mapping_ServiceRequest_attribute_Url_D}</summary>
        internal string Url { get; set; }
        /// <summary>${mapping_ServiceRequest_attribute_ProxyUrl_D}</summary>
        internal string ProxyUrl { get; set; }
        /// <summary>${mapping_ServiceRequest_attribute_Parameters_D}</summary>
        internal Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// POST请求的请求体，还是开出来吧，有些服务的请求体并不是键值对构成的Json。
        /// </summary>
        internal string PostBody { get; set; }

        //没办法，编辑的参数构造和别的都不一样！
        internal bool IsEditable { get; set; }

        //还是没办法,iServer6制作专题图。
        internal bool IsTempLayersSet { get; set; }
    }
}
