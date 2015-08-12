using System;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTransferPathParameter_Title}</para>
    /// </summary>
    public class FindTransferPathParameter
    {
        /// <summary>${REST_FindTransferPathParameter_constructor_D}</summary>
        public FindTransferPathParameter()
        {

        }
        /// <summary>${REST_FindTransferPathParameter_attribute_Points_D}</summary>
        public Array Points
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferPathParameter_attribute_TransferLine_D}</summary>
        public TransferLine[] TransferLines
        {
            get;
            set;
        }

    }
}
