using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Resources;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_utility_ScaleHelper_Title}</para>
    /// 	<para>${WP_utility_ScaleHelper_Description}</para>
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
            if (crs == null)
            {
                crs = new CoordinateReferenceSystem();
            }
            if (crs.Unit == Unit.Undefined)
            {
                crs.Unit = Unit.Degree;
            }
            if (crs.DatumAxis <= 0)
            {
                crs.DatumAxis = 6378137;
            }
            if (dpi > 0 && temp != null && temp.Length > 0)
            {
                double[] numArray = new double[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    numArray[i] = ScaleConversion(temp[i], dpi,crs);
                }
                return numArray;
            }
            return null;
        }
        
        /// <summary>
        /// ${WP_utility_ScaleHelper_method_ScaleConversion_D}
        /// </summary>
        /// <param name="input">${WP_utility_ScaleHelper_method_ScaleConversion_param_input}</param>
        /// <param name="dpi">${WP_utility_ScaleHelper_method_ScaleConversion_param_dpi}</param>
        /// <param name="crs">&{WP_utility_ScaleHelper_method_ScaleConversion_param_crs}</param>
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

        /// <summary>${WP_utility_ScaleHelper_method_GetSmDpi_D}</summary>
        /// <remarks>${WP_utility_ScaleHelper_method_GetSmDpi_remarks}</remarks>
        /// <param name="referViewBounds">${WP_utility_ScaleHelper_method_GetSmDpi_param_referViewBounds}</param>
        /// <param name="referViewer">${WP_utility_ScaleHelper_method_GetSmDpi_param_referViewer}</param>
        ///  <param name="referScale">${WP_utility_ScaleHelper_method_GetSmDpi_param_referScale}</param>
        ///  <param name="crs">${WP_utility_ScaleHelper_method_GetSmDpi_param_crs}</param>
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
            double num1 = referViewBounds.Width / referViewer.Width;//横向分辨率
            double num2 = referViewBounds.Height / referViewer.Height;//纵向分辨率
            if (crs.Unit == Unit.Degree)
            {
                double referResolution = num1 > num2 ? num1 : num2;//取横向或纵向分辨率中的较大者，用于计算DPI
                var dpi = 0.0254 * ratio / referResolution / referScale / ((Math.PI * 2 * crs.DatumAxis) / 360) / ratio;
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
