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
    /// 	<para>${ui_action_DrawLine_Title}</para>
    /// 	<para>${ui_action_DrawLine_Description_sl}</para>
	/// </summary>
    /// <example>
    /// <code lang="CS">
    /// DrawLine draw = new DrawPoint(MyMap);
    /// MyMap.Action=draw;
    /// draw.DrawCompleted += drawLine_DrawCompleted;
    /// void drawLine_DrawCompleted(object sender, DrawEventArgs e)
    /// {
    ///     //TODO
    /// }
    /// </code>
    /// </example>
    /// 
    /// <seealso cref="IDrawStyle">IDrawStyle Interface</seealso>
    public class DrawLine : Pan, IDrawStyle
    {
        private PolylineElement _polyline;
        private Point2DCollection _points;
        private bool _isActivated;
        private int _pointsCount;

        /// <summary>${ui_action_DrawLine_event_drawCompleted_D}</summary>
        /// <seealso cref="DrawEventArgs">DrawEventArgs Class</seealso>
        public event EventHandler<DrawEventArgs> DrawCompleted;

        /// <summary>${ui_action_DrawLine_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawLine draw = new DrawLine(MyMap)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawLine_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public DrawLine(Map map)
            : base(map)
        {
            this.Name = "DrawLine";
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

            _polyline = new PolylineElement();
            #region 所有风格的控制
            _polyline.Stroke = Stroke;
            _polyline.StrokeThickness = StrokeThickness;
            _polyline.StrokeMiterLimit = StrokeMiterLimit;
            _polyline.StrokeDashOffset = StrokeDashOffset;
            _polyline.StrokeDashArray = StrokeDashArray;
            _polyline.StrokeDashCap = StrokeDashCap;
            _polyline.StrokeEndLineCap = StrokeEndLineCap;
            _polyline.StrokeLineJoin = StrokeLineJoin;
            _polyline.StrokeStartLineCap = StrokeStartLineCap;
            _polyline.Opacity = Opacity;
            _polyline.Fill = Fill;
            _polyline.FillRule = FillRule;
            #endregion

            _points = new Point2DCollection();
            _polyline.Point2Ds = _points;
            _points.Add(firstPoint);
            _pointsCount++;
            DrawLayer.Children.Add(_polyline);

            _isActivated = true;
        }

        /// <summary>${ui_action_DrawLine_method_deactivate_D}</summary>
        public override void Deactivate()
        {
            _isActivated = false;
            _polyline = null;
            _points = null;
            _pointsCount = 0;
            if (DrawLayer != null)
            {
                DrawLayer.Children.Clear();
            }
            if (Map != null && Map.Layers != null)
            {
                Map.Layers.Remove(DrawLayer);
            }
        }
        /// <summary>${ui_action_DrawLine_event_OnKeyDown_D}</summary>
        public override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);
            if (_isActivated && e.Key == VirtualKey.Escape)
            {
                endDraw(true);
            }
        }

        /// <summary>
        /// ${ui_action_DrawLine_event_OnTapped_D}
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
                    _points.RemoveAt(_points.Count - 1);
                }
                _points.Add(item);
                _pointsCount++;
            }
        }

        /// <summary>${ui_action_DrawLine_event_OnPointerMoved_D}</summary>
        public override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_isActivated && !e.Pointer.IsInContact)
            {
                Point2D item = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 1);
                }
                _points.Add(item);
            }

        }

        /// <summary>
        /// ${ui_action_DrawLine_event_OnDoubleTapped_D}
        /// </summary>
        public override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            //DoubleTap时会先触发一次Tap，因此此处不再需要添加。
            //DoubleTap会引发Pan的放大，在此时不需要的，因此不能调用base的OnDoubleTapped。
            if (_points == null || _points.Count < 2)
            {
                return;
            }
            endDraw(false);
        }

        /// <summary>
        /// ${ui_action_DrawLine_event_OnHolding_D}
        /// </summary>
        public override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            if (e.HoldingState == HoldingState.Started)
            {
                Point2D item = Map.ScreenToMap(e.GetPosition(Map));

                if (_points == null || _points.Count < 1)
                    return;

                else
                {
                    while (_points.Count > _pointsCount)
                    {
                        _points.RemoveAt(_points.Count - 1);
                    }
                    _points.Add(item);
                    _pointsCount++;
                }
                endDraw(false);
            }
        }

        private void endDraw(bool isCancel = false)
        {
            if (_points != null)
            {
                while (_points.Count > _pointsCount)
                {
                    _points.RemoveAt(_points.Count - 1);
                }
                PolylineElement pLine = new PolylineElement()
                {
                    Point2Ds = this._points.Clone(),
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
                geoLine.Parts.Add(_points);
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
