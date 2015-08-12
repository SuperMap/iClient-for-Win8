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
    internal class FeaturesLayerContainer:LayerContainer
    {
        internal FeaturesLayerContainer(Layer layer)
            : base(layer)
        {

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point2D origin = this.Origin;
            double resolution = this.Resolution;
            Rectangle2D clipBox = new Rectangle2D(
                origin.X - (resolution * 4191.0),
                origin.Y - (resolution * 4191.0),
                origin.X + (resolution * 4191.0),
                origin.Y + (resolution * 4191.0));
            foreach (UIElement element in base.Children)
            {
                if (element.Visibility == Visibility.Collapsed)
                {
                    continue;
                }
                this.ArrangeFeature(element as FeatureElement, clipBox, origin, resolution);
            }
            return base.ArrangeOverride(finalSize);
        }

        private void ArrangeFeature(FeatureElement elm, Rectangle2D clipBox, Point2D origin, double resolution)
        {
            if (elm.Visibility != Visibility.Collapsed)
            {
                Rectangle2D b = GetBounds(elm);
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double x = (b.Left - origin.X) / resolution;
                    double y = (origin.Y - b.Top) / resolution;
                    if (((b.Width > 0.0) || (b.Height > 0.0)) && (elm.PathGeometry != null))
                    {
                        double ratio = elm.Resolution / resolution;
                        this.SetClip(elm, clipBox, resolution);
                        if (elm.ClippedGeometry != null)
                        {
                            b = elm.ClippedGeometry.Bounds;
                            if (Rectangle2D.IsNullOrEmpty(b))
                            {
                                return;
                            }
                            x = (b.Left - origin.X) / resolution;
                            y = (origin.Y - b.Top) / resolution;
                        }
                        if (elm.PathGeometry.Transform is ScaleTransform)
                        {
                            (elm.PathGeometry.Transform as ScaleTransform).ScaleX = (elm.PathGeometry.Transform as ScaleTransform).ScaleY = ratio;
                        }
                        else
                        {
                            elm.PathGeometry.Transform = new ScaleTransform { ScaleX = ratio, ScaleY = ratio };
                        }

                        double num4 = ((b.Width / elm.Resolution) * ratio) + 10.0;
                        double num5 = ((b.Height / elm.Resolution) * ratio) + 10.0;//这也已经加了10.0
                        num4 = Math.Min(32000.0, num4);
                        num5 = Math.Min(32000.0, num5);
                        elm.Arrange(new Rect(x, y, num4, num5));
                    }
                    else
                    {
                        elm.Arrange(new Rect(new Point(x, y), elm.DesiredSize));
                    }
                }
            }
        }

        private void SetClip(FeatureElement fe, Rectangle2D clipBox, double resolution)
        {
            Rectangle2D bounds = fe.Geometry.Bounds;
            if ((Rectangle2D.IsNullOrEmpty(bounds)) || ((bounds.Width / resolution) < 16383.5))
            {
                if (fe.ClippedGeometry != null)
                {
                    fe.ClearClip();
                }
            }
            else if (((Rectangle2D.IsNullOrEmpty(fe.ClipBox)) || !clipBox.Within(fe.ClipBox)) || ((fe.ClipBox.Width / resolution) >= 16383.5))
            {
                if (fe.Geometry is GeoRegion)
                {
                    fe.SetClip(new GeoRegionClip(clipBox).Clip(fe.Geometry as GeoRegion), clipBox);
                }
                else if (fe.Geometry is GeoLine)
                {
                    fe.SetClip(new GeoLineClip(clipBox).Clip(fe.Geometry as GeoLine), clipBox);
                }
            }
        }


    }
}
