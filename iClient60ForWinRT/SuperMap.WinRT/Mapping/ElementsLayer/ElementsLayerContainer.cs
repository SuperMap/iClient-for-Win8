using SuperMap.WinRT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Mapping
{
    internal class ElementsLayerContainer:LayerContainer
    {
        private List<Rectangle2D> orignalBounds;

        internal ElementsLayerContainer(Layer layer)
            : base(layer)
        {
            this.orignalBounds = new List<Rectangle2D>();
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            ElementsLayer layer = Layer as ElementsLayer;
            Point2D origin = this.Origin;
            double resolution = this.Resolution;
            Rectangle2D clipBox = new Rectangle2D(
                origin.X - (resolution * 4191.0),
                origin.Y - (resolution * 4191.0),
                origin.X + (resolution * 4191.0),
                origin.Y + (resolution * 4191.0));
            layer.BoundsCollection.Clear();
            orignalBounds.Clear();
            if (layer.IsAutoAvoidance)
            {
                foreach (UIElement element in base.Children)
                {
                    Rectangle2D b = GetBounds(element);
                    double referenceLength = (Layer as ElementsLayer).GetReferenceLength(element) * resolution;
                    if (b.BottomLeft == b.TopRight)
                    {
                        b = new Rectangle2D(b.Left - (referenceLength) / 2, b.Bottom - (element.DesiredSize.Height * resolution) / 2,
                                            b.Left + (referenceLength) / 2, b.Bottom + (element.DesiredSize.Height * resolution) / 2);
                    }
                    orignalBounds.Add(b);
                }
            }
            foreach (UIElement element in base.Children)
            {
                if (element.Visibility != Visibility.Visible)
                {
                    continue;
                }
                if (element is ShapeElement)
                {
                    this.ArrangeShapeBase(element as ShapeElement, clipBox, origin, resolution);
                    continue;
                }
                Rectangle2D b = GetBounds(element);
                if (layer.IsAutoAvoidance)
                {
                    if (b.BottomLeft == b.TopRight)
                    {
                        b = new Rectangle2D(b.Left - (element.DesiredSize.Width * resolution) / 2, b.Bottom - (element.DesiredSize.Height * resolution) / 2,
                                            b.Left + (element.DesiredSize.Width * resolution) / 2, b.Bottom + (element.DesiredSize.Height * resolution) / 2);
                    }
                    bool isIntersect;
                    bool isGetNewBounds;
                    double referenceLength = (Layer as ElementsLayer).GetReferenceLength(element) * resolution;
                    Size referenceSize = (Layer as ElementsLayer).GetReferenceSize(element);

                    b = isBoundsChange(b, referenceLength, referenceSize, out isIntersect, out isGetNewBounds);

                    if (isIntersect)
                    {
                        if (isGetNewBounds)
                        {
                            (element as UIElement).Opacity = 1;
                        }
                        else
                        {
                            (element as UIElement).Opacity = 0;
                        }
                    }
                    else
                    {
                        (element as UIElement).Opacity = 1;
                    }

                    (Layer as ElementsLayer).BoundsCollection.Add(element, b);
                }
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double pixelX = (b.Left - origin.X) / resolution;
                    double pixelY = (origin.Y - b.Top) / resolution;
                    double pixelWidth = b.Width / resolution;
                    double pixelHeight = b.Height / resolution;

                    if (((!double.IsInfinity(pixelWidth) && !double.IsInfinity(pixelHeight)) && (!double.IsInfinity(pixelX) && !double.IsInfinity(pixelY))) && ((!double.IsNaN(pixelWidth) && !double.IsNaN(pixelHeight)) && (!double.IsNaN(pixelX) && !double.IsNaN(pixelY))))
                    {
                        double width;
                        double height;
                        if (((element is FrameworkElement) && (pixelWidth != 0.0)) && (pixelHeight != 0.0))
                        {
                            (element as FrameworkElement).Width = pixelWidth;
                            (element as FrameworkElement).Height = pixelHeight;
                        }
                        if ((pixelWidth != 0.0) || (pixelHeight != 0.0))
                        {
                            element.Arrange(new Rect(pixelX, pixelY, pixelWidth, pixelHeight));
                            continue;
                        }
                        //相当于左上，右下，中心等位置的设定。
                        //设在某个点，bbox的宽和高都是0，比如pushpin\textbox等
                        switch (((HorizontalAlignment)element.GetValue(FrameworkElement.HorizontalAlignmentProperty)))
                        {
                            case HorizontalAlignment.Left:
                                width = element.DesiredSize.Width;
                                break;

                            case HorizontalAlignment.Right:
                                width = 0.0;
                                break;

                            default:
                                width = element.DesiredSize.Width * 0.5;
                                break;
                        }
                        switch (((VerticalAlignment)element.GetValue(FrameworkElement.VerticalAlignmentProperty)))
                        {
                            case VerticalAlignment.Top:
                                height = element.DesiredSize.Height;
                                break;

                            case VerticalAlignment.Bottom:
                                height = 0.0;
                                break;

                            default:
                                height = element.DesiredSize.Height * 0.5;
                                break;
                        }
                        element.Arrange(new Rect(new Point(pixelX - width, pixelY - height), element.DesiredSize));
                    }
                }
            }
            return base.ArrangeOverride(finalSize);
        }

        private void ArrangeShapeBase(ShapeElement sb, Rectangle2D clipBox, Point2D origin, double resolution)
        {
            if (sb.Visibility != Visibility.Collapsed)
            {
                Rectangle2D b = GetBounds(sb);
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double x = (b.Left - origin.X) / resolution;
                    double y = (origin.Y - b.Top) / resolution;

                    if (((b.Width > 0.0) || (b.Height > 0.0)) && (sb.Point2Ds != null))
                    {
                        double ratio = sb.Resolution / resolution;
                        this.SetClipShapeBase(sb, clipBox, resolution);
                        if (sb.ClippedPoint2Ds != null)
                        {
                            b = sb.ClippedPoint2Ds.GetBounds();
                            if (Rectangle2D.IsNullOrEmpty(b))
                            {
                                return;
                            }
                            x = (b.Left - origin.X) / resolution;
                            y = (origin.Y - b.Top) / resolution;
                        }

                        if (sb.EncapsulatedShape.RenderTransform is ScaleTransform)
                        {
                            (sb.EncapsulatedShape.RenderTransform as ScaleTransform).ScaleX = (sb.EncapsulatedShape.RenderTransform as ScaleTransform).ScaleY = ratio;
                        }
                        else
                        {
                            sb.EncapsulatedShape.RenderTransform = new ScaleTransform { ScaleX = ratio, ScaleY = ratio };
                        }
                        double num4 = b.Width / sb.Resolution * ratio + 10.0;
                        double num5 = b.Height / sb.Resolution * ratio + 10.0;
                        num4 = Math.Min(32000.0, num4);
                        num5 = Math.Min(32000.0, num5);
                        sb.Arrange(new Rect(x, y, num4, num5));
                    }
                    else
                    {
                        sb.Arrange(new Rect(new Point(x, y), sb.DesiredSize));
                    }
                }
            }
        }

        //对于SL自带的Rectangle暂不做处理
        private void SetClipShapeBase(ShapeElement sb, Rectangle2D clipBox, double resolution)
        {
            Rectangle2D bounds = sb.Point2Ds.GetBounds();
            if ((Rectangle2D.IsNullOrEmpty(bounds)) || ((bounds.Width / resolution) < 16383.5))
            {
                if (sb.ClippedPoint2Ds != null)
                {
                    sb.ClearClip();
                }
            }
            else if (((Rectangle2D.IsNullOrEmpty(sb.ClipBox)) || !clipBox.Within(sb.ClipBox)) || ((sb.ClipBox.Width / resolution) >= 16383.5))
            {
                if (sb is PolygonElement)
                {
                    sb.SetClip(new PolygonElementClip(clipBox).Clip(sb.Point2Ds), clipBox);
                }
                else if (sb is PolylineElement)
                {
                    sb.SetClip(new PolylineElementClip(clipBox).Clip(sb.Point2Ds), clipBox);
                }
            }
        }


        //获取是否需要变换bounds 以及bounds是否可以变换
        private Rectangle2D isBoundsChange(Rectangle2D bounds, double referenceLength, Size referenceSize, out bool isIntersect, out bool isGetNewBounds)
        {
            //初始化要修改的范围
            isIntersect = false;
            isGetNewBounds = false;
            var boundsCollection = (Layer as ElementsLayer).BoundsCollection.Values;
            var radius = referenceLength * 0.7;
            Rectangle2D newBounds = new Rectangle2D();
            if (referenceSize.Width == 0 && referenceSize.Height == 0)
            {
                referenceSize.Width = bounds.Width;
                referenceSize.Height = bounds.Height;
            }
            else
            {
                referenceSize.Width *= this.Resolution;
                referenceSize.Height *= this.Resolution;

            }

            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2, bounds.Bottom + radius, bounds.Right + referenceSize.Width / 2, bounds.Top + radius);
                        break;
                    case 1:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2 + radius, bounds.Bottom, bounds.Right + referenceSize.Width / 2 + radius, bounds.Top);
                        break;
                    case 2:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2, bounds.Bottom - radius, bounds.Right + referenceSize.Width / 2, bounds.Top - radius);
                        break;
                    case 3:
                        newBounds = new Rectangle2D(bounds.Left, bounds.Bottom - radius, bounds.Right, bounds.Top - radius);
                        break;
                    case 4:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2, bounds.Bottom - radius, bounds.Right - referenceSize.Width / 2, bounds.Top - radius);
                        break;
                    case 5:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2 - radius, bounds.Bottom, bounds.Right - referenceSize.Width / 2 - radius, bounds.Top);
                        break;
                    case 6:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2, bounds.Bottom + radius, bounds.Right - referenceSize.Width / 2, bounds.Top + radius);
                        break;
                    case 7:
                        newBounds = new Rectangle2D(bounds.Left, bounds.Bottom + radius, bounds.Right, bounds.Top + radius);
                        break;
                }

                if (computeIntersectWithAll(newBounds))
                {
                    isIntersect = true;
                    if (i == 1 || i == 5)
                    {
                        continue;
                    }

                    newBounds = new Rectangle2D(newBounds.Left, newBounds.Bottom + referenceSize.Height, newBounds.Right, newBounds.Top + referenceSize.Height);
                    if (computeIntersectWithAll(newBounds))
                    {
                        newBounds = new Rectangle2D(newBounds.Left, newBounds.Bottom - 2 * referenceSize.Height, newBounds.Right, newBounds.Top - 2 * referenceSize.Height);
                        if (computeIntersectWithAll(newBounds))
                        {
                            continue;
                        }
                        else
                        {
                            isGetNewBounds = true;
                            return newBounds;
                        }
                    }
                    else
                    {
                        isGetNewBounds = true;
                        return newBounds;

                    }
                }
                else
                {
                    isGetNewBounds = true;
                    return newBounds;

                }
            }
            return bounds;
        }

        //比较计算后bounds和每个bounds的相交
        private bool computeIntersectWithAll(Rectangle2D bounds)
        {
            var boundsCollection = (Layer as ElementsLayer).BoundsCollection.Values;
            foreach (Rectangle2D rect in boundsCollection)
            {
                if (rect != null)
                {
                    if (bounds.IntersectsWith(rect))
                    {
                        return true;
                    }
                }
            }
            foreach (Rectangle2D rect in orignalBounds)
            {
                if (rect != null)
                {
                    if (bounds.IntersectsWith(rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
