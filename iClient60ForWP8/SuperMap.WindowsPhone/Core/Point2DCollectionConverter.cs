using System;
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_Point2DCollectionConverter_Title}</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Point2DCollectionConverter : TypeConverter
    {
        /// <summary>${WP_core_Point2DCollectionConverter_method_CanConvertFrom_D}</summary>
        /// <param name="context">${WP_core_Point2DCollectionConverter_method_CanConvertFrom_param_context}</param>
        /// <param name="sourceType">${WP_core_Point2DCollectionConverter_method_CanConvertFrom_param_sourceType}</param>
        /// <returns>${WP_core_Point2DCollectionConverter_method_CanConvertFrom_return}</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }
        /// <summary>${WP_core_Point2DCollectionConverter_method_ConvertFrom_D}</summary>
        /// <param name="value">${WP_core_Point2DCollectionConverter_method_ConvertFrom_param_value}</param>
        /// <param name="context">${WP_core_Point2DCollectionConverter_method_ConvertFrom_param_context}</param>
        /// <param name="culture">${WP_core_Point2DCollectionConverter_method_ConvertFrom_param_culture}</param>
        /// <returns>${WP_core_Point2DCollectionConverter_method_ConvertFrom_return}</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str == null)
            {
                throw new NotSupportedException();
            }
            Point2DCollection points = new Point2DCollection();
            Point2DConverter converter = new Point2DConverter();
            int num = -1;
            for (int i = 0; i < (str.Length + 1); i++)
            {
                if ((i >= str.Length) || char.IsWhiteSpace(str[i]))
                {
                    int startIndex = num + 1;
                    int length = i - startIndex;
                    if (length >= 1)
                    {
                        string str2 = str.Substring(startIndex, length);
                        points.Add((Point2D)converter.ConvertFrom(str2));
                    }
                    num = i;
                }
            }
            return points;
        }
    }
}
