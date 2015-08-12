using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ColorGradientType_Title}</para>
    /// 	<para>${REST_ColorGradientType_Description}</para>
    /// </summary>
    public enum ColorGradientType {
        /// <summary>${REST_ColorGradientType_attribute_BlackWhite_D}</summary>
        BLACKWHITE,
        //黑白渐变色，对应值为0。
        /// <summary>${REST_ColorGradientType_attribute_BlueBlack_D}</summary>
        BLUEBLACK,
        //蓝黑渐变色，对应值为9。
        /// <summary>${REST_ColorGradientType_attribute_BlueRed_D}</summary>
        BLUERED,
        //蓝红渐变色，对应值为18。
        /// <summary>${REST_ColorGradientType_attribute_BlueWhite_D}</summary>
        BLUEWHITE,
        //蓝白渐变色，对应值为3。
        /// <summary>${REST_ColorGradientType_attribute_CyanBlack_D}</summary>
        CYANBLACK,
        //青黑渐变色，对应值为12。
        /// <summary>${REST_ColorGradientType_attribute_CyanBlue_D}</summary>
        CYANBLUE,
        //青蓝渐变色，对应值为21。
        /// <summary>${REST_ColorGradientType_attribute_CyanGreen_D}</summary>
        CYANGREEN,
        //青绿渐变色，对应值为22。
        /// <summary>${REST_ColorGradientType_attribute_CyanWhite_D}</summary>
        CYANWHITE,
        //青白渐变色，对应值为6。
        /// <summary>${REST_ColorGradientType_attribute_GreenBlack_D}</summary>
        GREENBLACK,
        //绿黑渐变色，对应值为8。
        /// <summary>${REST_ColorGradientType_attribute_GreenBlue_D}</summary>
        GREENBLUE,
        //绿蓝渐变色，对应值为16。
        /// <summary>${REST_ColorGradientType_attribute_GreenOrangeViolet_D}</summary>
        GREENORANGEVIOLET,
        //绿橙紫渐变色，对应值为24。
        /// <summary>${REST_ColorGradientType_attribute_GreenRed_D}</summary>
        GREENRED,
        //绿红渐变色，对应值为17。
        /// <summary>${REST_ColorGradientType_attribute_GreenWhite_D}</summary>
        GREENWHITE,
        //绿白渐变色，对应值为2。
        /// <summary>${REST_ColorGradientType_attribute_PinkBlack_D}</summary>
        PINKBLACK,
        //粉红到黑渐变色，对应值为11。
        /// <summary>${REST_ColorGradientType_attribute_PinkBlue_D}</summary>
        PINKBLUE,
        //粉红到蓝渐变色，对应值为20。
        /// <summary>${REST_ColorGradientType_attribute_PinkRed_D}</summary>
        PINKRED,
        //粉红到红渐变色，对应值为19。
        /// <summary>${REST_ColorGradientType_attribute_Pinkwhite_D}</summary>
        PINKWHITE,
        //粉红到白渐变色，对应值为5。
        /// <summary>${REST_ColorGradientType_attribute_RainBow_D}</summary>
        RAINBOW,
        //彩虹色，对应值为23。
        /// <summary>${REST_ColorGradientType_attribute_RedBlack_D}</summary>
        REDBLACK,
        //红黑渐变色，对应值为7。
        /// <summary>${REST_ColorGradientType_attribute_RedWhite_D}</summary>
        REDWHITE,
        //红白渐变色，对应值为1。
        /// <summary>${REST_ColorGradientType_attribute_Spectrum_D}</summary>
        SPECTRUM,
        //光谱渐变色，对应值为26。
        /// <summary>${REST_ColorGradientType_attribute_Terrain_D}</summary>
        TERRAIN,
        //地形渐变色，对应值为25。
        /// <summary>${REST_ColorGradientType_attribute_YellowBlack_D}</summary>
        YELLOWBLACK,
        //黄黑渐变色，对应值为10。
        /// <summary>${REST_ColorGradientType_attribute_YellowBlue_D}</summary>
        YELLOWBLUE,
        //黄蓝渐变色，对应值为15。
        /// <summary>${REST_ColorGradientType_attribute_YellowGreen_D}</summary>
        YELLOWGREEN,
        //黄绿渐变色，对应值为14。
        /// <summary>${REST_ColorGradientType_attribute_YellowRed_D}</summary>
        YELLOWRED,
        //黄红渐变色，对应值为13。
        /// <summary>${REST_ColorGradientType_attribute_YellowWhite_D}</summary>
        YELLOWWHITE,
        //黄白渐变色，对应值为4。

    }

}