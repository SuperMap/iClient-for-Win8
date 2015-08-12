using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SuperMap.WinRT.Mapping
{
    internal class TiledLayerContainer : LayerContainer
    {
        internal TiledLayerContainer(Layer layer)
            : base(layer)
        {

        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            Point2D origin = this.Origin;
            double resolution = this.Resolution;
            bool hasLine = Layer.Map.MapStatus == MapStatus.Panning || Layer.Map.MapStatus == MapStatus.Zooming;

            foreach (UIElement element in base.Children)
            {
                if (element.Visibility == Visibility.Collapsed)
                {
                    continue;
                }
                Rectangle2D b = GetBounds(element);
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double pixelX = (b.Left - origin.X) / resolution;
                    double pixelY = (origin.Y - b.Top) / resolution;
                    double pixelWidth = b.Width / resolution;
                    double pixelHeight = b.Height / resolution;

                    pixelWidth++;
                    pixelHeight++;
                    element.UseLayoutRounding = true;
                    if (DoubleUtil.ValueCheck(pixelHeight) && DoubleUtil.ValueCheck(pixelWidth))
                    {
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
                    }
                }
            }
            return base.ArrangeOverride(finalSize);
        }

    }
}
