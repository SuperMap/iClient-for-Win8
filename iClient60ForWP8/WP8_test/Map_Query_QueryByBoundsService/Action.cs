
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
using System.Windows.Shapes;

namespace Map_Query_QueryByBoundsService
{
    public abstract class Action
    {

        public virtual void Hold(GestureEventArgs e)
        {
        }
        public virtual void DoubleTap(GestureEventArgs e) { }
        public virtual void Tap(GestureEventArgs e) { }
        public virtual void OnHold(GestureEventArgs e) { }
        public virtual void OnManipulationStarted(ManipulationStartedEventArgs e) { }
        public virtual void OnManipulationDelta(ManipulationDeltaEventArgs e) { }
        public virtual void OnManipulationCompleted(ManipulationCompletedEventArgs e) { }
        public virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e) { }
        public virtual void OnMouseMove(MouseEventArgs e) { }
        public virtual void OnMouseEnter(MouseEventArgs e) { }
        public virtual void OnMouseLeave(MouseEventArgs e) { }
        public virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e) { }
        public abstract void Deactivate();
        public string Name { get; set; }
       


    }
    public class DrawRectangle : Action 
    {
        private Point2D startPt;
        private Rectangle rectangle;
        private bool isActivated;
        private bool isDrawing;
        Map map=new Map();
       
        public event EventHandler<DrawEventArgs> DrawCompleted;
      
        public DrawRectangle( )
        {
        }

   
        public DrawRectangle(Map map,ElementsLayer layer )
        {
           
        }

        private void Activate(Point2D item)
        {
            if (this.map == null || this.map.Layers == null)
            {
                return;
            }
            DrawLayer = new ElementsLayer();
            this.map.Layers.Add(DrawLayer);
            rectangle = new Rectangle();
            

            rectangle.SetValue(ElementsLayer.BBoxProperty , new Rectangle2D(item , item));
            DrawLayer.Children.Add(rectangle);

            isActivated = true;
            isDrawing = true;
        }

       
        public override void Deactivate( )
        {
            isActivated = false;
            isDrawing = false;
            rectangle = null;
            if (DrawLayer != null)
            {
                DrawLayer.Children.Clear();
            }
            if (this.map != null && this.map.Layers != null)
            {
                this.map.Layers.Remove(DrawLayer);
            }
        }

       
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            startPt = this.map.ScreenToMap(e.GetPosition(this.map));

           
                Activate(startPt);
            
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

       
        public override void OnMouseMove(MouseEventArgs e)
        {
            if (isDrawing)
            {


                Point2D item = this.map.ScreenToMap(e.GetPosition(this.map));

                double maxX = Math.Max(startPt.X , item.X);
                double minX = Math.Min(startPt.X , item.X);
                double maxY = Math.Max(startPt.Y , item.Y);
                double minY = Math.Min(startPt.Y , item.Y);

                Rectangle2D bounds = new Rectangle2D(minX , minY , maxX , maxY);
                rectangle.SetValue(ElementsLayer.BBoxProperty , bounds);
            }

            base.OnMouseMove(e);
        }

        
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

            Rectangle2D bounds = this.map.Bounds;
                if (bounds.IsEmpty || bounds.Width == 0.0 || bounds.Height == 0.0)
                {
                    return;
                }
                GeoRegion geoRegion = new GeoRegion();//构造返回的Geometry
                Point2DCollection geoPoints = new Point2DCollection();
                geoPoints.Add(bounds.BottomLeft);
                geoPoints.Add(new Point2D(bounds.Right, bounds.Bottom));
                geoPoints.Add(bounds.TopRight);
                geoPoints.Add(new Point2D(bounds.Left, bounds.Top));
                geoPoints.Add(bounds.BottomLeft);//需要添加起始点做为最后一个点
                geoRegion.Parts.Add(geoPoints);


                DrawEventArgs args2 = new DrawEventArgs
                {
                    Geometry = geoRegion
                };

                Deactivate();
                OnDrawCompleted(args2);
            
        }
        internal ElementsLayer DrawLayer { get; private set; }
       
        protected Rectangle Rectangle
        {
            get
            {
                return this.rectangle;
            }
        }

      
       
    }
    public class DrawPoint : Action
    {
        ElementsLayer _eLayer;
        Map _map;
        List<Pushpin> _pushpins;
        public event EventHandler<DrawEventArgs> DrawCompleted;

        public DrawPoint(Map map, ElementsLayer layer)
        {
            _map = map;
            _eLayer = layer;
            _pushpins = new List<Pushpin>();
        }

        public override void Tap(GestureEventArgs e)
        {
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            DrawEventArgs args = new DrawEventArgs
            {
                Geometry =new GeoPoint(point.X,point.Y)
            };
            OnDrawComplete(args);
            base.Tap(e);
        }
      

        private void OnDrawComplete(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this, args);
            }
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
        List<TextBlock> _distanceList;
        List<double> _distances;
        Point2DCollection _points;
        Feature _line;

        public MeasureDistance(Map map, ElementsLayer elementsLayer, FeaturesLayer featuresLayer)
        {
            _map = map;
            _eLayer = elementsLayer;
            _fLayer = featuresLayer;
            _isInited = false;
            _distanceList = new List<TextBlock>();
            _points = new Point2DCollection();
            _distances = new List<double>();
        }

        private void Init(Point2D firstPoint)
        {
            if (_isInited)
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
            _distanceList.Add(first);
            first.FontWeight = FontWeights.ExtraBlack;
            first.Foreground = new SolidColorBrush(Colors.Black);
            first.Text = "起点";
            _eLayer.AddChild(first, firstPoint);
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
                _eLayer.AddChild(text, point);
                Point2D lastPoint = _points[_points.Count - 1];
                double nowDistance = Math.Sqrt((point.X - lastPoint.X) * (point.X - lastPoint.X) + (point.Y - lastPoint.Y) * (point.Y - lastPoint.Y));
                double lastDistance = _distances[_distances.Count - 1];
                double distance = nowDistance + lastDistance;
                text.Text = string.Format("{0}公里", distance.ToString("0.0"));
                _distances.Add(distance);
                _distanceList.Add(text);
                _points.Add(point);
            }
        }

        public override void DoubleTap(GestureEventArgs e)
        {
            base.DoubleTap(e);
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            EndDistance(point);
        }

        public override void Hold(GestureEventArgs e)
        {
            base.Hold(e);
            Point2D point = _map.ScreenToMap(e.GetPosition(_map));
            EndDistance(point);
        }

        private void EndDistance(Point2D endPoint)
        {
            if (_isInited && _points.Count > 0)
            {
                TextBlock text = new TextBlock();
                _distanceList.Add(text);
                text.FontWeight = FontWeights.ExtraBlack;
                text.Foreground = new SolidColorBrush(Colors.Orange);
                _eLayer.AddChild(text, endPoint);
                Point2D lastPoint = _points[_points.Count - 1];
                double nowDistance = Math.Sqrt((endPoint.X - lastPoint.X) * (endPoint.X - lastPoint.X) + (endPoint.Y - lastPoint.Y) * (endPoint.Y - lastPoint.Y));
                double lastDistance = 0;
                double.TryParse(_distanceList[_distanceList.Count - 1].Text, out lastDistance);
                double distance = nowDistance + lastDistance;
                text.Text = string.Format("总长度：{0}公里", distance.ToString("0.0"));
                _points.Add(endPoint);
                _isInited = false;
            }
        }

        public override void Deactivate()
        {
            if (_distanceList != null && _distanceList.Count > 0)
            {
                foreach (TextBlock text in _distanceList)
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
