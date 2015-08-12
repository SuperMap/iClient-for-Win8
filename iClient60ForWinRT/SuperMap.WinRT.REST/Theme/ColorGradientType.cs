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
        //�ڰ׽���ɫ����ӦֵΪ0��
        /// <summary>${REST_ColorGradientType_attribute_BlueBlack_D}</summary>
        BLUEBLACK,
        //���ڽ���ɫ����ӦֵΪ9��
        /// <summary>${REST_ColorGradientType_attribute_BlueRed_D}</summary>
        BLUERED,
        //���콥��ɫ����ӦֵΪ18��
        /// <summary>${REST_ColorGradientType_attribute_BlueWhite_D}</summary>
        BLUEWHITE,
        //���׽���ɫ����ӦֵΪ3��
        /// <summary>${REST_ColorGradientType_attribute_CyanBlack_D}</summary>
        CYANBLACK,
        //��ڽ���ɫ����ӦֵΪ12��
        /// <summary>${REST_ColorGradientType_attribute_CyanBlue_D}</summary>
        CYANBLUE,
        //��������ɫ����ӦֵΪ21��
        /// <summary>${REST_ColorGradientType_attribute_CyanGreen_D}</summary>
        CYANGREEN,
        //���̽���ɫ����ӦֵΪ22��
        /// <summary>${REST_ColorGradientType_attribute_CyanWhite_D}</summary>
        CYANWHITE,
        //��׽���ɫ����ӦֵΪ6��
        /// <summary>${REST_ColorGradientType_attribute_GreenBlack_D}</summary>
        GREENBLACK,
        //�̺ڽ���ɫ����ӦֵΪ8��
        /// <summary>${REST_ColorGradientType_attribute_GreenBlue_D}</summary>
        GREENBLUE,
        //��������ɫ����ӦֵΪ16��
        /// <summary>${REST_ColorGradientType_attribute_GreenOrangeViolet_D}</summary>
        GREENORANGEVIOLET,
        //�̳��Ͻ���ɫ����ӦֵΪ24��
        /// <summary>${REST_ColorGradientType_attribute_GreenRed_D}</summary>
        GREENRED,
        //�̺콥��ɫ����ӦֵΪ17��
        /// <summary>${REST_ColorGradientType_attribute_GreenWhite_D}</summary>
        GREENWHITE,
        //�̰׽���ɫ����ӦֵΪ2��
        /// <summary>${REST_ColorGradientType_attribute_PinkBlack_D}</summary>
        PINKBLACK,
        //�ۺ쵽�ڽ���ɫ����ӦֵΪ11��
        /// <summary>${REST_ColorGradientType_attribute_PinkBlue_D}</summary>
        PINKBLUE,
        //�ۺ쵽������ɫ����ӦֵΪ20��
        /// <summary>${REST_ColorGradientType_attribute_PinkRed_D}</summary>
        PINKRED,
        //�ۺ쵽�콥��ɫ����ӦֵΪ19��
        /// <summary>${REST_ColorGradientType_attribute_Pinkwhite_D}</summary>
        PINKWHITE,
        //�ۺ쵽�׽���ɫ����ӦֵΪ5��
        /// <summary>${REST_ColorGradientType_attribute_RainBow_D}</summary>
        RAINBOW,
        //�ʺ�ɫ����ӦֵΪ23��
        /// <summary>${REST_ColorGradientType_attribute_RedBlack_D}</summary>
        REDBLACK,
        //��ڽ���ɫ����ӦֵΪ7��
        /// <summary>${REST_ColorGradientType_attribute_RedWhite_D}</summary>
        REDWHITE,
        //��׽���ɫ����ӦֵΪ1��
        /// <summary>${REST_ColorGradientType_attribute_Spectrum_D}</summary>
        SPECTRUM,
        //���׽���ɫ����ӦֵΪ26��
        /// <summary>${REST_ColorGradientType_attribute_Terrain_D}</summary>
        TERRAIN,
        //���ν���ɫ����ӦֵΪ25��
        /// <summary>${REST_ColorGradientType_attribute_YellowBlack_D}</summary>
        YELLOWBLACK,
        //�ƺڽ���ɫ����ӦֵΪ10��
        /// <summary>${REST_ColorGradientType_attribute_YellowBlue_D}</summary>
        YELLOWBLUE,
        //��������ɫ����ӦֵΪ15��
        /// <summary>${REST_ColorGradientType_attribute_YellowGreen_D}</summary>
        YELLOWGREEN,
        //���̽���ɫ����ӦֵΪ14��
        /// <summary>${REST_ColorGradientType_attribute_YellowRed_D}</summary>
        YELLOWRED,
        //�ƺ콥��ɫ����ӦֵΪ13��
        /// <summary>${REST_ColorGradientType_attribute_YellowWhite_D}</summary>
        YELLOWWHITE,
        //�ư׽���ɫ����ӦֵΪ4��

    }

}