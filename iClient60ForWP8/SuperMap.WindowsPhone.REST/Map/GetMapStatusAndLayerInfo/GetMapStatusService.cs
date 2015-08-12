using Newtonsoft.Json;
using SuperMap.WindowsPhone.Service;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_GetMapStatusService_Title}</para>
    /// 	<para>${WP_REST_GetMapStatusService_Description}</para>
    /// </summary>
    public class GetMapStatusService : ServiceBase
    {
        private int wkid;
        /// <summary>${WP_REST_GetMapStatusService_Title}</summary>
        /// <overloads>${WP_REST_GetMapStatusService_Description}</overloads>
        public GetMapStatusService()
        { }
        /// <summary>${WP_REST_GetMapStatusService_constructor_String_D}</summary>
        /// <param name="url">${WP_REST_GetMapStatusService_constructor_String_param_url}</param>
        public GetMapStatusService(string url)
            : base(url)
        { }

        public GetMapStatusService(string url, int wkid)
            : base(url)
        {
            this.wkid = wkid;
        }

        //url=http://localhost:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图
        //absoluteurl=http://192.168.11.11:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图.rjson
        /// <summary>${WP_REST_GetMapStatusService_method_ProcessAsync_D}</summary>
        /// <param name="state">${WP_REST_GetMapStatusService_method_ProcessAsync_param_state}</param>
        public async Task<GetMapStatusResult> ProcessAsync()
        {
            CheckUrl();

            var json = await base.SubmitRequest(Url,HttpRequestMethod.GET);
            GetMapStatusResult result = JsonConvert.DeserializeObject<GetMapStatusResult>(json);
            LastResult = result;
            return result;
        }

        private void CheckUrl()
        {
            if (Url.EndsWith("/"))
            {
                Url.TrimEnd(new char[] { '/' });
            }
            if (this.wkid > 0)
            {
                this.Url = this.Url + ".json?prjCoordSys={\"epsgCode\":" + this.wkid + "}";
            }
            else
            {
                this.Url = this.Url + ".json";
            }
        }

        private GetMapStatusResult lastResult;
        /// <summary>${WP_REST_GetMapStatusResult_attribute_lastResult_D}</summary>
        public GetMapStatusResult LastResult
        {
            get
            {
                return lastResult;
            }
            private set
            {
                lastResult = value;
                base.OnPropertyChanged("LastResult");
            }
        }
    }
}
