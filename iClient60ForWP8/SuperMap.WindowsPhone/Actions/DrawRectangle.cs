using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    /// <summary>
    /// 	<para>${ui_action_DrawRect_Title}。</para>
    /// 	<para>${ui_action_DrawRect_Description_sl}</para>
    /// </summary>
    public class DrawRectangle : MapAction , IDrawStyle
    {
        private Point2D startPt;
        private Rectangle rectangle;
        private bool isActivated;
        private bool isDrawing;

        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        /// <summary>${ui_action_DrawRect_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;
        /// <summary>${ui_action_DrawRect_constructor_None_D}</summary>
        /// <overloads>${ui_action_DrawRect_constructor_overloads}</overloads>
        public DrawRectangle( )
        {
        }

        /// <summary>${ui_action_DrawRect_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawRectangle draw = new DrawRectangle(MyMap,Cursors.Stylus)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawRect_constructor_Map_param_map}</param>
        public DrawRectangle(Map map )
            : base(map , "DrawRectangle")
        {
            startPt = Point2D.Empty;
            if (map.Theme == null)
            {
                Stroke = new SolidColorBrush(Colors.Green);
                StrokeThickness = MagicNumber.ACTION_STYLE_DEFAULT_STROKETHICKNESS;
                Fill = new SolidColorBrush(Colors.Black);
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
            DrawLayer = new ElementsLayer();
            Map.Layers.Add(DrawLayer);
            rectangle = new Rectangle();
            rectangle.Stroke = this.Stroke;
            rectangle.StrokeThickness = this.StrokeThickness;
            rectangle.StrokeMiterLimit = this.StrokeMiterLimit;
            rectangle.StrokeDashOffset = this.StrokeDashOffset;
            rectangle.StrokeDashArray = this.StrokeDashArray;
            rectangle.StrokeDashCap = this.StrokeDashCap;
            rectangle.StrokeEndLineCap = this.StrokeEndLineCap;
            rectangle.StrokeLineJoin = this.StrokeLineJoin;
            rectangle.StrokeStartLineCap = this.StrokeStartLineCap;
            rectangle.Opacity = this.Opacity;
            rectangle.Fill = this.Fill;

            rectangle.SetValue(ElementsLayer.BBoxProperty , new Rectangle2D(item , item));
            DrawLayer.Children.Add(rectangle);

            isActivated = true;
            isDrawing = true;
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate( )
        {
            isActivated = false;
            isDrawing = false;
            rectangle = null;
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
            //Point oldPt = e.GetPosition(MapControl);
            //Point newPt = oldPt;
            //if (MapControl.Map.Angle != 0.0)
            //{
            //    double radian = MapControl.Map.Angle / 180.0 * Math.PI;//变成弧度
            //    Point transOrigin = new Point(MapControl.ActualWidth / 2, MapControl.ActualHeight / 2);
            //    newPt = MathUtility.TransformPoint(oldPt, transOrigin, radian);
            //}
            startPt = Map.ScreenToMap(e.GetPosition(Map));

            if (!isActivated)
            {
                Activate(startPt);
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
                //Point oldPt = e.GetPosition(MapControl);
                //Point newPt = oldPt;
                //if (MapControl.Map.Angle != 0.0)
                //{
                //    double radian = MapControl.Map.Angle / 180.0 * Math.PI;//变成弧度
                //    Point transOrigin = new Point(MapControl.ActualWidth / 2, MapControl.ActualHeight / 2);
                //    newPt = MathUtility.TransformPoint(oldPt, transOrigin, radian);
                //}

                Point2D item = Map.ScreenToMap(e.GetPosition(Map));

                double maxX = Math.Max(startPt.X , item.X);
                double minX = Math.Min(startPt.X , item.X);
                double maxY = Math.Max(startPt.Y , item.Y);
                double minY = Math.Min(startPt.Y , item.Y);

                Rectangle2D bounds = new Rectangle2D(minX , minY , maxX , maxY);
                rectangle.SetValue(ElementsLayer.BBoxProperty , bounds);
            }

            base.OnMouseMove(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseUp_param_e}</param>
        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            endDraw();
            e.Handled = true;
            base.OnMouseLeftButtonUp(e);
        }

        private void OnDrawCompleted(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this , args);
            }
        }

        private void endDraw(bool isCancel = false)
        {
            if (rectangle != null)
            {
                Rectangle2D bounds = (Rectangle2D)rectangle.GetValue(ElementsLayer.BBoxProperty);
                if (bounds.IsEmpty || bounds.Width == 0.0 || bounds.Height == 0.0)
                {
                    return;
                }

                //GeoRegion geoRegion = new GeoRegion();//构造返回的Geometry
                //Point2DCollection geoPoints = new Point2DCollection();
                //geoPoints.Add(bounds.BottomLeft);
                //geoPoints.Add(new Point2D(bounds.Right, bounds.Bottom));
                //geoPoints.Add(bounds.TopRight);
                //geoPoints.Add(new Point2D(bounds.Left, bounds.Top));
                //geoPoints.Add(bounds.BottomLeft);//需要添加起始点做为最后一个点
                //geoRegion.Parts.Add(geoPoints);
                GeoRegion geoRegion = bounds.ToGeoRegion();

                DrawEventArgs args2 = new DrawEventArgs
                {
                    DrawName = this.Name,
                    Element = this.rectangle,
                    Geometry = geoRegion,
                    Canceled = isCancel
                };

                Deactivate();
                OnDrawCompleted(args2);
            }
        }
        internal ElementsLayer DrawLayer { get; private set; }
        /// <summary>${ui_action_MapAction_attribute_Rectangle_D}</summary>
        protected Rectangle Rectangle
        {
            get
            {
                return this.rectangle;
            }
        }

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
