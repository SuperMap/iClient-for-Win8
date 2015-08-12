using System;

namespace SuperMap.WinRT.OGC
{
    /// <summary>${mapping_RequestException_Tile}</summary>
    public class RequestException : Exception
    {
        /// <summary> ${mapping_RequestException_constructor_D} </summary>
        /// <param name="code">${mapping_RequestException_constructor_param_code}</param>
        /// <param name="locator">${mapping_RequestException_constructor_param_locator}</param>
        /// <param name="message">${mapping_RequestException_constructor_param_message}</param>
        public RequestException(string code, string locator, string message)
        {
            this.Code = code;
            this.Locator = locator;
            this.Message = message;
        }
        /// <summary> ${mapping_RequestException_constructor_D} </summary>
        /// <param name="other">${mapping_RequestException_constructor_param_other}</param>
        public RequestException(Exception other)
        {
            this.OtherMessage = other;
        }
        /// <summary> ${mapping_RequestException_constructor_param_code} </summary>
        public string Code { internal set; get; }
        /// <summary> ${mapping_RequestException_attribute_Locator_D} </summary>
        public string Locator { get; internal set; }
        /// <summary> ${mapping_RequestException_attribute_Message_D} </summary>
        public string Message { get; internal set; }
        /// <summary> ${mapping_RequestException_attribute_OtherMessage_D} </summary>
        public Exception OtherMessage { get; internal set; }
    }
}
