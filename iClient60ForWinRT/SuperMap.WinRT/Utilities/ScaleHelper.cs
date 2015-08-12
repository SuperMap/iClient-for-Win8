using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Resources;
using Windows.Foundation;

namespace SuperMap.WinRT.Utilities
{
    /// <summary>
    /// 	<para>${utility_ScaleHelper_Title}</para>
    /// 	<para>${utility_ScaleHelper_Description}</para>
    /// </summary>
    public static class ScaleHelper
    {
        internal static double[] CheckAndSortScales(double[] scales)
        {
            if (scales != null && scales.Length > 0)
            {
                if (scales[0] > 1)
                {
                    for (int i = 0; i < scales.Length; i++)
                    {
                        scales[i] = 1.0 / scales[i];
                    }
                }

                List<double> list = new List<double>();
                foreach (double item in scales)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
                list.Sort();
                return list.ToArray();
            }
            return null;
        }

        internal static double[] CheckAndSortResolutions(double[] resolutions)
        {
            if (resolutions != null && resolutions.Length > 0)
            {
                //从大到小排序
                List<double> list = new List<double>();
                foreach (double item in resolutions)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
                list.Sort();
                list.Reverse();
                return list.ToArray();
            }
            return null;
        }

        //短的数列  一定是 长的数列  的子集
        internal static void CheckResolutionsMatching(double[] mapResArray, double[] layerResArray)
        {
            if (mapResArray != null && layerResArray != null)
            {
                double[] shortArray;
                double[] longArray;
                if (mapResArray.Length > layerResArray.Length)
                {
                    shortArray = layerResArray;
                    longArray = mapResArray;
                }
                else
                {
                    shortArray = mapResArray;
                    longArray = layerResArray;
                }

                List<double> longList = new List<double>();
                foreach (double item in longArray)
                {
                    longList.Add(item);
                }
                foreach (double item in shortArray)
                {
                    int index = MathUtil.GetIndex(item, longList.ToArray());
                    if (index == -1)
                    {
                        throw new ArgumentException(ExceptionStrings.InvalidMatch);
                    }
                }
            }
        }

        //TODO:需要改，没用好
        internal static double[] ConversionBetweenScalesAndResulotions(double[] temp, double dpi, CoordinateReferenceSystem crs)
        {
            if (dpi > 0 && temp != null && temp.Length > 0)
            {
                double[] numArray = new double[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    numArray[i] = ScaleConversion(temp[i], dpi, crs);
                }
                return numArray;
            }
            return null;
        }
        /// <summary>${utility_ScaleHelper_method_ScaleConversion_D}</summary>
        /// <param name="input">${utility_ScaleHelper_method_ScaleConversion_param_input}</param>
        /// <param name="dpi">${utility_ScaleHelper_method_ScaleConversion_param_dpi}</param>
        /// <param name="crs">${utility_ScaleHelper_method_ScaleConversion_param_crs}</param>
        /// <returns>${utility_ScaleHelper_method_ScaleConversion_return}</returns>
        public static double ScaleConversion(double input, double dpi, CoordinateReferenceSystem crs)
        {
            CoordinateReferenceSystem tempCRS = crs;
            if (tempCRS == null)
            {
                tempCRS = new CoordinateReferenceSystem();
            }
            if (tempCRS.DatumAxis <= 0)
            {
                tempCRS.DatumAxis = 6378137;
            }
            if (tempCRS.Unit == Unit.Undefined)
            {
                tempCRS.Unit = Unit.Degree;
            }
            if (dpi > 0.0 && input > 0.0)
            {
                if (tempCRS.Unit == Unit.Degree)
                {
                    input *= ((Math.PI * 2 * tempCRS.DatumAxis) / 360);

                }
                var scale = 0.0254 / input / dpi;
                return scale;
            }
            return double.NaN;
        }
        /// <summary>
        /// ${utility_ScaleHelper_method_GetSmDpi_D}</summary>
        /// <remarks>${utility_ScaleHelper_method_GetSmDpi_remarks}</remarks>
        /// <param name="referViewBounds">${utility_ScaleHelper_method_GetSmDpi_param_referViewBounds}</param>
        /// <param name="referViewer">${utility_ScaleHelper_method_GetSmDpi_param_referViewer}</param>
        ///  <param name="referScale">${utility_ScaleHelper_method_GetSmDpi_param_referScale}</param>
        ///  <param name="crs">${utility_ScaleHelper_method_GetSmDpi_param_crs}</param>
        ///  <returns>${utility_ScaleHelper_method_GetSmDpi_return}</returns>
        public static double GetSmDpi(Rectangle2D referViewBounds, Rect referViewer, double referScale, CoordinateReferenceSystem crs)
        {
            CoordinateReferenceSystem tempCRS = crs;
            if (tempCRS == null)
            {
                tempCRS = new CoordinateReferenceSystem();
            }
            if (tempCRS.DatumAxis <= 0)
            {
                tempCRS.DatumAxis = 6378137;
            }
            if (tempCRS.Unit == Unit.Undefined)
            {
                tempCRS.Unit = Unit.Degree;
            }
            int ratio = 10000;
            //10000 是 0.1毫米与米的转换，底层UGC有个int变换，换回去即可


            //double nD1 = referViewBounds.Width * ratio * referScale;//逻辑宽度（单位：0.1毫米），比例尺=图上距离：实际距离，
            //这里所计算的即为referViewBounds.width代表的图上宽度。

            //int nD3 = (nD1 > 0.0) ? (int)(nD1 + 0.5) : (int)(nD1 - 0.5);//判断当前地图单位是否为经纬度，若逻辑宽度nD1的值介于-0.5~0.5之间，则认为地图单位为经纬度
            double num1 = referViewBounds.Width / referViewer.Width;//横向分辨率
            double num2 = referViewBounds.Height / referViewer.Height;//纵向分辨率

            //地图单位为经纬度
            if (tempCRS.Unit == Unit.Degree)
            {
                double referResolution = num1 > num2 ? num1 : num2;//取横向或纵向分辨率中的较大者，用于计算DPI
                var dpi = 0.0254 * ratio / referResolution / referScale / ((Math.PI * 2 * tempCRS.DatumAxis) / 360) / ratio;
                return dpi;
            }
            else
            {
                var dpi = 0.0254 * ratio / num1 / referScale / ratio;
                return dpi;
            }
        }
    }
}
