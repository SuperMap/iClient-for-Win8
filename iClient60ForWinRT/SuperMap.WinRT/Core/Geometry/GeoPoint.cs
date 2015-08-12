using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_GeoPoint_Title}</para>
    /// 	<para>${core_GeoPoint_Description}</para>
    /// </summary>
    [DataContract(Name = "point")]
    public class GeoPoint : Geometry//, IEquatable<GeoPoint>
    {
        private double x;
        private double y;
        /// <summary>${core_GeoPoint_constructor_None_D}</summary>
        /// <overloads>${core_GeoPoint_constructor_overloads}</overloads>
        public GeoPoint()
            : this(double.NaN, double.NaN)
        {
        }
        /// <summary>${core_GeoPoint_constructor_Point2D_D}</summary>
        /// <param name="location">${core_GeoPoint_constructor_Point2D_param_point}</param>
        public GeoPoint(Point2D location)
            : this(location.X, location.Y)
        {
        }
        /// <summary>${core_GeoPoint_method_clone_D}</summary>
        public override Geometry Clone()
        {
            return base.MemberwiseClone() as GeoPoint;
        }
        /// <summary>${core_GeoPoint_method_Offset_D}</summary>
        /// <param name="dx">${core_GeoPoint_method_Offset_param_deltaX}</param>
        /// /// <param name="dy">${core_GeoPoint_method_Offset_param_deltaY}</param>
        public override void Offset(double dx, double dy)
        {
            this.X += dx;
            this.Y += dy;
        }
        /// <summary>${core_GeoPoint_constructor_Double_D}</summary>
        /// <overloads>${core_GeoPoint_constructor_overloads}</overloads>
        /// <param name="x">${core_GeoPoint_constructor_Double_param_x}</param>
        /// <param name="y">${core_GeoPoint_constructor_Double_param_y}</param>
        public GeoPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>${core_GeoPoint_method_toString_D}</summary>
        /// <returns>${core_GeoPoint_method_toString_return}</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[] { this.X, this.Y });
        }

        /// <summary>${core_GeoPoint_attribute_bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                if (double.IsNaN(this.X) || double.IsNaN(this.Y))
                {
                    return Rectangle2D.Empty;
                }

                return new Rectangle2D(this.X, this.Y, this.X, this.Y);
            }
        }
        /// <summary>${core_GeoPoint_attribute_Location_D}</summary>
        public Point2D Location
        {
            get
            {
                if (double.IsNaN(this.X) || double.IsNaN(this.Y))
                {
                    return Point2D.Empty;
                }
                return new Point2D(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
        /// <summary>${core_GeoPoint_attribute_x_D}</summary>
        [DataMember(Name = "x")]
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                if (this.x != value)
                {
                    this.x = value;
                    base.RaiseGeometryChanged();
                }
            }
        }
        /// <summary>${core_GeoPoint_attribute_y_D}</summary>
        [DataMember(Name = "y")]
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y != value)
                {
                    this.y = value;
                    base.RaiseGeometryChanged();
                }
            }
        }
    }
}
