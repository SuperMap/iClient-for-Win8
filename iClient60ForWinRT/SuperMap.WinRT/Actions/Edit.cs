using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.System;

namespace SuperMap.WinRT.Actions
{
    /// <summary> 
    /// 	<para>${ui_action_Edit_Title}</para>
    /// 	<para>${ui_action_Edit_Description}</para>
    /// </summary>
    public class Edit : MapAction
    {
        private FeaturesLayer editLayer;//传入的被编辑图层
        private Feature activeFeature;//当前被编辑的要素
        private PredefinedLineStyle hoverLineStyle;//虚拟线的风格
        private Feature draggingVertex;//被拖动的顶点,虚拟顶点之一
        private Feature snapVertex;//捕捉点
        private FeaturesLayer hoverLayer;//虚拟图层，包括顶点hoverVertexStyle方形,捕捉点snapStyle圆形虚拟线hoverLine透明
        private Feature hoverCenterFeature;//平移线要素
        private Point2D startPoint = Point2D.Empty;//用于移动整个线-面要素
        private Point2D lastPostion = Point2D.Empty;//用于移动整个线-面要素
        private Point2D startGeoPoint = Point2D.Empty;//用于移动点要素

        /// <summary>${ui_action_Edit_event_geometryEdit_D}</summary>
        public event EventHandler<GeometryEditEventArgs> GeometryEdit;
        /// <summary>${ui_action_Edit_constructor_D}</summary>
        /// <param name="map">${ui_action_Edit_constructor_param_map}</param>
        /// <param name="layer">${ui_action_Edit_constructor_param_layer}</param>
        public Edit(Map map , FeaturesLayer layer)
        {
            if (map != null && layer != null)
            {
                Name = "Edit";
                Map = map;
                editLayer = layer;
                editLayer.PointerPressed += editLayer_PointerPressed;
                editLayer.PointerReleased += editLayer_PointerReleased;
                if (map.Theme == null)
                {
                    SetDefaultStyle();
                }
                else
                {
                    HoverVertexStyle = map.Theme.HoverVertexStyle;
                    SnapStyle = map.Theme.SnapStyle;
                    HoverCenterStyle = map.Theme.HoverCenterStyle;
                    hoverLineStyle = new PredefinedLineStyle() { Stroke = new SolidColorBrush(Colors.Transparent) , StrokeThickness = 10 };//Colors.Transparent
                }
            }
        }

        private void SetDefaultStyle( )
        {
            HoverVertexStyle = new PredefinedMarkerStyle() { Size = 10 , Symbol = PredefinedMarkerStyle.MarkerSymbol.Square , Color = new SolidColorBrush(Colors.Green) };
            SnapStyle = new PredefinedMarkerStyle() { Size = 12 , Symbol = PredefinedMarkerStyle.MarkerSymbol.Circle , Color = new SolidColorBrush(Colors.LightGray) };
            HoverCenterStyle = new PredefinedMarkerStyle() { Size = 20 , Symbol = PredefinedMarkerStyle.MarkerSymbol.Star , Color = new SolidColorBrush(Colors.Red) };
            hoverLineStyle = new PredefinedLineStyle() { Stroke = new SolidColorBrush(Colors.Transparent) , StrokeThickness = 10 };//Colors.Transparent
        }

        private void editLayer_PointerPressed(object sender, FeaturePointerRoutedEventArgs args)
        {
            Point2D startGeoRegion = Map.ScreenToMap(args.GetCurrentPoint(Map).Position);
            args.Handler = true;
            Map.Focus(Windows.UI.Xaml.FocusState.Pointer);
            startEdit(args.Feature , false , startGeoRegion);
        }
        private void editLayer_PointerReleased(object sender, FeaturePointerRoutedEventArgs args)
        {
            if (activeFeature != null && !startPoint.IsEmpty)
            {
                OnGeometryEdit(activeFeature , GeometryEditAction.Moved);
            }//面要素移动
            stopTracking();
            args.Handler = true;
        }

