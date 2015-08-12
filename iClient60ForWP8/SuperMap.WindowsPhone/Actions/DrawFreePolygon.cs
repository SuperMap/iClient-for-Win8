using System;
using System.Windows.Input;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawFreeHandPolygon_Title}。</para>
    /// 	<para>${ui_action_DrawFreeHandPolygon_Description}</para>
    /// </summary>
    public class DrawFreePolygon : MapAction, IDrawStyle
    {
        private PolygonElement polygon;
        private Point2DCollection points;
        private Point2D startPt;
        private bool isActivated;
        private bool isDrawing;

        /// <summary>${ui_action_DrawFreeHandPolygon_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;
        
        /// <summary>${ui_action_DrawFreeHandPolygon_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawFreePolygon draw = new DrawFreePolygon(MyMap,Cursors.Stylus)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawFreeHandPolygon_constructor_Map_param_map}</param>
        public DrawFreePolygon(Map map)
            : base(map, "DrawFreePolygon")
        {
            if (map.Theme == null)
            {
                Stroke = new SolidColorBrush(Colors.Red);
                StrokeThickness = MagicNumber.ACTION_STYLE_DEFAULT_STROKETHICKNESS;
                Opacity = MagicNumber.ACTION_STYLE_DEFAULT_OPACITY;
                Fill = new SolidColorBrush(Colors.Green);
            }
            else
            {
                this.Stroke = map.Theme.Stroke;
                this.StrokeThickness = map.Theme.StrokeThickness;
                this.Fill = map.Theme.Fill;
                this.Opacity = map.Theme.Opacity;
            }
        }

        private void Activate()
        {
            if (Map == null || Map.Layers == null)
            {
                return;
            }

            polygon = new PolygonElement();
            #region 所有风格的控制
            polygon.Stroke = Stroke;
            polygon.StrokeThickness = StrokeThickness;
            polygon.StrokeMiterLimit = StrokeMiterLimit;
            polygon.StrokeDashOffset = StrokeDashOffset;
            polygon.StrokeDashArray = StrokeDashArray;
            polygon.StrokeDashCap = StrokeDashCap;
            polygon.StrokeEndLineCap = StrokeEndLineCap;
            polygon.StrokeLineJoin = StrokeLineJoin;
            polygon.StrokeStartLineCap = StrokeStartLineCap;
            polygon.Opacity = Opacity;
            polygon.Fill = Fill;
            polygon.FillRule = FillRule;
            #endregion
            points = new Point2DCollection();
            polygon.Point2Ds = points;
            points.Add(startPt);

            DrawLayer = new ElementsLayer();

            Map.Layers.Add(DrawLayer);
            DrawLayer.Children.Add(polygon);

            isActivated = true;
            isDrawing = true;
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            isActivated = false;
            isDrawing = false;
            polygon = null;
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
            startPt = Map.ScreenToMap(e.GetPosition(Map));

            if (!isActivated)
            {
                Activate();
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
                points.Add(item);
            }
            base.OnMouseMove(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseUp_param_e}</param>
        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            endDraw();
            base.OnMouseLeftButtonUp(e);
        }

        private void endDraw(bool isCancel = false)
        {
            if (points != null)
            {
                PolygonElement pRegion = new PolygonElement()
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
                GeoRegion geoRegion = new GeoRegion();//构造返回的Geometry
                points.Add(startPt);//需要添加起始点做为最后一个点
                geoRegion.Parts.Add(points);

                DrawEventArgs args = new DrawEventArgs
                {
                    DrawName = Name,
                    Element = pRegion,
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
