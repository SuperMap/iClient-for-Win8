using System;

namespace SuperMap.Web.OGC
{
    /// <summary>${mapping_ResultEventArgs_Tile}</summary>
    public class ResultEventArgs<T> : EventArgs
    {
        /// <summary>${mapping_ResultEventArgs_constructor_D}</summary>
        internal ResultEventArgs(T result, string originRes, object userState)
        {
            this.Result = result;
            this.OriginResult = originRes;
            this.UserState = userState;
        }
        /// <summary>${mapping_ResultEventArgs_attribute_Result_D}</summary>
        public T Result { get; private set; }
        /// <summary>${mapping_ResultEventArgs_attribute_OriginResult_D}</summary>
        public string OriginResult { get; private set; }
        /// <summary>${mapping_ResultEventArgs_attribute_UserState_D}</summary>
        public object UserState { get; private set; }
    }
}
