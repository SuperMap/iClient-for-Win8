using System;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.Devices.Input;
using Windows.UI.Input;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawPolygon_Title}。</para>
    /// 	<para>${ui_action_DrawPolygon_Description_sl}</para>
    /// </summary>
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawPolygon : Pan, IDrawStyle
    {
        private PolygonElement _polygon;
        private Point2DCollection _points;
        private Point2D _startPoint;
        private bool _isActivated;
        private int _pointsCount;
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        /// <summary>${ui_action_DrawPolygon_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;

        /// <summary>${ui_action_DrawPolygon_constructor_Map_D}</summary>
        /// <example>
        /// <code lang="CS">
        /// DrawPolygon draw = new DrawPolygon(MyMap);
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawPolygon_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public DrawPolygon(Map map)
            : base(map)
        {
            this.Name = "DrawPolygon";
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
            _startPoint = firstPoint;

            _polygon = new PolygonElement();
            #region 所有风格的控制
            _polygon.Stroke = this.Stroke;
            _polygon.StrokeThickness = this.StrokeThickness;
            _polygon.StrokeMiterLimit = this.StrokeMiterLimit;
            _polygon.StrokeDashOffset = this.StrokeDashOffset;
            _polygon.StrokeDashArray = this.StrokeDashArray;
            _polygon.StrokeDashCap = this.StrokeDashCap;
            _polygon.StrokeEndLineCap = this.StrokeEndLineCap;
            _polygon.StrokeLineJoin = this.StrokeLineJoin;
            _polygon.StrokeStartLineCap = this.StrokeStartLineCap;
            _polygon.Opacity = this.Opacity;
            _polygon.Fill = this.Fill;
            _polygon.FillRule = this.FillRule;
            #endregion

            _points = new Point2DCollection();
            _polygon.Point2Ds = _points;
            _points.Add(firstPoint);
            _pointsCount++;
            _points.Add(firstPoint.Clone());
            _pointsCount++;
            DrawLayer.Children.Add(_polygon);

            _isActivated = true;
        }

        /// <summary>${ui_action_DrawPolygon_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            _isActivated = false;
            _polygon = null;
            _points = null;
            _pointsCount = 0;
            _startPoint = Point2D.Empty;
            if (DrawLayer != null)
            {
                DrawLayer.Children.Clear();
            }
            if (Map != null && Map.Layers != null)
            {
                Map.Layers.Remove(DrawLayer);
            }
        }
        /// <summary>${ui_action_DrawPolygon_method_OnKeyDown_D}</summary>
        public override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (_isActivated && e.Key == VirtualKey.Escape)
            {
                endDraw(true);
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// ${ui_action_DrawPolygon_method_OnTapped_D}
        /// </summary>
        public override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            Point2D item = Map.ScreenToMap(e.GetPosition(Map));

            if (!_isActivated)
            {
                this.Activate(item);
            }
            else
            {
                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 2);
                }
                _points.Insert(_points.Count - 1, item);
                _pointsCount++;
            }
        }

        /// <summary>${ui_action_DrawPolygon_event_OnPointerMoved_D}</summary>
        public override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isActivated && !e.Pointer.IsInContact)
            {
                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 2);
                }
                Point2D item = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
                _points.Insert(_points.Count - 1, item);
            }
            base.OnPointerMoved(e);
        }

        /// <summary>${ui_action_DrawPolygon_event_OnDoubleTapped_D}</summary>
        public override void OnDoubleTapped(Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            //如果只画了小于或等于两个点的话，就不执行完成事件。
            if (_points != null && _points.Count <3)
                return;

            endDraw(false);
        }

        /// <summary>
        /// ${ui_action_DrawPolygon_event_OnHolding_D}
        /// </summary>
        public override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            if (e.HoldingState == HoldingState.Started)
            {
                //如果只画了小于或等于两个点的话，就不执行完成事件。
                if (_points == null || _points.Count < 2)
                    return;

                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 2);
                }
                Point2D item = Map.ScreenToMap(e.GetPosition(Map));
                _points.Insert(_points.Count - 1, item);
                _pointsCount++;
                endDraw(false);
            }
        }
        private void endDraw(bool isCancel = false)
        {
            if (_points != null)
            {
                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 2);
                }
                PolygonElement pRegion = new PolygonElement()
                {
                    Point2Ds = _points.Clone(),//不克隆，在返回后还与下面的GeoRegion指向一个内存地址
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

                geoRegion.Parts.Add(_points);

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