        private void startEdit(Feature feature , bool suppressEvent)
        {
            startEdit(feature , suppressEvent , Point2D.Empty);
        }
        private void startEdit(Feature feature , bool suppressEvent , Point2D start)
        {
            if (activeFeature != feature)
            {
                StopEdit();
                activeFeature = feature;
                buildHoverLayer(activeFeature);
                if (!suppressEvent)
                {
                    OnGeometryEdit(activeFeature , GeometryEditAction.EditStarted);
                }
            }
            else if (activeFeature.Geometry is GeoRegion && !start.IsEmpty)
            {
                prepareMoveLineOrRegionFeature(start);
            }//表明移动整个要素
        }

        private void buildHoverLayer(Feature feature)
        {
            hoverLayer = new FeaturesLayer();
            if (feature.Geometry is GeoPoint)
            {
                buildByGeoPoint(feature);
            }
            else if (feature.Geometry is GeoLine)
            {
                buildByGeoLine(feature);
            }
            else if (feature.Geometry is GeoRegion)
            {
                buildByGeoRegion(feature);
            }
            Map.Layers.Add(hoverLayer);
            hoverLayer.PointerPressed += virtualLayer_PointerPressed;
            hoverLayer.PointerMoved += virtualLayer_PointerMoved;
            hoverLayer.PointerReleased += virtualLayer_PointerReleasedp;
            hoverLayer.PointerExited += virtualLayer_PointerExited;
        }
        private void buildByGeoLine(Feature feature)
        {
            GeoLine line = feature.Geometry as GeoLine;
            if (line.Parts.Count > 0)
            {
                addCenterFeature(feature);
                Feature first = null;
                //遍历每个part
                for (int k = 0 ; k < line.Parts.Count ; k++)
                {
                    //显示每个part
                    Point2DCollection points = line.Parts[k];
                    for (int i = 0 ; i < points.Count - 1 ; i++)
                    {
                        Feature f = addHoverVertex(feature , new GeoPoint(points[i]) , i , k);
                        if (i == 0)
                        {
                            first = f;
                        }
                        Point2D p0 = points[i];
                        Point2D p1 = points[i + 1];
                        //TODO:
                        addHoverLineSegment(p0 , p1 , i , i + 1 , feature , points , k);
                        f.Attributes.Add("Point2DCollection" , points);
                    }
                    Feature f2 = addHoverVertex(feature , new GeoPoint(points[points.Count - 1]) , points.Count - 1 , k);
                    f2.Attributes.Add("Point2DCollection" , points);
                }
            }
        }
        private void buildByGeoRegion(Feature feature)
        {
            GeoRegion region = feature.Geometry as GeoRegion;

            if (region.Parts.Count > 0)
            {
                for (int k = 0 ; k < region.Parts.Count ; k++)
                {
                    Point2DCollection points = region.Parts[k];
                    Feature first = null;
                    for (int i = 0 ; i < points.Count - 1 ; i++)
                    {
                        Feature f = addHoverVertex(feature , new GeoPoint(points[i]) , i , k);
                        if (i == 0)
                        {
                            first = f;
                        }
                        Point2D p0 = points[i];
                        Point2D p1 = points[i + 1];
                        if (( ( feature.Geometry as GeoRegion ).Parts[k].Count - 1 ) == ( i + 1 ))
                        {
                            addHoverLineSegment(p0 , p1 , i , 0 , feature , points , k);
                        }
                        else
                        {
                            addHoverLineSegment(p0 , p1 , i , i + 1 , feature , points , k);
                        }
                        f.Attributes.Add("Point2DCollection" , points);
                    }

                    if (points[0] == points[points.Count - 1])
                    {
                        first.Attributes.Add("LastPoint" , points[points.Count - 1]);
                    }
                }
            }

        }
        private void buildByGeoPoint(Feature feature)
        {
            draggingVertex = addHoverVertex(feature , feature.Geometry as GeoPoint , -1 , 0);
        }

