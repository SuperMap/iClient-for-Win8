using System;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTransferSolutionsParameter_Title}</para>
    /// </summary>
    public class FindTransferSolutionsParameter
    {
        /// <summary>${REST_FindTransferSolutionsParameter_constructor_D}</summary>
        public FindTransferSolutionsParameter()
        {
            SolutionCount = 5;
            TransferTactic = TransferTactic.LESS_TIME;
            TransferPreference = TransferPreference.NONE;
            WalkingRatio = 10;
        }
        /// <summary>${REST_FindTransferSolutionsParameter_attribute_SolutionCount_D}</summary>
        public int SolutionCount
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferSolutionsParameter_attribute_TransferTactic_D}</summary>
        public TransferTactic TransferTactic
        {
            get;
            set;
        }

        /// <summary>${REST_FindTransferSolutionsParameter_attribute_TransferPreference_D}</summary>
        public TransferPreference TransferPreference
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferSolutionsParameter_attribute_WalkingRatio_D}</summary>
        public double WalkingRatio
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferSolutionsParameter_attribute_Points_D}</summary>
        public Array Points
        {
            get;
            set;
        }
    }
}
