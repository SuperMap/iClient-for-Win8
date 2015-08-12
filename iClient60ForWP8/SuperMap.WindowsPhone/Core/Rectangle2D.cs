using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using SuperMap.WindowsPhone.Resources;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>  
    /// 	<para>${WP_core_Rectangle2D_Title}</para>
    /// 	<para>${WP_core_Rectangle2D_Description}</para>
    /// </summary>
    public class Rectangle2D : IFormattable
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;

        /// <summary>
        /// ${WP_core_Rectangle2D_constructor_None_D}
        /// </summary>
        public Rectangle2D()
        {

        }

        /// <summary>${WP_core_Rectangle2D_constructor_Double_Double_D}</summary>
        /// <param name="left">${WP_core_Rectangle2D_constructor_Double_Double_param_left}</param>
        /// <param name="bottom">${WP_core_Rectangle2D_constructor_Double_Double_param_bottom}</param>
        /// <param name="right">${WP_core_Rectangle2D_constructor_Double_Double_param_right}</param>
        /// <param name="top">${WP_core_Rectangle2D_constructor_Double_Double_param_top}</param>
        public Rectangle2D(double left, double bottom, double right, double top)
            : this()
        {
            FromXYWidthHeight(left, bottom, right - left, top - bottom);
        }

        /// <summary>${WP_core_Rectangle2D_constructor_Point2D_Point2D_D}</summary>
        /// <param name="point1">${WP_core_Rectangle2D_constructor_Point2D_Point2D_param_bottomLeft}</param>
        /// <param name="point2">${WP_core_Rectangle2D_constructor_Point2D_Point2D_param_topRight}</param>
        public Rectangle2D(Point2D point1, Point2D point2)
        {
            this._x = Math.Min(point1.X, point2.X);
            this._y = Math.Min(point1.Y, point2.Y);
            this._width = Math.Max((double)(Math.Max(point1.X, point2.X) - this._x), (double)0.0);
            this._height = Math.Max((double)(Math.Max(point1.Y, point2.Y) - this._y), (double)0.0);
        }

        /// <summary>${WP_core_Rectangle2D_constructor_Point2D_Double_D}</summary>
        /// <param name="bottomLeft">${WP_core_Rectangle2D_constructor_Point2D_Double_param_bottomLeft}</param>
        /// <param name="width">${WP_core_Rectangle2D_constructor_Point2D_Double_param_width}</param>
        /// <param name="height">${WP_core_Rectangle2D_constructor_Point2D_Double_param_height}</param>
        public Rectangle2D(Point2D bottomLeft, double width, double height)
            : this(bottomLeft.X, bottomLeft.Y, bottomLeft.X + width, bottomLeft.Y + height)
        {
        }

        private void FromXYWidthHeight(double x, double y, double width, double height)
        {
            if (width < 0.0)
            {
                throw new ArgumentException(ExceptionStrings.WidthLessThanZero);
            }
            if (height < 0.0)
            {
                throw new ArgumentException(ExceptionStrings.HeightLessThanZero);
            }
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
        }

        /// <summary>${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_D}</summary>
        /// <param name="x">${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_param_x}</param>
        /// <param name="y">${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_param_y}</param>
        /// <param name="width">${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_param_width}</param>
        /// <param name="height">${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_param_height}</param>
        /// <returns>${WP_core_Rectangle2D_method_CreateFromXYWidthHeight_return}</returns>
        public static Rectangle2D CreateFromXYWidthHeight(double x, double y, double width, double height)
        {
            if (!x.ValueCheck() || !y.ValueCheck() || !width.ValueCheck() || !height.ValueCheck())
            {
                return Rectangle2D.Empty;
            }

            return new Rectangle2D(x, y, x + width, y + height);
        }

        /// <summary>${WP_core_Rectangle2D_attribute_empty_D_sl}</summary>
        public static Rectangle2D Empty
        {
            get
            {
                return new Rectangle2D
                {
                    _x = double.PositiveInfinity,
                    _y = double.PositiveInfinity,
                    _width = double.NegativeInfinity,
                    _height = double.NegativeInfinity
                };
            }
        }

        /// <summary>
        /// ${WP_core_Rectangle2D_method_IsNullOrEmpty_D}
        /// </summary>
        /// <param name="rect">${WP_core_Rectangle2D_method_IsNullOrEmpty_param_rect}</param>
        /// <returns>${WP_core_Rectangle2D_method_IsNullOrEmpty_return}</returns>
        public static bool IsNullOrEmpty(Rectangle2D rect)
        {
            if (rect == null)
            {
                return true;
            }
            if (rect._x.ValueCheck() && rect._y.ValueCheck() && rect._width.ValueCheck() && rect._height.ValueCheck())
            {
                return false;
            }
            if (rect._width < 0.0 || rect._height < 0.0)
            {
                return true;
            }
            return true;
        }

        /// <summary>${WP_core_Rectangle2D_attribute_left_D}</summary>
        public double Left
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return this._x;
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_right_D}</summary>
        public double Right
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return (this._x + this._width);
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_bottom_D}</summary>
        public double Bottom
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return this._y;
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_top_D}</summary>
        public double Top
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return (this._y + this._height);
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_center_D}</summary>
        public Point2D Center
        {
            get
            {
                return new Point2D((this.Left + this.Right) / 2, (this.Bottom + this.Top) / 2);
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_height_D}</summary>
        public double Height
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return this._height;
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_width_D}</summary>
        public double Width
        {
            get
            {
                if (Rectangle2D.IsNullOrEmpty(this))
                {
                    return double.NegativeInfinity;
                }
                return this._width;
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_topRight_D}</summary>
        public Point2D TopRight
        {
            get
            {
                return new Point2D(this.Right, this.Top);
            }
        }

        /// <summary>${WP_core_Rectangle2D_attribute_bottomLeft_D}</summary>
        public Point2D BottomLeft
        {
            get
            {
                return new Point2D(this.Left, this.Bottom);
            }
        }
        /// <summary>${WP_core_Rectangle2D_constructor_Point2D_Point2D_param_TopLeft}</summary>
        public Point2D TopLeft
        {
            get
            {
                return new Point2D(this.Left, this.Top);
            }
        }
        /// <summary>${WP_core_Rectangle2D_constructor_Point2D_Point2D_param_BottomRight}</summary>
        public Point2D BottomRight
        {
            get
            {
                return new Point2D(this.Right, this.Bottom);
            }
        }

        /// <summary>${WP_core_Rectangle2D_operators_DoubleEquals_D}</summary>
        /// <returns>${WP_core_Rectangle2D_operators_DoubleEquals_return}</returns>
        /// <param name="rect1">${WP_core_Rectangle2D_operators_DoubleEquals_param_rect}</param>
        /// <param name="rect2">${WP_core_Rectangle2D_operators_DoubleEquals_param_rect}</param>
        public static bool operator ==(Rectangle2D rect1, Rectangle2D rect2)
        {
            if (object.Equals(rect1, null) || object.Equals(rect2, null))
            {
                return object.Equals(rect1, rect2);
            }
            return (rect1._x == rect2._x) && (rect1._y == rect2._y) && (rect1._width == rect2._width) && (rect1._height == rect2._height);
        }
        /// <summary>${WP_core_Rectangle2D_operators_NotEqual_D}</summary>
        /// <returns>${WP_core_Rectangle2D_operators_NotEqual_return}</returns>
        /// <param name="rect1">${WP_core_Rectangle2D_operators_DoubleEquals_param_rect}</param>
        /// <param name="rect2">${WP_core_Rectangle2D_operators_DoubleEquals_param_rect}</param>
        public static bool operator !=(Rectangle2D rect1, Rectangle2D rect2)
        {
            return !(rect1 == rect2);
        }

        /// <summary>${WP_core_Rectangle2D_method_equalsObject_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_equalsObject_return}</returns>
        /// <overloads>${WP_core_Rectangle2D_method_equals_overloads}</overloads>
        /// <param name="obj">${WP_core_Rectangle2D_method_equalsObject_param_rect}</param>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Rectangle2D))
            {
                return false;
            }
            Rectangle2D rectangle = (Rectangle2D)obj;
            return (this == rectangle);
        }
        /// <summary>${WP_core_Rectangle2D_method_GetHashCode_D}</summary>
        public override int GetHashCode()
        {
            return ((this.Left.GetHashCode() ^ this.Bottom.GetHashCode()) ^ this.Right.GetHashCode() ^ this.Top.GetHashCode());
        }

        /// <summary>${WP_core_Rectangle2D_method_toString_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_toString_return}</returns>
        /// <overloads>${WP_core_Rectangle2D_method_toString_overloads}</overloads>
        public override string ToString()
        {
            return ((IFormattable)this).ToString(null, null);
        }

        /// <summary>${WP_core_Rectangle2D_method_equals_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_equals_return}</returns>
        /// <overloads>${WP_core_Rectangle2D_method_equals_overloads}</overloads>
        /// <param name="rectangle">${WP_core_Rectangle2D_method_equals_param_rectangle}</param>
        public bool Equals(Rectangle2D rectangle)
        {
            return (this == rectangle);
        }

        /// <summary>${WP_core_Rectangle2D_method_offset_D}</summary>
        /// <param name="dx">${WP_core_Rectangle2D_method_offset_param_dx}</param>
        /// <param name="dy">${WP_core_Rectangle2D_method_offset_param_dy}</param>
        /// <returns>${WP_core_Rectangle2D_method_offset_return}</returns>
        public Rectangle2D Offset(double dx, double dy)
        {
            Rectangle2D clone = this.Clone();
            clone._x += dx;
            clone._y += dy;
            return clone;
        }

        /// <summary>${WP_core_Rectangle2D_method_inflate_D_sl}</summary>
        /// <param name="width">${WP_core_Rectangle2D_method_inflate_param_width_sl}</param>
        /// <param name="height">${WP_core_Rectangle2D_method_inflate_param_height_sl}</param>
        /// <returns>${WP_core_Rectangle2D_method_inflate_return}</returns>
        public Rectangle2D Inflate(double width, double height)
        {
            if (width < -this.Width * 0.5)
            {
                return Rectangle2D.Empty;
            }
            if (height < -this.Height * 0.5)
            {
                return Rectangle2D.Empty;
            }
            Rectangle2D clone = this.Clone();
            clone._x -= width;
            clone._y -= height;
            clone._width += width * 2;
            clone._height += height * 2;
            return clone;
        }

        /// <summary>${WP_core_Rectangle2D_method_intersectsWith_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_intersectsWith_return}</returns>
        /// <param name="rect">${WP_core_Rectangle2D_method_intersectsWith_param_rect}</param>
        public bool IntersectsWith(Rectangle2D rect)
        {
            if ((this._width < 0.0) || (rect.Width < 0.0))
            {
                return false;
            }

            return (rect.Left <= this.Right) && (rect.Right >= this.Left) && (rect.Bottom <= this.Top) && (rect.Top >= this.Bottom);
        }

        /// <summary>${WP_core_Rectangle2D_method_intersect_D}</summary>
        /// <param name="rect">${WP_core_Rectangle2D_method_intersect_param_rect}</param>
        /// <returns>${WP_core_Rectangle2D_method_intersect_return}</returns>
        public Rectangle2D Intersect(Rectangle2D rect)
        {
            if (!this.IntersectsWith(rect))
            {
                return Rectangle2D.Empty;
            }
            else
            {
                Rectangle2D clone = this.Clone();
                double maxLeft = Math.Max(this.Left, rect.Left);
                double maxBottom = Math.Max(this.Bottom, rect.Bottom);
                clone._width = Math.Max((Math.Min(this.Right, rect.Right) - maxLeft), (double)0.0);
                clone._height = Math.Max((Math.Min(this.Top, rect.Top) - maxBottom), (double)0.0);
                clone._x = maxLeft;
                clone._y = maxBottom;
                return clone;
            }
        }

        /// <summary>${WP_core_Rectangle2D_method_union_Point2D_D}</summary>
        /// <overloads>${WP_core_Rectangle2D_method_union_overloads}</overloads>
        /// <param name="point">${WP_core_Rectangle2D_method_union_Point2D_param_point}</param>
        /// <returns>${WP_core_Rectangle2D_method_union_return}</returns>
        public Rectangle2D Union(Point2D point)
        {
            return this.Union(new Rectangle2D(point, point));
        }

        /// <summary>${WP_core_Rectangle2D_method_union_Rectangle2D_D}</summary>
        /// <param name="rect">${WP_core_Rectangle2D_method_union_Rectangle2D_param_rect}</param>
        /// <returns>${WP_core_Rectangle2D_method_union_return}</returns>
        public Rectangle2D Union(Rectangle2D rect)
        {
            if (Rectangle2D.IsNullOrEmpty(rect))
            {
                return this.Clone();
            }
            if (Rectangle2D.IsNullOrEmpty(this))
            {
                return rect;
            }
            else
            {
                double minLeft = Math.Min(this.Left, rect.Left);
                double minBottom = Math.Min(this.Bottom, rect.Bottom);
                double maxRight = Math.Max(this.Right, rect.Right);
                double maxTop = Math.Max(this.Top, rect.Top);
                Rectangle2D clone = this.Clone();
                clone._width = Math.Max((double)(maxRight - minLeft), (double)0.0);
                clone._height = Math.Max((double)(maxTop - minBottom), (double)0.0);
                clone._x = minLeft;
                clone._y = minBottom;

                return clone;
            }
        }

        /// <summary>${WP_core_Rectangle2D_method_containsXY_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_containsXY_return}</returns>
        /// <overloads>${WP_core_Rectangle2D_method_contains_overloads}</overloads>
        /// <param name="x">${WP_core_Rectangle2D_method_containsXY_param_x}</param>
        /// <param name="y">${WP_core_Rectangle2D_method_containsXY_param_y}</param>
        public bool Contains(double x, double y)
        {
            return (this.Left <= x) && (x <= this.Right) && (this.Bottom <= y) && (y <= this.Top);
        }

        internal bool Within(Rectangle2D other)
        {
            return ((((this.Left >= other.Left) && (this.Right <= other.Right)) && (this.Bottom >= other.Bottom)) && (this.Top <= other.Top));
        }


        /// <summary>${WP_core_Rectangle2D_method_containsPoint2D_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_containsPoint2D_return_sl}</returns>
        /// <param name="point">${WP_core_Rectangle2D_method_containsPoint2D_param_point}</param>
        public bool Contains(Point2D point)
        {
            return this.Contains(point.X, point.Y);
        }

        /// <summary>${WP_core_Rectangle2D_method_containsRectangle2D_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_containsRectangle2D_return_sl}</returns>
        /// <param name="rect">${WP_core_Rectangle2D_method_containsRectangle2D_param_rect}</param>
        public bool Contains(Rectangle2D rect)
        {
            if (Rectangle2D.IsNullOrEmpty(rect))
            {
                return false;
            }
            return (this.Left <= rect.Left) && (rect.Right <= this.Right) && (this.Bottom <= rect.Bottom) && (rect.Top <= this.Top);
        }

        /// <summary>${WP_core_Rectangle2D_method_Expand_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_expand_return}</returns>
        public Rectangle2D Expand(double expandFactor)
        {
            if (expandFactor > 0)
            {
                Rectangle2D clone = this.Clone();
                clone._x += (1 - expandFactor) * clone._width * 0.5;
                clone._y += (1 - expandFactor) * clone._height * 0.5;
                clone._height *= expandFactor;
                clone._width *= expandFactor;
                return clone;
            }
            return Rectangle2D.Empty;
        }

        /// <summary>
        /// ${WP_core_Rectangle2D_method_clone_D}
        /// </summary>
        /// <returns>${WP_core_Rectangle2D_method_clone_return}</returns>
        public Rectangle2D Clone()
        {
            return new Rectangle2D(_x, _y, _x + _width, _y + _height);
        }

        /// <summary>${WP_core_Rectangle2D_method_toString_IFormatProvider_D}</summary>
        /// <returns>${WP_core_Rectangle2D_method_toString_return}</returns>
        /// <param name="provider">${WP_core_Rectangle2D_method_toString_IFormatProvider_param_provider}</param>
        public string ToString(IFormatProvider provider)
        {
            return ((IFormattable)this).ToString(null, provider);
        }
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            if (Rectangle2D.IsNullOrEmpty(this))
            {
                return "Empty";
            }
            return string.Format(provider, "{0:" + format + "},{1:" + format + "},{2:" + format + "},{3:" + format + "}", new object[] { this.Left, this.Bottom, this.Right, this.Top });
        }
    }
}