        //当线对象时，显示bounds的中心点；
        private void addCenterFeature(Feature feature)
        {
            GeoPoint center = new GeoPoint(feature.Geometry.Bounds.Center.X , feature.Geometry.Bounds.Center.Y);
            hoverCenterFeature = new Feature { Geometry = center , Style = HoverCenterStyle };
            hoverCenterFeature.SetZIndex(3);
            hoverLayer.AddFeature(hoverCenterFeature);
        }
        //添加虚拟线；
        private void addHoverLineSegment(Point2D p0 , Point2D p1 , int index0 , int index1 , Feature feature , Point2DCollection points , int partIndex)
        {
            GeoLine line = new GeoLine();
            line.Parts.Add(new Point2DCollection() { p0 , p1 });
            Feature segment = new Feature() { Geometry = line , Style = hoverLineStyle };
            segment.SetZIndex(1);

            segment.Attributes.Add("Point2DCollection" , points);
            segment.Attributes.Add("Feature" , feature);
            segment.Attributes.Add("Index0" , index0);
            segment.Attributes.Add("Index1" , index1);
            segment.Attributes.Add("PartIndex" , partIndex);

            hoverLayer.Features.Add(segment);
        }
        //添加虚拟顶点；
        private Feature addHoverVertex(Feature feature , GeoPoint p , int index , int partIndex)
        {
            Feature hoverVertex = new Feature() { Geometry = p , Style = HoverVertexStyle };
            hoverVertex.SetZIndex(2);
            hoverLayer.Features.Add(hoverVertex);
            hoverVertex.Attributes.Add("Feature" , feature);
            hoverVertex.Attributes.Add("Index" , index);
            hoverVertex.Attributes.Add("PartIndex" , partIndex);
            hoverVertex.AddOnDoubleTapped((s , e) => { deleteOneVertex(s as Feature); });//双击删除某个顶点，线和面。
            return hoverVertex;
        }
        //重新构建整个虚拟图层，比如添加或删除一个顶点时
        private void rebuildHoverLayer(Feature feature)
        {
            stopEdit(true);
            startEdit(feature , true);
        }

        private void virtualLayer_PointerExited(object sender, FeaturePointerRoutedEventArgs args)
        {
            if (snapVertex != null)
            {
                hoverLayer.Features.Remove(snapVertex);
                snapVertex = null;
            }
        }
        private void virtualLayer_PointerMoved(object sender, FeaturePointerRoutedEventArgs args)
        {
            Feature hoverFeature = args.Feature;
            //在虚拟线上，并且没遇到虚拟顶点 ，显示捕捉点
            if (draggingVertex == null && hoverFeature.Geometry is GeoLine)
            {
                GeoLine line = hoverFeature.Geometry as GeoLine;
                Point2D pMap = Map.ScreenToMap(args.GetCurrentPoint(Map).Position);
                Point2D snap = FindPointOnLineClosestToPoint(line.Parts[0][0] , line.Parts[0][1] , pMap);
                if (snapVertex == null)
                {
                    snapVertex = new Feature() { Style = SnapStyle , Geometry = new GeoPoint(snap) };
                    hoverLayer.Features.Add(snapVertex);
                }
                else
                {
                    snapVertex.Geometry = new GeoPoint(snap);
                }
            }
        }
        private void virtualLayer_PointerPressed(object sender, FeaturePointerRoutedEventArgs args)
        {
            args.Handler = true;
            Feature hoverFeature = args.Feature;
            if (hoverFeature == hoverCenterFeature)
            {
                prepareMoveLineOrRegionFeature(( hoverCenterFeature.Geometry as GeoPoint ).Location);
            }
            else
                if (hoverFeature.Geometry is GeoPoint)
                {
                    prepareMovePointFeature(hoverFeature);
                }
                else if (hoverFeature.Geometry is GeoLine)
                {
                    Point2D pMap = Map.ScreenToMap(args.GetCurrentPoint(Map).Position);
                    addOneVertex(hoverFeature , pMap);
                }
        }
        private void virtualLayer_PointerReleasedp(object sender, FeaturePointerRoutedEventArgs args)
        {
            if (activeFeature != null && hoverCenterFeature != null && !startPoint.IsEmpty
                && startPoint != ( hoverCenterFeature.Geometry as GeoPoint ).Location)
            {
                OnGeometryEdit(activeFeature , GeometryEditAction.Moved);
            }//线要素移动
            else if (activeFeature != null && draggingVertex != null && startGeoPoint.IsEmpty)
            {
                OnGeometryEdit(activeFeature , GeometryEditAction.Moved);
            }//点要素移动
            else if (activeFeature != null && draggingVertex != null && !startGeoPoint.IsEmpty
                && ( startGeoPoint != ( draggingVertex.Geometry as GeoPoint ).Location ))
            {
                OnGeometryEdit(activeFeature , GeometryEditAction.VertexMoved);
                changeCenterFeatureLocation();
            }//线面顶点移动
            stopTracking();
            args.Handler = true;
        }

