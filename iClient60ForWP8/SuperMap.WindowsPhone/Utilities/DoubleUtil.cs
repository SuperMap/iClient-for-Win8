using System;

namespace SuperMap.WindowsPhone.Utilities
{
    //来源：System.Windows.Controls.Data程序集，System.Windows.Controls命名空间
    internal static class DoubleUtil
    {
        // Fields
        //0.000,019,964,719,130,085,3
        //1E-6精度不够，1E-10又太大了，那现在就用这个吧。
        internal const double DBL_EPSILON = 1E-10;

        public static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            double num = ((Math.Abs(value1) + Math.Abs(value2)) + 10.0) * DBL_EPSILON;
            double num2 = value1 - value2;
            return ((-num < num2) && (num > num2));
        }

        /// <summary>
        /// 校验double值的有效性，如果返回false，这个值不可用，不能参与计算。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValueCheck(this double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value.CompareTo(double.Epsilon) == 0 ||
                value.CompareTo(double.MaxValue) == 0 || value.CompareTo(double.MinValue) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static bool GreaterThan(double value1, double value2)
        //{
        //    return ((value1 > value2) && !AreClose(value1, value2));
        //}

        //public static bool GreaterThanOrClose(double value1, double value2)
        //{
        //    if (value1 <= value2)
        //    {
        //        return AreClose(value1, value2);
        //    }
        //    return true;
        //}

        //public static bool IsZero(double value)
        //{
        //    return (Math.Abs(value) < 9.9999999999999991E-06);//9.99...何必
        //}

        //public static bool LessThan(double value1, double value2)
        //{
        //    return ((value1 < value2) && !AreClose(value1, value2));
        //}

        //public static bool LessThanOrClose(double value1, double value2)
        //{
        //    if (value1 >= value2)
        //    {
        //        return AreClose(value1, value2);
        //    }
        //    return true;
        //}
    }
}
