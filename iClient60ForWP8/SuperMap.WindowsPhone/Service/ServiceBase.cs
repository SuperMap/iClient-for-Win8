using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using SuperMap.WindowsPhone.Resources;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Service
{
    /// <summary>
    /// 	<para>${WP_mapping_ServiceBase_Tile}</para>
    /// 	<para>${WP_mapping_ServiceBase_Description}</para>
    /// </summary>
    public abstract class ServiceBase : INotifyPropertyChanged
    {
        private ServiceRequest request;
        

        /// <summary>${WP_mapping_ServiceBase_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${WP_mapping_ServiceBase_constructor_None_D}</summary>
        protected ServiceBase()
            : this(null)
        {
        }

        /// <summary>${WP_mapping_ServiceBase_constructor_string_D}</summary>
        protected ServiceBase(string url)
        {
            Url = url;
            request = new ServiceRequest();
        }

        protected async Task<string> SubmitRequest(string url)
        {
            return await SubmitRequest(url, HttpRequestMethod.GET);
        }
        
        /// <param name="url">${WP_mapping_ServiceBase_method_SubmitRequest_param_url_D}</param>
        /// <param name="parameters">${WP_mapping_ServiceBase_method_SubmitRequest_param_parameters_D}</param>
        /// <param name="onCompleted">${WP_mapping_ServiceBase_method_SubmitRequest_param_onCompleted_D}</param>
        /// <param name="state">${WP_mapping_ServiceBase_method_SubmitRequest_param_state_D}</param>
        /// <param name="forcePost">${WP_mapping_ServiceBase_method_SubmitRequest_param_forcePost_D}</param>
        /// <summary>${WP_mapping_ServiceBase_method_SubmitRequest_D}</summary>
        /// <overloads>${WP_mapping_ServiceBase_method_SubmitRequest_overloads_D}</overloads>
        protected async Task<string> SubmitRequest(string url, HttpRequestMethod method)
        {
            return await SubmitRequest(url, method,string.Empty);
        }

        /// <param name="url">${WP_mapping_ServiceBase_method_SubmitRequest_param_url_D}</param>
        /// <param name="parameters">${WP_mapping_ServiceBase_method_SubmitRequest_param_parameters_D}</param>
        /// <param name="onCompleted">${WP_mapping_ServiceBase_method_SubmitRequest_param_onCompleted_D}</param>
        /// <param name="state">${WP_mapping_ServiceBase_method_SubmitRequest_param_state_D}</param>
        /// <param name="forcePost">${WP_mapping_ServiceBase_method_SubmitRequest_param_forcePost_D}</param>
        /// <param name="isEditable">${WP_mapping_ServiceBase_method_SubmitRequest_param_isEditable_D}</param>
        /// <param name="isTempLayersSet">${WP_mapping_ServiceBase_method_SubmitRequest_param_isTempLayersSet_D}</param>
        /// <summary>${WP_mapping_ServiceBase_method_SubmitRequest_D}</summary>
        /// <overloads>${WP_mapping_ServiceBase_method_SubmitRequest_overloads_D}</overloads>
        protected async Task<string> SubmitRequest(string url, HttpRequestMethod method,string data)
        {
            request.Url = url;
            request.PostBody = data;
            request.ProxyUrl = ProxyURL;
            request.RequestMethod = method;
            return await request.BeginRequest();
        }

        /// <summary>${WP_mapping_ServiceBase_method_OnPropertyChanged_D }</summary>
        /// <param name="name">${WP_mapping_ServiceBase_method_OnPropertyChanged_param_name_D}</param>
        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>${WP_mapping_ServiceBase_attribute_DisableClientCaching_D}</summary>
        public bool DisableClientCaching { get; set; }
        
        /// <summary>${WP_mapping_ServiceBase_attribute_ProxyURL_D}</summary>
        public string ProxyURL { get; set; }
        /// <summary>${WP_mapping_ServiceBase_attribute_Token_D}</summary>
        public string Token { get; set; }
        /// <summary>${WP_mapping_ServiceBase_attribute_Url_D}</summary>
        public string Url { get; set; }
    }
}