        private void addOneVertex(Feature hoverFeature , Point2D pMap)
        {
            GeoLine line = hoverFeature.Geometry as GeoLine;
            Point2DCollection pnts = hoverFeature.Attributes["Point2DCollection"] as Point2DCollection;
            Feature parent = hoverFeature.Attributes["Feature"] as Feature;
            int _partIndex = (int)hoverFeature.Attributes["PartIndex"];
            if (snapVertex != null)
            {
                hoverLayer.Features.Remove(snapVertex);
            }

            Point2D snapPt = FindPointOnLineClosestToPoint(line.Parts[0][0] , line.Parts[0][1] , pMap);
            int index = pnts.IndexOf(line.Parts[0][0]);
            pnts.Insert(index + 1 , snapPt);
            hoverLayer.Features.Remove(hoverFeature);

            rebuildHoverLayer(parent);
            OnGeometryEdit(hoverFeature , GeometryEditAction.VertexAdded);
        }

        private void prepareMovePointFeature(Feature hoverFeature)
        {
            draggingVertex = hoverFeature;
            draggingVertex.Select();
            startTracking();
            startGeoPoint = ( hoverFeature.Geometry as GeoPoint ).Location;
        }
        private void prepareMoveLineOrRegionFeature(Point2D start)
        {
            if (hoverCenterFeature != null)
            {
                hoverCenterFeature.Select();
            }
            startPoint = start;
            lastPostion = start;
        }

        //可以停止对某个要素的编辑，但Map的Action状态仍然为Edit
        /// <summary>${ui_action_Edit_method_stopEdit_D}</summary>
        public void StopEdit( )
        {
            stopEdit(false);
        }

        private void stopEdit(bool suppressEvent)
        {
            if (hoverLayer != null)
            {
                hoverLayer.ClearFeatures();
                Map.Layers.Remove(hoverLayer);
                hoverLayer.PointerPressed -= virtualLayer_PointerPressed;
                hoverLayer.PointerMoved -= virtualLayer_PointerMoved;
                hoverLayer.PointerReleased -= virtualLayer_PointerReleasedp;
                hoverLayer.PointerExited -= virtualLayer_PointerExited;
                hoverLayer = null;
            }
            stopTracking();
            if (!suppressEvent && activeFeature != null)
            {
                OnGeometryEdit(activeFeature , GeometryEditAction.EditCompleted);
            }
            if (activeFeature != null)
            {
                activeFeature = null;
            }
        }

        private void startTracking( )
        {
            Map.Focus(FocusState.Pointer);
        }
        private void stopTracking( )
        {
            startGeoPoint = Point2D.Empty;
            startPoint = Point2D.Empty;
            lastPostion = Point2D.Empty;

            if (draggingVertex != null)
            {
                draggingVertex.UnSelect();
                draggingVertex = null;
                if (activeFeature != null && activeFeature.Geometry is GeoPoint)
                {
                    StopEdit();
                }
            }
        }

