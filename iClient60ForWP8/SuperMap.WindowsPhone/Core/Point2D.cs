using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_WP_core_Point2D_Title}</para>
    /// 	<para>${WP_WP_core_Point2D_Description}</para>
    /// </summary>
    public class Point2D : IFormattable, INotifyPropertyChanged
    {
        private double x;
        private double y;

        /// <summary>
        /// ${WP_core_Point2D_constructor_Double_D}
        /// </summary>
        public Point2D()
        {

        }

        /// <summary>${WP_core_Point2D_constructor_Double_D}</summary>
        /// <param name="x">${WP_core_Point2D_constructor_Double_param_x}</param>
        /// <param name="y">${WP_core_Point2D_constructor_Double_param_y}</param>
        public Point2D(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>${WP_core_Point2D_attribute_x_D}</summary>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
                this.raisePropertyChanged("Point2D");
            }
        }
        /// <summary>${WP_core_Point2D_attribute_y_D}</summary>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
                this.raisePropertyChanged("Point2D");
            }
        }
        /// <summary>${WP_core_Point2D_attribute_Tag_D}</summary>
        public object Tag
        {
            get;
            set;
        }


        /// <summary>${WP_core_Point2D_attribute_empty_D}</summary>
        public static Point2D Empty
        {
            get { return new Point2D{x = double.PositiveInfinity, y = double.PositiveInfinity}; }
        }

        /// <summary>
        /// ${WP_core_Point2D_method_IsNullOrEmpty_D}
        /// </summary>
        /// <param name="point">${WP_core_Point2D_method_IsNullOrEmpty_param_point}</param>
        /// <returns>${WP_core_Point2D_method_IsNullOrEmpty_return}</returns>
        public static bool IsNullOrEmpty(Point2D point)
        {
            if (point == null)
            {
                return true;
            }

            if (point.X.ValueCheck() && point.Y.ValueCheck())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>${WP_core_Point2D_operators_DoubleEquals_D}</summary>
        /// <returns>${WP_core_Point2D_operators_DoubleEquals_return}</returns>
        /// <param name="point1">${WP_core_Point2D_operators_DoubleEquals_param_point}</param>
        /// <param name="point2">${WP_core_Point2D_operators_DoubleEquals_param_point}</param>
        public static bool operator ==(Point2D point1, Point2D point2)
        {
            if (object.Equals(point1, null) || object.Equals(point2, null))
            {
                return object.Equals(point1, point2);
            }
            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }
        /// <summary>${WP_core_Point2D_operators_NotEqual_D}</summary>
        /// <returns>${WP_core_Point2D_operators_NotEqual_return}</returns>
        /// <param name="point1">${WP_core_Point2D_operators_DoubleEquals_param_point}</param>
        /// <param name="point2">${WP_core_Point2D_operators_DoubleEquals_param_point}</param>
        public static bool operator !=(Point2D point1, Point2D point2)
        {
            return !(point1 == point2);
        }

        /// <overloads>${WP_core_Point2D_method_equals_overloads}</overloads>
        /// <summary>${WP_core_Point2D_method_equals_Object_D}</summary>
        /// <returns>${WP_core_Point2D_method_equals_Object_return}</returns>
        /// <param name="obj">${WP_core_Point2D_method_equals_Object_param_object}</param>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Point2D))
            {
                return false;
            }
            Point2D p = (Point2D)obj;
            return (this == p);
        }
        /// <summary>${WP_core_Point2D_method_GetHashCode_D}</summary>
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }

        /// <returns>${WP_core_Point2D_method_equals_Point2D_return}</returns>
        /// <summary>${WP_core_Point2D_method_equals_Point2D_D}<br/></summary>
        /// <overloads>${WP_core_Point2D_method_equals_overloads}</overloads>
        /// <param name="point">${WP_core_Point2D_method_equals_Point2D_param_point}<br/></param>
        public bool Equals(Point2D point)
        {
            return (this == point);
        }

        /// <summary> ${WP_core_Point2D_method_toString_D}</summary>
        /// <returns>${WP_core_Point2D_method_toString_return}</returns>
        /// <overloads>${WP_core_Point2D_method_toString_overloads}</overloads>
        public override string ToString()
        {
            return ((IFormattable)this).ToString(null, null);
        }
        /// <summary>${WP_core_GeoRegion_method_Offset_D}</summary>
        /// <param name="deltaX">${WP_core_Point2D_method_offset_param_dx}</param>
        /// <param name="deltaY">${WP_core_Point2D_method_offset_param_dy}</param>
        public Point2D Offset(double deltaX, double deltaY)
        {
            return new Point2D(this.X + deltaX, this.Y + deltaY);
        }

        /// <summary>${WP_core_Point2D_method_toString_IFormatProvider_D}</summary>
        /// <returns>${WP_core_Point2D_method_toString_return}</returns>
        /// <overloads>${WP_core_Point2D_method_toString_overloads}</overloads>
        /// <param name="provider">${WP_core_Point2D_method_toString_IFormatProvider_param_provider}</param>
        public string ToString(IFormatProvider provider)
        {
            return ((IFormattable)this).ToString(null, provider);
        }
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            if (Point2D.IsNullOrEmpty(this))
            {
                return "Empty";
            }
            return string.Format(provider, "{0:" + format + "},{1:" + format + "}", new object[] { this.x, this.y });
        }
        /// <summary>${WP_core_Geometry_method_clone_D}</summary>
        public Point2D Clone()
        {
            return new Point2D(x, y);
        }
        /// <summary>${WP_mapping_Layer_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void raisePropertyChanged(string propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
