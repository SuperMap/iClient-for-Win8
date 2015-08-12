using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_Point2D_Title}</para>
    /// 	<para>${core_Point2D_Description}</para>
    /// </summary>
    /// <example>
    /// <code>
    /// Point2D p=new Point2D(20,30);
    /// </code>
    /// </example>
    [DataContract, StructLayout(LayoutKind.Sequential)]
    public class Point2D : IFormattable, INotifyPropertyChanged
    {
        private double x;
        private double y;

        /// <summary>
        /// ${core_Point2D_constructor}
        /// </summary>
        public Point2D()
        {

        }

        /// <summary>${core_Point2D_constructor_Double_D}</summary>
        /// <param name="x">${core_Point2D_constructor_Double_param_x}</param>
        /// <param name="y">${core_Point2D_constructor_Double_param_y}</param>
        public Point2D(double x, double y)
            : this()
        {
            if (DoubleUtil.ValueCheck(x))
            {
                this.x = x;
            }
            else
            {
                this.x = double.NegativeInfinity;
            }
            if (DoubleUtil.ValueCheck(y))
            {
                this.y = y;
            }
            else
            {
                this.y = double.NegativeInfinity;
            }
        }

        /// <summary>${core_Point2D_attribute_x_D}</summary>
        [DataMember(Name = "x")]
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                if (DoubleUtil.ValueCheck(value))
                {
                    x = value;
                }
                else
                {
                    x = double.NegativeInfinity;
                }
                this.raisePropertyChanged("Point2D");
            }
        }
        /// <summary>${core_Point2D_attribute_y_D}</summary>
        [DataMember(Name = "y")]
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (DoubleUtil.ValueCheck(value))
                {
                    y = value;
                }
                else
                {
                    y = double.NegativeInfinity;
                }
                this.raisePropertyChanged("Point2D");
            }
        }

        /// <summary>
        /// ${core_Point2D_method_IsNullOrEmpty_D}
        /// </summary>
        /// <param name="point">${core_Point2D_method_IsNullOrEmpty_param_point}</param>
        /// <returns>${core_Point2D_method_IsNullOrEmpty_return}</returns>
        public static bool IsNullOrEmpty(Point2D point)
        {
            if (point == null)
            {
                return true;
            }
            if (point.X == double.NegativeInfinity || point.Y == double.NegativeInfinity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>${core_Point2D_attribute_empty_D}</summary>
        public static Point2D Empty
        {
            get { return new Point2D(double.NegativeInfinity, double.NegativeInfinity); }
        }

        /// <summary>${core_Point2D_operators_DoubleEquals_D}</summary>
        /// <returns>${core_Point2D_operators_DoubleEquals_return}</returns>
        /// <param name="point1">${core_Point2D_operators_DoubleEquals_param_point}</param>
        /// <param name="point2">${core_Point2D_operators_DoubleEquals_param_point}</param>
        public static bool operator ==(Point2D point1, Point2D point2)
        {
            if (object.Equals(point1, null) || object.Equals(point2, null))
            {
                return object.Equals(point1, point2);
            }

            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }
        /// <summary>${core_Point2D_operators_NotEqual_D}</summary>
        /// <returns>${core_Point2D_operators_NotEqual_return}</returns>
        /// <param name="point1">${core_Point2D_operators_DoubleEquals_param_point}</param>
        /// <param name="point2">${core_Point2D_operators_DoubleEquals_param_point}</param>
        public static bool operator !=(Point2D point1, Point2D point2)
        {
            return !(point1 == point2);
        }

        /// <overloads>${core_Point2D_method_equals_overloads}</overloads>
        /// <summary>${core_Point2D_method_equals_Object_D}</summary>
        /// <returns>${core_Point2D_method_equals_Object_return}</returns>
        /// <param name="obj">${core_Point2D_method_equals_Object_param_object}</param>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Point2D))
            {
                return false;
            }
            Point2D p = (Point2D)obj;
            return (this == p);
        }
        /// <summary>${core_Point2D_method_GetHashCode_D}</summary>
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }

        /// <returns>${core_Point2D_method_equals_Point2D_return}</returns>
        /// <summary>${core_Point2D_method_equals_Point2D_D}<br/></summary>
        /// <overloads>${core_Point2D_method_equals_overloads}</overloads>
        /// <param name="point">${core_Point2D_method_equals_Point2D_param_point}<br/></param>
        public bool Equals(Point2D point)
        {
            return (this == point);
        }

        /// <summary> ${core_Point2D_method_toString_D}</summary>
        /// <returns>${core_Point2D_method_toString_return}</returns>
        /// <overloads>${core_Point2D_method_toString_overloads}</overloads>
        public override string ToString()
        {
            return ((IFormattable)this).ToString(null, null);
        }
        /// <summary>${core_Point2D_method_offset_D}</summary>
        /// <param name="deltaX">${core_Point2D_method_offset_param_dx}</param>
        /// <param name="deltaY">${core_Point2D_method_offset_param_dy}</param>
        public Point2D Offset(double deltaX, double deltaY)
        {
            return new Point2D(this.X + deltaX, this.Y + deltaY);
        }

        /// <summary>${core_Point2D_method_toString_IFormatProvider_D}</summary>
        /// <returns>${core_Point2D_method_toString_return}</returns>
        /// <overloads>${core_Point2D_method_toString_overloads}</overloads>
        /// <param name="provider">${core_Point2D_method_toString_IFormatProvider_param_provider}</param>
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
        /// <summary>${core_Geometry_method_clone_D}</summary>
        public Point2D Clone()
        {
            return new Point2D(X, Y);
        }
        /// <summary>${mapping_Layer_event_PropertyChanged_D}</summary>
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
