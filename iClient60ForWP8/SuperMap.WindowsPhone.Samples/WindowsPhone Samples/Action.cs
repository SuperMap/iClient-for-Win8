using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Samples
{
    public abstract class Action
    {
        public virtual void Hold(GestureEventArgs e)
        {

        }

        public virtual void DoubleTap(GestureEventArgs e)
        {

        }

        public virtual void Tap(GestureEventArgs e)
        {

        }

        public abstract void Deactivate();
    }

    public class DrawPoint : Action
    {
        ElementsLayer _eLayer;
        Map _map;
        List<Pushpin> _pushpins;

        public DrawPoint(Map map, ElementsLayer layer)
        {
            _map = map;
            _eLayer = layer;
            _pushpins = new List<Pushpin>();
        }

        public override void Tap(GestureEventArgs e)
        {
            base.Tap(e);
            Pushpin pushpin = new Pushpin();
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            pushpin.Location = point;
            _eLayer.AddChild(pushpin);
            _pushpins.Add(pushpin);
        }

        public override void Deactivate()
        {
            foreach (Pushpin pushpin in _pushpins)
            {
                if (_eLayer.Children.Contains(pushpin))
                {
                    _eLayer.Children.Remove(pushpin);
                }
            }
            _pushpins.Clear();
        }
    }

    public class MeasureDistance : Action
    {
        Map _map;
        ElementsLayer _eLayer;
        FeaturesLayer _fLayer;
        bool _isInited;
        List<Border> _distanceList;
        List<double> _distances;
        Point2DCollection _points;
        Feature _line;
        private bool _isMeasure;

        public MeasureDistance(Map map, ElementsLayer elementsLayer, FeaturesLayer featuresLayer, bool isMeasure)
        {
            _map = map;
            _eLayer = elementsLayer;
            _fLayer = featuresLayer;
            _isInited = false;
            _distanceList = new List<Border>();
            _points = new Point2DCollection();
            _distances = new List<double>();
            _isMeasure = isMeasure;
        }


        private void Init(Point2D firstPoint)
        {
            if (_isInited)
            {
                return;
            }
            if (!_isMeasure)
            {
                return;
            }
            Deactivate();
            _line = new Feature();
            GeoLine line = new GeoLine();
            line.Parts = new System.Collections.ObjectModel.ObservableCollection<Point2DCollection>();
            line.Parts.Add(_points);
            PredefinedLineStyle style = new PredefinedLineStyle();
            style.Stroke = new SolidColorBrush(Colors.Red);
            style.StrokeThickness = 2;
            style.Symbol = PredefinedLineStyle.LineSymbol.Solid;
            _line.Geometry = line;
            _line.Style = style;
            _fLayer.AddFeature(_line);
            TextBlock first = new TextBlock();
            Border borderText = new Border();
            borderText.BorderBrush = new SolidColorBrush(Colors.LightGray);
            borderText.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            borderText.Child = first;
            _distanceList.Add(borderText);
            first.FontWeight = FontWeights.ExtraBlack;
            first.Foreground = new SolidColorBrush(Colors.Black);
            first.Text = "起点";
            _eLayer.AddChild(borderText, firstPoint);
            _points.Add(firstPoint);
            _distances.Add(0);
            _isInited = true;
        }

        public override void Tap(GestureEventArgs e)
        {
            base.Tap(e);
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            if (!_isInited)
            {
                Init(point);
            }
            else
            {
                TextBlock text = new TextBlock();
                text.FontWeight = FontWeights.ExtraBlack;
                text.Foreground = new SolidColorBrush(Colors.Black);
                text.Opacity = 1;
                Border borderText = new Border();
                borderText.BorderBrush = new SolidColorBrush(Colors.LightGray);
                borderText.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                borderText.Child = text;
                _eLayer.AddChild(borderText, point);
                Point2D lastPoint = _points[_points.Count - 1];
                double nowDistance = Math.Sqrt((point.X - lastPoint.X) * (point.X - lastPoint.X) + (point.Y - lastPoint.Y) * (point.Y - lastPoint.Y));
                double lastDistance = _distances[_distances.Count - 1];
                double distance = nowDistance / 1000 + lastDistance;
                text.Text = string.Format("{0}公里", distance.ToString("0.0"));
                _distances.Add(distance);
                _distanceList.Add(borderText);
                _points.Add(point);
            }
        }

        public override void DoubleTap(GestureEventArgs e)
        {
            base.DoubleTap(e);
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            EndDistance(point, true);
        }

        public override void Hold(GestureEventArgs e)
        {
            base.Hold(e);
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            EndDistance(point, false);
        }

        private void EndDistance(Point2D endPoint, bool removeLast)
        {
            if (_isInited && _points.Count > 0)
            {
                if (removeLast)
                {
                    _eLayer.Children.RemoveAt(_points.Count - 1);
                }
                TextBlock text = new TextBlock();
                text.FontWeight = FontWeights.ExtraBlack;
                text.Foreground = new SolidColorBrush(Colors.Orange);
                Border borderText = new Border();
                borderText.BorderBrush = new SolidColorBrush(Colors.LightGray);
                borderText.Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
                borderText.Child = text;
                _eLayer.AddChild(borderText, endPoint);
                _distanceList.Add(borderText);
                Point2D lastPoint = _points[_points.Count - 1];
                double nowDistance = Math.Sqrt((endPoint.X - lastPoint.X) * (endPoint.X - lastPoint.X) + (endPoint.Y - lastPoint.Y) * (endPoint.Y - lastPoint.Y));
                double lastDistance = 0;
                lastDistance = _distances[_distances.Count - 1];
                double distance = nowDistance / 1000 + lastDistance;
                text.Text = string.Format("总长度：{0}公里", distance.ToString("0.0"));
                _points.Add(endPoint);
                _isInited = false;
                _isMeasure = false;
            }
        }

        public override void Deactivate()
        {
            if (_distanceList != null && _distanceList.Count > 0)
            {
                foreach (Border text in _distanceList)
                {
                    if (_eLayer.Children.Contains(text))
                    {
                        _eLayer.Children.Remove(text);
                    }
                }
                _distanceList.Clear();
            }
            if (_line != null)
            {
                if (_fLayer.Features.Contains(_line))
                {
                    _fLayer.Features.Remove(_line);
                }
                _line = null;
            }
            if (_points != null && _points.Count > 0)
            {
                _points.Clear();
            }
            _distances.Clear();
        }
    }
}
