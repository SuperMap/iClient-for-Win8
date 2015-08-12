using System;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindStopParameter_Title}</para>
    /// </summary>
    public class FindStopParameter
    {
        /// <summary>${REST_FindStopParameter_attribute_KeyWord_D}</summary>
        public string KeyWord { get; set; }

        /// <summary>${REST_FindStopParameter_attribute_ReturnPosition_D}</summary>
        public bool ReturnPosition { get; set; }

        /// <summary>${REST_FindStopParameter_constructor_D}</summary>
        public FindStopParameter()
        {

        }

        /// <summary>${REST_FindStopParameter_constructor_String_D}</summary>
        /// <param name="keyword">${REST_FindStopParameter_constructor_param_keyword}</param>
        /// <param name="returnPosition">${REST_FindStopParameter_constructor_param_returnPosition}</param>
        public FindStopParameter(string keyword, bool returnPosition)
        {
            this.KeyWord = keyword;
            this.ReturnPosition = returnPosition;
        }

    }
}
