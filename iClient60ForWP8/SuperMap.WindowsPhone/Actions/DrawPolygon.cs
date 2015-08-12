using System;
using System.Windows.Input;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawPolygon_Title}。</para>
    /// 	<para>${ui_action_DrawPolygon_Description_sl}</para>
    /// </summary>
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawPolygon : MapAction, IDrawStyle
    {
        private PolygonElement polygon;
        private Point2DCollection points;
        private Point2D startPoint;
        private bool isActivated;
        private bool isDrawing;
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        /// <summary>${ui_action_DrawPolygon_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;
        
        /// <summary>${ui_action_DrawPolygon_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawPolygon draw = new DrawPolygon(MyMap,Cursors.Stylus)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawPolygon_constructor_Map_param_map}</param>
        public DrawPolygon(Map map)
            : base(map, "DrawPolygon")
        {
            if (map.Theme == null)
            {
                Fill = new SolidColorBrush(Colors.Red);
                Stroke = new SolidColorBrush(Colors.Red);
                StrokeThickness = MagicNumber.ACTION_STYLE_DEFAULT_STROKETHICKNESS;
                Opacity = MagicNumber.ACTION_STYLE_DEFAULT_OPACITY;
            }
            else
            {
                this.Stroke = map.Theme.Stroke;
                this.StrokeThickness = map.Theme.StrokeThickness;
                this.Fill = map.Theme.Fill;
                this.Opacity = map.Theme.Opacity;
            }
        }
        private void Activate(Point2D firstPoint)
        {
            if (Map == null || Map.Layers == null)
            {
                return;
            }

            DrawLayer = new ElementsLayer();

            Map.Layers.Add(DrawLayer);
            startPoint = firstPoint;

            polygon = new PolygonElement();
            #region 所有风格的控制
            polygon.Stroke = this.Stroke;
            polygon.StrokeThickness = this.StrokeThickness;
            polygon.StrokeMiterLimit = this.StrokeMiterLimit;
            polygon.StrokeDashOffset = this.StrokeDashOffset;
            polygon.StrokeDashArray = this.StrokeDashArray;
            polygon.StrokeDashCap = this.StrokeDashCap;
            polygon.StrokeEndLineCap = this.StrokeEndLineCap;
            polygon.StrokeLineJoin = this.StrokeLineJoin;
            polygon.StrokeStartLineCap = this.StrokeStartLineCap;
            polygon.Opacity = this.Opacity;
            polygon.Fill = this.Fill;
            polygon.FillRule = this.FillRule;
            #endregion

            points = new Point2DCollection();
            polygon.Point2Ds = points;
            points.Add(firstPoint);
            points.Add(firstPoint);
            points.Add(firstPoint);

            DrawLayer.Children.Add(polygon);

            isDrawing = true;
            isActivated = true;
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            isActivated = false;
            isDrawing = false;
            polygon = null;
            points = null;
            startPoint = Point2D.Empty;
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
                Activate(item);
            }
            else
            {
                points.Insert(points.Count - 2, item);
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
                points.RemoveAt(points.Count - 2);
                points.Add(item);
            }
            base.OnMouseMove(e);
        }

        public override void OnDoubleTap(GestureEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));
            points.Insert(points.Count - 2, item);
            if (points != null && points.Count - 2 <= 2)
                return;
            endDraw(true);
            e.Handled = true;
            base.OnDoubleTap(e);
        }

        public override void OnHold(GestureEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));
            points.Insert(points.Count - 2, item);
            if (points != null && points.Count - 2 <= 2)
                return;
            endDraw(true);
            e.Handled = true;
            base.OnHold(e);
        }

        private void endDraw(bool isDblClick = false, bool isCancel = false)
        {
            if (points != null)
            {
                points.RemoveAt(points.Count - 2);
                if (isDblClick)
                {
                    points.RemoveAt(points.Count - 2);
                }

                PolygonElement pRegion = new PolygonElement()
                {
                    Point2Ds = points.Clone(),//不克隆，在返回后还与下面的GeoRegion指向一个内存地址
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
                };//构造返回Element对象

                GeoRegion geoRegion = new GeoRegion();//构造返回的Geometry
                points.Add(startPoint);//需要添加起始点做为最后一个点
                geoRegion.Parts.Add(points);

                DrawEventArgs args = new DrawEventArgs
                {
                    DrawName = Name,
                    Element = pRegion,    //Element = this.polyline  //直接返回是固定像素的
                    Geometry = geoRegion,
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
