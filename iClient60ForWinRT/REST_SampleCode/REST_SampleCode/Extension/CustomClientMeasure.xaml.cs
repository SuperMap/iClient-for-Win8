using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using Windows.Devices.Input;
using Windows.Storage;
using Windows.UI.Input;

namespace REST_SampleCode
{
    public partial class CustomClientMeasure : Page
    {
        TiledBingMapsLayer _layer;
        ElementsLayer _eLayer;

        public CustomClientMeasure()
        {
            InitializeComponent();
            _layer = MyMap.Layers["bingMapLayer"] as TiledBingMapsLayer;
            _eLayer = MyMap.Layers["ELayer"] as ElementsLayer;
            this.Unloaded += CustomClientMeasure_Unloaded;
        }

        void CustomClientMeasure_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= CustomClientMeasure_Unloaded;
            MyMap.Dispose();
        }

        private void mybtmeasuredistance_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action = new MeasureDistance(MyMap, _eLayer);
        }

        private void mybtnmeasurearea_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action = new MeasureArea(MyMap, _eLayer);

        }

        private void pan_Click(object sender, RoutedEventArgs e)
        {

            Pan pan = new Pan(MyMap);
            MyMap.Action = pan;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action.Deactivate();
            _eLayer.Children.Clear();
        }

    }

    public class MeasureArea : Pan, IDrawStyle
    {
        private PolygonElement _polygon;
        private Point2DCollection _points;
        private bool _isActivated;
        private int _pointsCount = 0;
        private List<TextBlock> _textBlockContainer = new List<TextBlock>();

        public MeasureArea(Map map, ElementsLayer elementsLayer)
            : base(map)
        {
            Name = "MeasureArea";
            Map = map;
            Stroke = new SolidColorBrush(Colors.Red);
            Fill = new SolidColorBrush(Color.FromArgb(0x99, 255, 255, 255));
            StrokeThickness = 3;
            Opacity = 1;
            DrawLayer = elementsLayer;
        }

        private void Activate(Point2D firstPoint)
        {
            _polygon = new PolygonElement();
            _textBlockContainer = new List<TextBlock>();
            #region 所有风格的控制
            _polygon.Stroke = Stroke;
            _polygon.StrokeThickness = StrokeThickness;
            _polygon.StrokeMiterLimit = StrokeMiterLimit;
            _polygon.StrokeDashOffset = StrokeDashOffset;
            _polygon.StrokeDashArray = StrokeDashArray;
            _polygon.StrokeDashCap = StrokeDashCap;
            _polygon.StrokeEndLineCap = StrokeEndLineCap;
            _polygon.StrokeLineJoin = StrokeLineJoin;
            _polygon.StrokeStartLineCap = StrokeStartLineCap;
            _polygon.Opacity = Opacity;
            _polygon.Fill = Fill;
            _polygon.FillRule = FillRule;
            #endregion

            _points = new Point2DCollection();
            _polygon.Point2Ds = _points;
            _points.Add(firstPoint);
            _pointsCount++;
            _points.Add(firstPoint.Clone());
            _pointsCount++;
            DrawLayer.Children.Add(_polygon);

            TextBlock textBlock = new TextBlock();
            textBlock.FontWeight = FontWeights.ExtraBlack;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.Text = "起点";
            _textBlockContainer.Add(textBlock);

            DrawLayer.AddChild(textBlock, firstPoint);
            _isActivated = true;
        }

        public override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            Point2D point = Map.ScreenToMap(e.GetPosition(Map));

            if (!_isActivated)
            {
                Activate(point);
            }
            else
            {
                DrawPoint(point, false);
            }

        }

        public override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_isActivated && !e.Pointer.IsInContact)
            {
                Point2D point = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
                DrawPoint(point, true);
            }

        }

        private void DrawPoint(Point2D point, bool isTemp)
        {
            bool needRemovePoints = _points.Count > _pointsCount;
            while (_points.Count > _pointsCount)
            {
                _points.RemoveAt(_points.Count - 2);
            }
            _points.Insert(_points.Count - 1, point);
            if (!isTemp)
            {
                _pointsCount++;
            }
            string areaString = string.Format("{0:0.0000}平方千米", GetArea(_points));
            if (needRemovePoints)
            {
                _textBlockContainer[_textBlockContainer.Count - 1].Text = areaString;
                DrawLayer.Children.Remove(_textBlockContainer[_textBlockContainer.Count - 1]);
                DrawLayer.AddChild(_textBlockContainer[_textBlockContainer.Count - 1], point);
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.FontWeight = FontWeights.ExtraBlack;
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                textBlock.Text = areaString;
                _textBlockContainer.Add(textBlock);

                DrawLayer.AddChild(textBlock, point);
            }
        }

        private double GetArea(Point2DCollection point2DCollection)
        {
            #region 法1：

            double tempArea = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i < point2DCollection.Count - 1; i++)
            {
                x1 = point2DCollection[i].X;
                x2 = point2DCollection[i + 1].X;
                y1 = point2DCollection[i].Y;
                y2 = point2DCollection[i + 1].Y;
                //tempArea += x1 * yDiff - y1 * xDiff;
                tempArea += x1 * y2 - x2 * y1;
            }
            tempArea += point2DCollection[point2DCollection.Count - 1].X * point2DCollection[0].Y -
                        point2DCollection[point2DCollection.Count - 1].Y * point2DCollection[0].X;
            return Math.Abs(tempArea) / 2000000;

            #endregion

            #region 法2：
            //int i, j;
            //double ar = 0;

            //for (i = 0; i < point2DCollection.Count; i++)
            //{
            //    j = (i + 1) % point2DCollection.Count;
            //    ar += point2DCollection[i].X * point2DCollection[j].Y;
            //    ar -= point2DCollection[i].Y * point2DCollection[j].X;
            //}

            //ar /= 2;
            //return (ar < 0 ? -ar : ar);
            #endregion
        }

        public override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            if (_points == null || _points.Count < 3)
            {
                //当点的个数小余三个时，不能结束。
                return;
            }
            Complete();
        }

        public override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            if (e.HoldingState == HoldingState.Started)
            {
                Point2D point = Map.ScreenToMap(e.GetPosition(Map));

                if (_points == null || _points.Count < 2)
                {
                    //当点的个数小余两个时，不能结束。
                    return;
                }
                else
                {
                    DrawPoint(point, false);
                    Complete();
                }
            }
        }

        private void Complete()
        {
            _points = null;
            _isActivated = false;
            _pointsCount = 0;
            _polygon = null;
            _textBlockContainer = null;
        }

        public override void Deactivate()
        {
            Complete();
            DrawLayer.Children.Clear();
        }

        public FillRule FillRule { get; set; }
        public ElementsLayer DrawLayer { get; set; }

        #region IDrawStyle 成员
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public double StrokeMiterLimit { get; set; }
        public double StrokeDashOffset { get; set; }
        public DoubleCollection StrokeDashArray { get; set; }
        public PenLineCap StrokeDashCap { get; set; }
        public PenLineCap StrokeEndLineCap { get; set; }
        public PenLineCap StrokeStartLineCap { get; set; }
        public PenLineJoin StrokeLineJoin { get; set; }
        public double Opacity { get; set; }
        #endregion
    }

    public class MeasureDistance : Pan, IDrawStyle
    {
        private PolylineElement _polyline;
        private Point2DCollection _points;
        private bool _isActivated;
        private int _pointsCount = 0;
        private List<TextBlock> _textBlockContainer = new List<TextBlock>();
        private List<double> _distances;

        public MeasureDistance(Map map, ElementsLayer elementsLayer)
            : base(map)
        {
            Name = "MeasureDistance";
            Map = map;
            Stroke = new SolidColorBrush(Colors.Red);
            StrokeThickness = 3;
            Opacity = 1;
            DrawLayer = elementsLayer;
        }

        private void Activate(Point2D firstPoint)
        {
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

            _textBlockContainer = new List<TextBlock>();
            TextBlock textblock = new TextBlock();
            textblock.FontWeight = FontWeights.ExtraBlack;
            textblock.Foreground = new SolidColorBrush(Colors.White);
            textblock.Text = "起点";
            _textBlockContainer.Add(textblock);
            DrawLayer.AddChild(textblock, firstPoint);

            _distances = new List<double>();
            _distances.Add(0);

            _isActivated = true;
        }

        public override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            Point2D point = Map.ScreenToMap(e.GetPosition(Map));

            if (!_isActivated)
            {
                Activate(point);
            }
            else
            {
                DrawPoint(point, false);
            }
        }

        public override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_isActivated && !e.Pointer.IsInContact)
            {
                Point2D point = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
                DrawPoint(point, true);
            }

        }

        public override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            if (_points == null || _points.Count < 2)
            {
                //当点的个数小余两个时，不能结束。
                return;
            }
            Complete();
        }

        public override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            if (e.HoldingState == HoldingState.Started)
            {
                Point2D point = Map.ScreenToMap(e.GetPosition(Map));

                if (_points == null || _points.Count < 1)
                {
                    //当点的个数小余一个时，不能结束。
                    return;
                }
                else
                {
                    DrawPoint(point, false);
                    Complete();
                }
            }
        }

        private void DrawPoint(Point2D point, bool isTemp)
        {
            bool needRemovePoints = _points.Count > _pointsCount;
            while (_points.Count > _pointsCount)
            {
                _points.RemoveAt(_points.Count - 1);
            }
            _points.Add(point);
            if (!isTemp)
            {
                _pointsCount++;
            }
            Point2D lastPoint = _points[_points.Count - 2];
            double distance = Math.Sqrt(Math.Pow(point.X - lastPoint.X, 2) + Math.Pow(point.Y - lastPoint.Y, 2));

            if (needRemovePoints)
            {
                _distances.Remove(_distances.Count - 1);
                distance = _distances[_distances.Count - 1] + distance;
                _distances.Add(distance);
                _textBlockContainer[_textBlockContainer.Count - 1].Text = string.Format("{0:0.0000}千米", distance / 1000);
                DrawLayer.Children.Remove(_textBlockContainer[_textBlockContainer.Count - 1]);
                DrawLayer.AddChild(_textBlockContainer[_textBlockContainer.Count - 1], point);
            }
            else
            {
                distance = _distances[_distances.Count - 1] + distance;
                _distances.Add(distance);
                TextBlock textBlock = new TextBlock();
                textBlock.FontWeight = FontWeights.ExtraBlack;
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                textBlock.Text = string.Format("{0:0.0000}千米", distance / 1000);
                _textBlockContainer.Add(textBlock);

                DrawLayer.AddChild(textBlock, point);
            }
        }

        private void Complete()
        {
            _points = null;
            _isActivated = false;
            _pointsCount = 0;
            _polyline = null;
            _textBlockContainer = null;
            _distances = null;
        }

        public override void Deactivate()
        {
            Complete();
            DrawLayer.Children.Clear();
        }

        public FillRule FillRule { get; set; }
        public ElementsLayer DrawLayer { get; set; }
        #region IDrawStyle 成员
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public double StrokeMiterLimit { get; set; }
        public double StrokeDashOffset { get; set; }
        public DoubleCollection StrokeDashArray { get; set; }
        public PenLineCap StrokeDashCap { get; set; }
        public PenLineCap StrokeEndLineCap { get; set; }
        public PenLineCap StrokeStartLineCap { get; set; }
        public PenLineJoin StrokeLineJoin { get; set; }
        public double Opacity { get; set; }
        #endregion
    }

}
