using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawCircle_Title}</para>
    /// 	<para>${ui_action_DrawCircle_Description_sl}</para>
    /// </summary>
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawCircle : MapAction, IDrawStyle
    {
        private Point2D startPt = Point2D.Empty;
        private Ellipse ellipse;
        private bool isActivated;
        private bool isDrawing;
        private double _radius;
        /// <summary>${ui_action_DrawCircle_event_drawCompleted_D}</summary>
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        public event EventHandler<DrawEventArgs> DrawCompleted;
        
        /// <summary>${ui_action_DrawCircle_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawCircle draw = new DrawCircle(MyMap,Cursors.Wait)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawCircle_constructor_Map_param_map}</param>
        public DrawCircle(Map map)
            : base(map, "DrawCircle")
        {
            if (map.Theme == null)
            {
                Stroke = new SolidColorBrush(Colors.Black);
                StrokeThickness = MagicNumber.ACTION_STYLE_DEFAULT_STROKETHICKNESS;
                Fill = new SolidColorBrush(Colors.Red);
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

        private void Activate(Point2D item)
        {
            if (Map == null || Map.Layers == null)
            {
                return;
            }
            ellipse = new Ellipse();
            #region 所有风格的控制
            ellipse.Stroke = Stroke;
            ellipse.StrokeThickness = StrokeThickness;
            ellipse.Fill = Fill;
            ellipse.StrokeMiterLimit = StrokeMiterLimit;
            ellipse.StrokeDashOffset = StrokeDashOffset;
            ellipse.StrokeDashArray = StrokeDashArray;
            ellipse.StrokeDashCap = StrokeDashCap;
            ellipse.StrokeEndLineCap = StrokeEndLineCap;
            ellipse.StrokeLineJoin = StrokeLineJoin;
            ellipse.StrokeStartLineCap = StrokeStartLineCap;
            ellipse.Opacity = Opacity;
            #endregion

            DrawLayer = new ElementsLayer();
            Map.Layers.Add(DrawLayer);

            ellipse.SetValue(ElementsLayer.BBoxProperty, new Rectangle2D(item, item));
            DrawLayer.Children.Add(ellipse);

            isActivated = true;
            isDrawing = true;
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            isActivated = false;
            isDrawing = false;
            ellipse = null;
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
            startPt = item;

            if (!isActivated)
            {
                this.Activate(item);
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
                double width = Math.Abs(item.X - startPt.X);
                double height = Math.Abs(startPt.Y - item.Y);
                double r = Math.Sqrt(width * width + height * height);
                this._radius = r;
                Rectangle2D bounds = new Rectangle2D(startPt.X - r, startPt.Y - r, startPt.X + r, startPt.Y + r);//圆
                ellipse.SetValue(ElementsLayer.BBoxProperty, bounds);
            }

            base.OnMouseMove(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseUp_param_e}</param>
        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Point2D item = Map.ScreenToMap(e.GetPosition(Map));
            double width = Math.Abs(item.X - startPt.X);
            double height = Math.Abs(startPt.Y - item.Y);
            double r = Math.Sqrt(width * width + height * height);
            this._radius = r;
            endDraw(this._radius);
            base.OnMouseLeftButtonUp(e);
        }

        private void endDraw(double radius, bool isCancel = false)
        {
            if (this.ellipse != null)
            {
                //Point2D item = Map.ScreenToMap(e.GetPosition(Map));
                Rectangle2D b = ElementsLayer.GetBBox(ellipse);
                DrawEventArgs args2 = new DrawEventArgs
                {
                    DrawName = Name,
                    Element = ellipse, //可以由它的bbox得到半径
                    Geometry = new GeoCircle(new Point2D(startPt.X, startPt.Y), radius), //返回圆的中心点
                    Canceled = isCancel
                };
                Deactivate();
                OnDrawComplete(args2);
            }
        }

        private void OnDrawComplete(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this, args);
            }
        }
        internal ElementsLayer DrawLayer { get; private set; }
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
