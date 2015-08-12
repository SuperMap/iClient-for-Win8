using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows;
using SuperMap.WindowsPhone.Resources;
using SuperMap.WindowsPhone.Utilities;
using System.IO;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Service
{
    /// <summary>
    /// 	<para>${WP_mapping_ServiceRequest_Tile}</para>
    /// 	<para>${WP_mapping_ServiceRequest_Description}</para>
    /// </summary>
    public class ServiceRequest
    {
        /// <summary>${WP_mapping_ServiceRequest_event_Completed_D}</summary>
        //public event EventHandler<RequestEventArgs> Completed;
        /// <summary>${WP_mapping_ServiceRequest_event_Failed_D}</summary>
        //public event EventHandler<RequestEventArgs> Failed;

        internal ServiceRequest()
        {
        }

        /// <summary>${WP_mapping_ServiceRequest_method_BeginRequest_Object_D}</summary>
        /// <param name="state">${WP_mapping_ServiceRequest_method_BeginRequest_Object_param_state}</param>
        public async Task<string> BeginRequest()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(ProxyEncodeUrl(Url));
            request.AllowReadStreamBuffering = true;
            string uriString = ProxyEncodeUrl(this.Url, this.ProxyUrl);

            if (this.RequestMethod==HttpRequestMethod.POST)//REST的POST
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                
                request.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(PostBody);
                TaskFactory factory = new TaskFactory();
                Stream stream = await factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null);
                await stream.WriteAsync(data, 0, data.Length);
                stream.Dispose();
            }
            else
            {
                request.Method = "GET";
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
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

        /// <summary>${WP_mapping_ServiceRequest_attribute_forcePost_D}</summary>
        internal HttpRequestMethod RequestMethod { get; set; }

        /// <summary>${WP_mapping_ServiceRequest_attribute_Url_D}</summary>
        internal string Url { get; set; }
        /// <summary>${WP_mapping_ServiceRequest_attribute_ProxyUrl_D}</summary>
        internal string ProxyUrl { get; set; }
        /// <summary>${WP_mapping_ServiceRequest_attribute_Parameters_D}</summary>
        internal string PostBody { get; set; }

    }

    
}
