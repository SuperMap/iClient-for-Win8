using System;
using System.Windows.Input;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;
using Microsoft.Devices;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawLine_Title}。</para>
    /// 	<para>${ui_action_DrawLine_Description_sl}</para>
    /// </summary>
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawLine : MapAction, IDrawStyle
    {
        private PolylineElement polyline;
        private Point2DCollection points;
        private bool isActivated;
        private bool isDrawing;

        /// <summary>${ui_action_DrawLine_event_drawCompleted_D}</summary>
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        public event EventHandler<DrawEventArgs> DrawCompleted;
       
        /// <summary>${ui_action_DrawLine_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawLine draw = new DrawLine(MyMap,Cursors.Stylus)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawLine_constructor_Map_param_map}</param>
        public DrawLine(Map map)
            : base(map, "DrawLine")
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
            DrawLayer = new ElementsLayer();
            if (Map.Layers == null)
            {
                return;
            }
            Map.Layers.Add(DrawLayer);

            polyline = new PolylineElement();
            #region 所有风格的控制
            polyline.Stroke = Stroke;
            polyline.StrokeThickness = StrokeThickness;
            polyline.StrokeMiterLimit = StrokeMiterLimit;
            polyline.StrokeDashOffset = StrokeDashOffset;
            polyline.StrokeDashArray = StrokeDashArray;
            polyline.StrokeDashCap = StrokeDashCap;
            polyline.StrokeEndLineCap = StrokeEndLineCap;
            polyline.StrokeLineJoin = StrokeLineJoin;
            polyline.StrokeStartLineCap = StrokeStartLineCap;
            polyline.Opacity = Opacity;
            polyline.Fill = Fill;
            polyline.FillRule = FillRule;
            #endregion

            points = new Point2DCollection();
            polyline.Point2Ds = points;
            points.Add(firstPoint);
            points.Add(firstPoint);

            DrawLayer.Children.Add(polyline);

            isDrawing = true;
            isActivated = true;
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
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
        
        /// <summary>${ui_action_MapAction_event_onMouseDown_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseDown_param_e}</param>
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));

            if (!isActivated)
            {
                this.Activate(item);
            }
            else
            {
                points.Add(item);
            }
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseMove_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseMove_param_e}</param>
        public override void OnMouseMove(MouseEventArgs e)
        {
            if (isDrawing)
            {
                Point2D item = Map.ScreenToMap(e.GetPosition(Map));

                points.RemoveAt(points.Count - 1);
                points.Add(item);
            }

            base.OnMouseMove(e);
        }

        public override void OnDoubleTap(GestureEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));
            points.Add(item);
            endDraw(true);
            e.Handled = true;
            base.OnDoubleTap(e);
        }

        public override void OnHold(GestureEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));
            points.Add(item);
            endDraw(true);
            e.Handled = true;
            base.OnHold(e);
        }

        private void endDraw(bool isDblClick = false, bool isCancel = false)
        {
            
            if (points != null)
            {
                if (isDblClick)
                {
                    points.RemoveAt(points.Count - 1);
                }
                PolylineElement pLine = new PolylineElement()
                {
                    Point2Ds = this.points.Clone(),
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
                    Element = pLine,    //Element = this.polyline  //直接返回是固定像素的
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
