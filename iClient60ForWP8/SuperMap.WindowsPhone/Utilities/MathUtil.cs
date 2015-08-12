using System;

namespace SuperMap.WindowsPhone.Utilities
{
    internal static class MathUtil
    {
        public static double GetNearest(double resolution, double[] resolutions, double minRes, double maxRes)
        {
            if (resolutions == null || resolutions.Length < 1)
            {
                return resolution;
            }
            int index;
            if (double.IsNaN(minRes) || double.IsNaN(maxRes))
            {
                index = GetNearestIndex(resolution, resolutions);
            }
            else
            {
                index = GetNearestIndex(resolution, resolutions, minRes, maxRes);
            }
            return resolutions[index];
        }

        public static int GetNearestIndex(double resolution, double[] resolutions, double minRes, double maxRes)
        {
            int i = GetNearestIndex(resolution, resolutions);
            if (!double.IsNaN(minRes) && !double.IsNaN(maxRes) && i != -1)
            {
                if (resolutions[i] < minRes)
                {
                    i--;
                }
                else if (resolutions[i] > maxRes)
                {
                    i++;
                }
                i = (i < 0) ? 0 : (i >= resolutions.Length ? resolutions.Length - 1 : i);
            }
            return i;
        }

        public static int GetIndex(double resolution, double[] resolutions)
        {
            int index = -1;
            if (resolutions != null)
            {
                for (int i = 0; i < resolutions.Length; i++)
                {
                    if (DoubleUtil.AreClose(resolution, resolutions[i]))
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        public static int GetNearestIndex(double resolution, double[] resolutions)
        {
            if (resolutions == null || resolutions.Length < 1)
            {
                return -1;
            }
            int index = 0;
            for (int i = 1; i < resolutions.Length; i++)
            {
                if (DoubleUtil.AreClose(resolution, resolutions[i]))
                {
                    index = i;
                    break;
                }
                else if (Math.Abs(resolutions[i] - resolution) < Math.Abs(resolutions[index] - resolution))
                {
                    index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// 判断值是否超出最大最小值得范围。如果超出，返回最大值/最小值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minRes"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double MinMaxCheck(double value, double minValue, double maxValue)
        {
            if (!double.IsNaN(minValue) && value < minValue)
            {
                return minValue;
            }
            if (!double.IsNaN(maxValue) && value > maxValue)
            {
                return maxValue;
            }
            return value;
        }

        //没有用到
        //public static double GetNearest(double resolution, double[] resolutions)
        //{
        //    if (resolutions == null || resolutions.Length < 1)
        //    {
        //        return resolution;
        //    }
        //    int index = GetNearestIndex(resolution, resolutions);
        //    return resolutions[index];
        //}

        //public static Point TransformPoint(Point oldPoint, Point transformOrigin, double radian)
        //{
        //    radian = -radian;//rotation顺时针为正，改计算角度是以逆时针为正
        //    Point newPoint = new Point(double.NaN, double.NaN);
        //    newPoint.X = (oldPoint.X - transformOrigin.X) * Math.Cos(radian) - (oldPoint.Y - transformOrigin.Y) * Math.Sin(radian) + transformOrigin.X;
        //    newPoint.Y = (oldPoint.X - transformOrigin.X) * Math.Sin(radian) + (oldPoint.Y - transformOrigin.Y) * Math.Cos(radian) + transformOrigin.Y;
        //    return newPoint;
        //}

        public static bool CheckNewAndOldEqual(double[] newValues, double[] oldValues)
        {
            if (newValues != null && oldValues != null)
            {
                if (newValues.Length != oldValues.Length)
                {
                    return false;
                }
                for (int i = 0; i < newValues.Length; i++)
                {
                    if (newValues[i] != oldValues[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (newValues == null && oldValues == null)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
