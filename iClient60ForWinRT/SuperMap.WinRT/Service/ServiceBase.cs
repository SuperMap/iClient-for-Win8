using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using SuperMap.WinRT.Resources;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Service
{
    /// <summary>
    /// 	<para>${mapping_ServiceBase_Tile}</para>
    /// 	<para>${mapping_ServiceBase_Description}</para>
    /// </summary>
    public abstract class ServiceBase : INotifyPropertyChanged
    {
        private ServiceRequest request;
        ///// <summary>${mapping_ServiceBase_event_Failed_D}</summary>
        //public event EventHandler<ServiceFailedEventArgs> Failed;

        /// <summary>${mapping_ServiceBase_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${mapping_ServiceBase_constructor_None_D}</summary>
        protected ServiceBase()
            : this(null)
        {
        }

        /// <summary>${mapping_ServiceBase_constructor_string_D}</summary>
        protected ServiceBase(string url)
        {
            Url = url;
            request = new ServiceRequest();
            //request.Completed += new EventHandler<RequestEventArgs>(request_Completed);
            //request.Failed += new EventHandler<RequestEventArgs>(request_Failed);
        }
        
        //这里改了很多东西，提供一个重载就好了！

        /// <param name="url">${mapping_ServiceBase_method_SubmitRequest_param_url_D}</param>
        /// <param name="parameters">${mapping_ServiceBase_method_SubmitRequest_param_parameters_D}</param>
        /// <param name="onCompleted">${mapping_ServiceBase_method_SubmitRequest_param_onCompleted_D}</param>
        /// <param name="state">${mapping_ServiceBase_method_SubmitRequest_param_state_D}</param>
        /// <param name="forcePost">${mapping_ServiceBase_method_SubmitRequest_param_forcePost_D}</param>
        /// <summary>${mapping_ServiceBase_method_SubmitRequest_D}</summary>
        /// <overloads>${mapping_ServiceBase_method_SubmitRequest_overloads_D}</overloads>
        protected async Task<string> SubmitRequest(string url, Dictionary<string, string> parameters, bool forcePost)
        {
            return await SubmitRequest(url, parameters, forcePost, false);
        }

        /// <param name="url">${mapping_ServiceBase_method_SubmitRequest_param_url_D}</param>
        /// <param name="parameters">${mapping_ServiceBase_method_SubmitRequest_param_parameters_D}</param>
        /// <param name="onCompleted">${mapping_ServiceBase_method_SubmitRequest_param_onCompleted_D}</param>
        /// <param name="state">${mapping_ServiceBase_method_SubmitRequest_param_state_D}</param>
        /// <param name="forcePost">${mapping_ServiceBase_method_SubmitRequest_param_forcePost_D}</param>
        /// <param name="isEditable">${mapping_ServiceBase_method_SubmitRequest_param_isEditable_D}</param>
        /// <summary>${mapping_ServiceBase_method_SubmitRequest_D}</summary>
        /// <overloads>${mapping_ServiceBase_method_SubmitRequest_overloads_D}</overloads>
        protected async Task<string> SubmitRequest(string url, Dictionary<string, string> parameters, bool forcePost, bool isEditable)
        {
            return await SubmitRequest(url, parameters, forcePost, isEditable, false);
        }

        /// <param name="url">${mapping_ServiceBase_method_SubmitRequest_param_url_D}</param>
        /// <param name="parameters">${mapping_ServiceBase_method_SubmitRequest_param_parameters_D}</param>
        /// <param name="onCompleted">${mapping_ServiceBase_method_SubmitRequest_param_onCompleted_D}</param>
        /// <param name="state">${mapping_ServiceBase_method_SubmitRequest_param_state_D}</param>
        /// <param name="forcePost">${mapping_ServiceBase_method_SubmitRequest_param_forcePost_D}</param>
        /// <param name="isEditable">${mapping_ServiceBase_method_SubmitRequest_param_isEditable_D}</param>
        /// <param name="isTempLayersSet">${mapping_ServiceBase_method_SubmitRequest_param_isTempLayersSet_D}</param>
        /// <summary>${mapping_ServiceBase_method_SubmitRequest_D}</summary>
        /// <overloads>${mapping_ServiceBase_method_SubmitRequest_overloads_D}</overloads>
        protected async Task<string> SubmitRequest(string url, Dictionary<string, string> parameters, bool forcePost, bool isEditable, bool isTempLayersSet)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            if (!forcePost)
                AppendBaseQueryParameters(parameters);

            request.Url = url;
            request.Parameters = parameters;
            request.ProxyUrl = ProxyURL;
            request.ForcePost = forcePost;
            request.IsEditable = isEditable;
            request.IsTempLayersSet = isTempLayersSet;
            return await request.BeginRequest();
        }

        protected async Task<string> SubmitRequest(string url, string postBody, bool isEditable, bool isTempLayersSet)
        {
            request.Url = url;
            request.PostBody = postBody;
            request.ProxyUrl = ProxyURL;
            request.ForcePost = true;
            request.IsEditable = isEditable;
            request.IsTempLayersSet = isTempLayersSet;
            return await request.BeginRequest();
        }

        private void AppendBaseQueryParameters(Dictionary<string, string> parameters)
        {
            if (DisableClientCaching)
            {
                parameters.Add("t", string.Format(CultureInfo.InvariantCulture, DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
            }

            if (!string.IsNullOrEmpty(Token))
            {
                parameters.Add("token", Token);
            }
        }

        /// <summary>${mapping_ServiceBase_method_OnPropertyChanged_D }</summary>
        /// <param name="name">${mapping_ServiceBase_method_OnPropertyChanged_param_name_D}</param>
        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        //internal void OnProcessFailed(ServiceFailedEventArgs args)
        //{
        //    if (this.Failed != null)
        //    {
        //        this.Failed(this, args);
        //    }
        //}

        //private void request_Failed(object sender, RequestEventArgs e)
        //{
        //    Exception inner = e.Error;
        //    //if (e.Error is SecurityException)
        //    //{
        //    //    //TODO:资源
        //    //    inner = new SecurityException(ExceptionStrings.ServiceAddressError, inner);
        //    //    OnProcessFailed(new ServiceFailedEventArgs(inner, e.UserState));
        //    //}
        //    //else
        //    //{
        //    //    OnProcessFailed(new ServiceFailedEventArgs(inner, e.UserState));
        //    //}

        //    //服务端返回NotFound
        //    ServiceException ex = new ServiceException(404, e.Error.Message);
        //    OnProcessFailed(new ServiceFailedEventArgs(ex, e.UserState));
        //}

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    if (!CheckForServiceError(e))
        //    {
        //        object[] userState = (object[])e.UserState;
        //        EventHandler<RequestEventArgs> handler = userState[1] as EventHandler<RequestEventArgs>;
        //        if (handler != null)
        //        {
        //            handler(this, new RequestEventArgs(userState[0], e.Result));
        //        }
        //    }
        //}

        //private bool CheckForServiceError(RequestEventArgs e)
        //{
        //    ServiceException error = ServiceException.CreateFromJSON(e.Result);
        //    if (error != null)
        //    {
        //        OnProcessFailed(new ServiceFailedEventArgs(error, e.UserState));
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>${mapping_ServiceBase_attribute_DisableClientCaching_D}</summary>
        public bool DisableClientCaching { get; set; }
        
        /// <summary>${mapping_ServiceBase_attribute_ProxyURL_D}</summary>
        public string ProxyURL { get; set; }
        /// <summary>${mapping_ServiceBase_attribute_Token_D}</summary>
        public string Token { get; set; }
        /// <summary>${mapping_ServiceBase_attribute_Url_D}</summary>
        public string Url { get; set; }
    }
}
