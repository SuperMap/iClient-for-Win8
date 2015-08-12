using System;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawFreeHandLine_Title}</para>
    /// 	<para>${ui_action_DrawFreeHandLine_Description_sl}</para>
    /// </summary>
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawFreeLine : MapAction, IDrawStyle
    {
        private PolylineElement polyline;
        private Point2DCollection points;
        private bool isActivated;
        private bool isDrawing;
        /// <summary>${ui_action_DrawFreeHandLine_event_drawCompleted_D}</summary>
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        public event EventHandler<DrawEventArgs> DrawCompleted;
        

        /// <summary>${ui_action_DrawFreeHandPolygon_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawFreeLine draw = new DrawFreeLine(MyMap)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawFreeHandPolygon_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public DrawFreeLine(Map map)
            : base(map, "DrawFreeLine")
        {
            if (map.Theme == null)
            {
                Stroke = new SolidColorBrush(Colors.Red);
                StrokeThickness = MagicNumber.ACTION_STYLE_DEFAULT_STROKETHICKNESS;
                Opacity = MagicNumber.ACTION_STYLE_DEFAULT_OPACITY;
            }
            else
            {
                this.Stroke = map.Theme.Stroke;
                this.StrokeThickness = map.Theme.StrokeThickness;
                this.Opacity = map.Theme.Opacity;
            }
        }

        private void Activate(Point2D firstPoint)
        {
            if (Map == null || Map.Layers == null)
            {
                return;
            }

            polyline = new PolylineElement();
            polyline.Stroke = Stroke;
            polyline.StrokeThickness = StrokeThickness;
            polyline.Opacity = Opacity;
            polyline.StrokeMiterLimit = StrokeMiterLimit;
            polyline.StrokeDashOffset = StrokeDashOffset;
            polyline.StrokeDashArray = StrokeDashArray;
            polyline.StrokeDashCap = StrokeDashCap;
            polyline.StrokeEndLineCap = StrokeEndLineCap;
            polyline.StrokeLineJoin = StrokeLineJoin;
            polyline.StrokeStartLineCap = StrokeStartLineCap;
            polyline.Fill = Fill;
            polyline.FillRule = FillRule;

            points = new Point2DCollection();
            polyline.Point2Ds = points;
            points.Add(firstPoint);

            DrawLayer = new ElementsLayer();

            Map.Layers.Add(DrawLayer);
            DrawLayer.Children.Add(polyline);

            isActivated = true;
            isDrawing = true;
        }

        /// <summary>${ui_action_FrawFreeLine_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            isActivated = false;
            isDrawing = false;
            polyline = null;
            points = null;
            if (DrawLayer != null)
            {
                DrawLayer.Children.Clear();
            }
            if (Map != null && Map.Layers != null)
            {
                Map.Layers.Remove(DrawLayer);
            }
        }

        /// <summary>${ui_action_FrawFreeLine_event_OnPointerPressed_D}</summary>
        public override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
            if (!isActivated)
            {
                Activate(item);
            }
            e.Handled = true;
            base.OnPointerPressed(e);
        }

        /// <summary>${ui_action_FrawFreeLine_event_OnPointerMoved_D}</summary>
        public override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isDrawing)
            {
                Point2D item = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
                points.Add(item);
            }
            base.OnPointerMoved(e);
        }

        /// <summary>${ui_action_FrawFreeLine_event_OnPointerReleased_D}</summary>
        public override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            endDraw();
            base.OnPointerReleased(e);
        }
        
        private void endDraw(bool isCancel = false)
        {
            if (points != null)
            {
                PolylineElement pLine = new PolylineElement()
                {
                    Point2Ds = points.Clone(),
                    #region 所有风格的控制
                    Stroke = this.Stroke,
                    StrokeThickness = this.StrokeThickness,
                    StrokeMiterLimit = this.StrokeMiterLimit,
                    StrokeDashOffset = this.StrokeDashOffset,
                    StrokeDashArray = this.StrokeDashArray,
                    StrokeDashCap = this.StrokeDashCap,
                    StrokeEndLineCap = this.StrokeEndLineCap,
                    StrokeLineJoin = this.StrokeLineJoin,
                    StrokeStartLineCap = this.StrokeStartLineCap,
                    Opacity = this.Opacity,
                    Fill = this.Fill,
                    FillRule = this.FillRule
                    #endregion
                };
                GeoLine geoLine = new GeoLine();//构造返回的Geometry
                geoLine.Parts.Add(points);
                DrawEventArgs args = new DrawEventArgs
                {
                    DrawName = Name,
                    Element = pLine,
                    Geometry = geoLine,
                    Canceled = isCancel
                };

                Deactivate();
                OnDrawCompleted(args);
            }
        }

        private void OnDrawCompleted(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this, args);
            }
        }

        internal ElementsLayer DrawLayer { get; private set; }
        /// <summary>${ui_action_DrawLine_attribute_fillRule}</summary>
        public FillRule FillRule { get; set; }

        #region IDrawStyle 成员
        /// <summary>${ui_action_IDrawStyle}</summary>
        public Brush Fill { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public Brush Stroke { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public double StrokeThickness { get; set; }

        /// <summary>${ui_action_IDrawStyle}</summary>
        public double StrokeMiterLimit { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public double StrokeDashOffset { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public DoubleCollection StrokeDashArray { get; set; }

        /// <summary>${ui_action_IDrawStyle}</summary>
        public PenLineCap StrokeDashCap { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public PenLineCap StrokeEndLineCap { get; set; }
        /// <summary>${ui_action_IDrawStyle}</summary>
        public PenLineCap StrokeStartLineCap { get; set; }

        /// <summary>${ui_action_IDrawStyle}</summary>
        public PenLineJoin StrokeLineJoin { get; set; }

        /// <summary>${ui_action_IDrawStyle}</summary>
        public double Opacity { get; set; }
        #endregion
    }
}
