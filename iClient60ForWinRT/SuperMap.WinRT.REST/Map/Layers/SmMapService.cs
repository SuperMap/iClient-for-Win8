
using System;
using System.Net;
using System.Security;
using SuperMap.WinRT.Resources;
using System.IO;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    internal class SmMapService
    {
        public SmMapService(string mapServiceUrl)
        {
            MapServiceUrl = mapServiceUrl;
        }

        //方法
        public async Task Initialize(int wkid)
        {
            string url = string.Format("{0}.json", MapServiceUrl) + "?prjCoordSys={" + string.Format("\"epsgCode\":{0}", wkid) + "}";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync().ConfigureAwait(false);
                reader.Dispose();
                response.Dispose();
                request.Abort();
                DownloadStringCompleted(result);
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
        //方法
        public async Task Initialize()
        {
            string url = string.Format("{0}.json", MapServiceUrl);
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync().ConfigureAwait(false);
                reader.Dispose();
                response.Dispose();
                request.Abort();
                DownloadStringCompleted(result);
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

        private SmMapServiceInfo DownloadStringCompleted(string result)
        {
            JsonObject json = JsonObject.Parse(result);
            MapServiceInfo = SmMapServiceInfo.FromJson(json);
            return MapServiceInfo;
        }

        //属性
        public SmMapServiceInfo MapServiceInfo { get; private set; }
        public string MapServiceUrl { get; private set; }

        ////事件
        //internal class MapServiceInitalizeArgs : EventArgs
        //{
        //    public SmMapService MapService { get; set; }
        //}
        //internal event EventHandler<MapServiceInitalizeArgs> Initialized;

        //internal class MapServiceFaultEventArgs
        //{
        //    public bool Cancelled { get; internal set; }

        //    public Exception Error { get; internal set; }
        //}
        //internal delegate void MapServiceFaultEventHandler(object sender, MapServiceFaultEventArgs args);
        //internal event MapServiceFaultEventHandler Failed;
    }
}