        /// <summary>${ui_action_MapAction_event_onMouseMove_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseMove_param_e}</param>
        public override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);

            Point2D newPnt = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
            if (!startPoint.IsEmpty && ( activeFeature.Geometry is GeoLine || activeFeature.Geometry is GeoRegion ))
            {
                moveLineOrRegionFeature(newPnt);
            }//(!startPoint.IsEmpty)表明了在整体移动线或面要素
            else if (draggingVertex != null && draggingVertex.Geometry != null && draggingVertex.Geometry is GeoPoint)
            {
                ( draggingVertex.Geometry as GeoPoint ).Location = newPnt;//改变虚拟顶点的位置
                changeActiveFeatureVertex(newPnt);//改变要素顶点的位置
                changeLineSegment(newPnt);//改变相邻两条虚拟线的位置
            }//拖动某个线或面的顶点
        }

        private void changeCenterFeatureLocation( )
        {
            if (activeFeature != null && hoverCenterFeature != null)
            {
                ( hoverCenterFeature.Geometry as GeoPoint ).Location = activeFeature.Geometry.Bounds.Center;
            }
        }

        private void changeLineSegment(Point2D newPnt)
        {
            int index = (int)draggingVertex.Attributes["Index"];
            int _partIndex = (int)draggingVertex.Attributes["PartIndex"];
            foreach (Feature item in hoverLayer.Features)
            {
                if (item.Attributes.ContainsKey("PartIndex") && (int)item.Attributes["PartIndex"] == _partIndex)
                {
                    if (item.Geometry is GeoLine)
                    {
                        int index0 = (int)item.Attributes["Index0"];
                        int index1 = (int)item.Attributes["Index1"];

                        if (index0 == index)
                        {
                            ( ( item.Geometry ) as GeoLine ).Parts[0][0] = newPnt;
                        }
                        else if (index1 == index)
                        {
                            ( ( item.Geometry ) as GeoLine ).Parts[0][1] = newPnt;
                        }
                    }
                }
            }
        }

        private void changeActiveFeatureVertex(Point2D newPnt)
        {
            if (activeFeature.Geometry is GeoRegion)
            {
                int index = (int)draggingVertex.Attributes["Index"];
                int _partIndex = (int)draggingVertex.Attributes["PartIndex"];
                GeoRegion region = activeFeature.Geometry as GeoRegion;

                region.Parts[_partIndex][index] = newPnt;
                if (draggingVertex.Attributes.ContainsKey("LastPoint"))
                {
                    int lastIndex = region.Parts[_partIndex].Count - 1;
                    region.Parts[_partIndex][lastIndex] = newPnt;
                }
            }
            else if (activeFeature.Geometry is GeoLine)
            {
                int index = (int)draggingVertex.Attributes["Index"];
                int _partIndex = (int)draggingVertex.Attributes["PartIndex"];
                GeoLine line = activeFeature.Geometry as GeoLine;

                line.Parts[_partIndex][index] = newPnt;
            }
        }

        private void moveLineOrRegionFeature(Point2D newPnt)
        {
            double deltaX = newPnt.X - lastPostion.X;
            double deltaY = newPnt.Y - lastPostion.Y;
            if (hoverCenterFeature != null)
            {
                ( hoverCenterFeature.Geometry as GeoPoint ).Location = newPnt;
            }//是线要素的话，移动五角星
            activeFeature.Geometry.Offset(deltaX , deltaY);

            lastPostion = newPnt;

            //同时要移动VirtualLayer上的元素
            moveFeaturesOnVirtualLayer(deltaX , deltaY);
        }

        private void moveFeaturesOnVirtualLayer(double deltaX , double deltaY)
        {
            foreach (Feature item in hoverLayer.Features)
            {
                item.Geometry.Offset(deltaX , deltaY);
            }
        }

        private void deleteOneVertex(Feature hoverVertex)
        {
            Feature parent = hoverVertex.Attributes["Feature"] as Feature;
            Point2DCollection points = hoverVertex.Attributes["Point2DCollection"] as Point2DCollection;
            int index = (int)hoverVertex.Attributes["Index"];
            if (parent.Geometry is GeoRegion && points.Count < 5)
            {
                return;
            }
            if (parent.Geometry is GeoLine && points.Count < 3)
            {
                return;
            }
            if (parent.Geometry is GeoRegion && index == 0)
            {
                points.RemoveAt(points.Count - 1);
                points.Add(points[1]);
            }//特殊处理下第1个/最后1个点删除
            points.RemoveAt(index);

            rebuildHoverLayer(parent);
            OnGeometryEdit(hoverVertex , GeometryEditAction.VertexRemoved);
        }

        private static Point2D FindPointOnLineClosestToPoint(Point2D p0 , Point2D p1 , Point2D p)
        {
            Point2D p0p = new Point2D(p.X - p0.X , p.Y - p0.Y);
            Point2D p1p = new Point2D(p1.X - p0.X , p1.Y - p0.Y);
            double p0p1sq = p1p.X * p1p.X + p1p.Y * p1p.Y;
            double p0p_p0p1 = p0p.X * p1p.X + p0p.Y * p1p.Y;
            double t = p0p_p0p1 / p0p1sq;
            if (t < 0.0)
            {
                t = 0.0;
            }
            else if (t > 1.0)
            {
                t = 1.0;
            }
            return new Point2D(p0.X + p1p.X * t , p0.Y + p1p.Y * t);
        }

        /// <summary>${ui_action_MapAction_event_onDblClick_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onDblClick_param_e}</param>
        public override void OnDoubleTapped(Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            StopEdit();
            e.Handled = true;
            base.OnDoubleTapped(e);
        }
        
        /// <summary>${ui_action_MapAction_event_onKeyDown_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onKeyDown_param_e}</param>
        public override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete && draggingVertex != null && activeFeature != null)
            {
                if (activeFeature.Geometry is GeoPoint)
                {
                    this.editLayer.Features.Remove(activeFeature);
                    stopTracking();
                }
                else
                {
                    deleteOneVertex(draggingVertex);
                }
            }
            if (e.Key == VirtualKey.Escape)
            {
                StopEdit();
            }
            base.OnKeyDown(e);
        }
        //调用该函数，Map的Action将被置为空
        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate( )
        {
            StopEdit();
            editLayer.PointerPressed -= editLayer_PointerPressed;
            editLayer.PointerReleased -= editLayer_PointerReleased;
        }
        private void OnGeometryEdit(Feature f , GeometryEditAction action)
        {
            if (GeometryEdit != null)
            {
                GeometryEdit(this , new GeometryEditEventArgs(f , action));
            }
        }
        /// <summary>${ui_action_Edit_attribute_HoverVertexStyle_D}</summary>
        public MarkerStyle HoverVertexStyle { get; set; }
        /// <summary>${ui_action_Edit_attribute_HoverCenterStyle_D}</summary>
        public MarkerStyle HoverCenterStyle { get; set; }
        /// <summary>${ui_action_Edit_attribute_SnapStyle_D}</summary>
        public MarkerStyle SnapStyle { get; set; }

        /// <summary>${ui_action_GeometryEditAction_Title}</summary>
        public enum GeometryEditAction
        {
            /// <summary>${ui_action_GeometryEditAction_attribute_Moved_D}</summary>
            Moved ,
            /// <summary>${ui_action_GeometryEditAction_attribute_vertexAdded_D}</summary>
            VertexAdded ,
            /// <summary>${ui_action_GeometryEditAction_attribute_vertexRemoved_D}</summary>
            VertexRemoved ,
            /// <summary>${ui_action_GeometryEditAction_attribute_vertexMoved_D}</summary>
            VertexMoved ,
            /// <summary>${ui_action_GeometryEditAction_attribute_editCompleted_D}</summary>
            EditCompleted ,
            /// <summary>${ui_action_GeometryEditAction_attribute_editStarted_D}</summary>
            EditStarted
        }
        /// <summary>
        /// 	<para>${ui_action_GeometryEditEventArgs_Title}</para>
        /// 	<para>${ui_action_GeometryEditEventArgs_Description}</para>
        /// </summary>
        public sealed class GeometryEditEventArgs : EventArgs
        {

            internal GeometryEditEventArgs(Feature feature , GeometryEditAction action)
            {
                Feature = feature;
                Action = action;
            }

            /// <summary>${ui_action_GeometryEditEventArgs_attribute_feature_D}</summary>
            public Feature Feature { get; private set; }
            /// <summary>${ui_action_GeometryEditEventArgs_attribute_geometryEditAction_D}</summary>
            public GeometryEditAction Action { get; private set; }
        }
    }
    internal static class MouseExtensions
    {
        private const int doubleClickInterval = 222;
        private static readonly DependencyProperty DoubleTappedTimerProperty = DependencyProperty.RegisterAttached("DoubleTappedTimer", typeof(DispatcherTimer), typeof(UIElement), null);
        private static readonly DependencyProperty DoubleTappedHandlersProperty = DependencyProperty.RegisterAttached("DoubleTappedHandlers", typeof(List<PointerEventHandler>), typeof(UIElement), null);
        private static readonly DependencyProperty DoubleTappedPositionProperty = DependencyProperty.RegisterAttached("DoubleTappedPosition", typeof(Point), typeof(UIElement), null);

        public static void AddOnDoubleTapped(this Feature element, PointerEventHandler handler)
        {
            element.PointerPressed += element_PointerPressed;
            List<PointerEventHandler> handlers = element.GetValue(DoubleTappedHandlersProperty) as List<PointerEventHandler>;
            if (handlers == null)
            {
                handlers = new List<PointerEventHandler>();
                element.SetValue(DoubleTappedHandlersProperty , handlers);
            }
            handlers.Add(handler);
        }

        //没有用到，先注释。
        //public static void RemoveDoubleClick(this Feature element, MouseButtonEventHandler handler)
        //{
        //    element.MouseLeftButtonDown -= element_MouseLeftButtonDown;
        //    List<MouseButtonEventHandler> handlers = element.GetValue(DoubleClickHandlersProperty) as List<MouseButtonEventHandler>;
        //    if (handlers != null)
        //    {
        //        handlers.Remove(handler);
        //        if (handlers.Count == 0)
        //            element.ClearValue(DoubleClickHandlersProperty);
        //    }
        //}
        private static void element_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Feature element = sender as Feature;
            Point position = e.GetCurrentPoint(element.Layer.Map).Position;
            DispatcherTimer timer = element.GetValue(DoubleTappedTimerProperty) as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                Point oldPosition = (Point)element.GetValue(DoubleTappedPositionProperty);
                element.ClearValue(DoubleTappedTimerProperty);
                element.ClearValue(DoubleTappedPositionProperty);
                if (Math.Abs(oldPosition.X - position.X) < 1 && Math.Abs(oldPosition.Y - position.Y) < 1)
                {
                    List<PointerEventHandler> handlers = element.GetValue(DoubleTappedHandlersProperty) as List<PointerEventHandler>;
                    if (handlers != null)
                    {
                        foreach (PointerEventHandler handler in handlers)
                        {
                            handler(sender , e);
                        }
                    }
                    return;
                }
            }
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(doubleClickInterval) };
            timer.Tick += new EventHandler<object>((s , args) =>
            {
                ( s as DispatcherTimer ).Stop();
                element.ClearValue(DoubleTappedTimerProperty);
                element.ClearValue(DoubleTappedPositionProperty);
            });
            element.SetValue(DoubleTappedTimerProperty, timer);
            element.SetValue(DoubleTappedPositionProperty, position);
            timer.Start();
        }
    }
}
