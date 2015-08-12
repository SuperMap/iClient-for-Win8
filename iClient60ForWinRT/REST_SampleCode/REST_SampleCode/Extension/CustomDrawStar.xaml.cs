using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace REST_SampleCode
{
    public partial class CustomDrawStar : Page
    {
        private ElementsLayer elementslayer;
        TiledDynamicRESTLayer _layer;

        public CustomDrawStar()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            elementslayer = MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += CustomDrawStar_Failed;
            this.Unloaded += CustomDrawStar_Unloaded;
        }

        void CustomDrawStar_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= CustomDrawStar_Unloaded;
            _layer.Failed -= CustomDrawStar_Failed;
            MyMap.Dispose();
        }

        async void CustomDrawStar_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void pan_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action = new Pan(MyMap);
        }

        private void Mybtn_Click(object sender, RoutedEventArgs e)
        {
            DrawStar star = new DrawStar(MyMap);
            MyMap.Action = star;
            star.DrawCompleted += new System.EventHandler<DrawEventArgs>(star_DrawCompleted);
        }

        private void star_DrawCompleted(object sender, DrawEventArgs e)
        {
            elementslayer.AddChild(e.Element);
        }
    }

    public class DrawStar : MapAction, IDrawStyle
    {
        private Point2D startPt = Point2D.Empty;
        private PolygonElement pentagram;
        private Point2DCollection points;

        private bool isActivated;
        private bool isDrawing;

        private List<PolygonElement> oldPentagrams;
        public event EventHandler<DrawEventArgs> DrawCompleted;

        #region 变量
        private double om;
        private double a1m;
        private double a0x;
        private double a0y;
        private double ox;
        private double oy;
        private Point2D o;
        private Point2D a0;
        private Point2D a1;
        private Point2D a2;
        private Point2D a3;
        private Point2D a4;

        private double maxX;
        private double minX;
        private double maxY;
        private double minY;
        #endregion

        public DrawStar(Map map)
            : this(map,CoreCursorType.Hand)
        {
        }

        public DrawStar(Map map, CoreCursorType cursor)
        {

            Name = "DrawStar";
            Map = map;

            Stroke = new SolidColorBrush(Colors.Red);
            StrokeThickness = 2;
            Fill = new SolidColorBrush(Colors.Red);
            FillRule = FillRule.Nonzero;
            Opacity = 1;


        }

        private void Activate()
        {
            pentagram = new PolygonElement();
            #region 所有风格的控制
            pentagram.Stroke = this.Stroke;
            pentagram.StrokeThickness = this.StrokeThickness;
            pentagram.Fill = this.Fill;
            pentagram.FillRule = this.FillRule;
            pentagram.Opacity = this.Opacity;
            pentagram.StrokeMiterLimit = this.StrokeMiterLimit;
            pentagram.StrokeDashOffset = this.StrokeDashOffset;
            pentagram.StrokeDashArray = this.StrokeDashArray;
            pentagram.StrokeDashCap = this.StrokeDashCap;
            pentagram.StrokeEndLineCap = this.StrokeEndLineCap;
            pentagram.StrokeLineJoin = this.StrokeLineJoin;
            pentagram.StrokeStartLineCap = this.StrokeStartLineCap;
            #endregion
            a0 = new Point2D();
            a1 = new Point2D();
            a2 = new Point2D();
            a3 = new Point2D();
            a4 = new Point2D();

            points = new Point2DCollection();
            pentagram.Point2Ds = points;
            oldPentagrams = new List<PolygonElement>();

            DrawLayer = new ElementsLayer();
            Map.Layers.Add(DrawLayer);

            isActivated = true;
            isDrawing = true;
        }

        public override void Deactivate()
        {
            isActivated = false;
            isDrawing = false;
            oldPentagrams = null;
            pentagram = null;
            points = null;
            if (DrawLayer != null)
            {
                DrawLayer.Children.Clear();
            }
            if (Map != null)
            {
                Map.Layers.Remove(DrawLayer);
            }
        }
        public override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            startPt = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);
            if (!isActivated)
            {
                Activate();
            }
            e.Handled = true;
            base.OnPointerPressed(e);
        }
        public override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (isDrawing)
            {
                for (int i = 0; i < oldPentagrams.Count - 1; i++)
                {
                    DrawLayer.Children.Remove(oldPentagrams[i]);
                }

                Point2D item = Map.ScreenToMap(e.GetCurrentPoint(Map).Position);

                #region 五角星算法
                maxX = Math.Max(startPt.X, item.X);
                minX = Math.Min(startPt.X, item.X);
                maxY = Math.Max(startPt.Y, item.Y);
                minY = Math.Min(startPt.Y, item.Y);

                om = Math.Cos((2 * Math.PI) / 5) * (maxY - minY) / 2;
                a1m = Math.Sin((2 * Math.PI) / 5) * (maxY - minY) / 2;
                a0x = (maxY - minY) / 2 + minX;
                a0y = maxY;
                ox = (maxY - minY) / 2 + minX;
                oy = maxY + (minY - maxY) / 2;
                o = new Point2D(ox, oy);

                a0 = new Point2D(a0x, a0y);
                a1 = new Point2D(ox - a1m, oy + om);
                a2 = new Point2D(ox + a1m, oy + om);
                a3 = new Point2D(ox - Math.Sin(Math.PI / 5) * (maxY - minY) / 2, oy - Math.Cos(Math.PI / 5) * (maxY - minY) / 2);
                a4 = new Point2D(ox + Math.Sin(Math.PI / 5) * (maxY - minY) / 2, oy - Math.Cos(Math.PI / 5) * (maxY - minY) / 2);

                points.Clear();
                points.Add(a0);
                points.Add(a3);
                points.Add(a2);
                points.Add(a1);
                points.Add(a4);

                #endregion
                DrawLayer.Children.Add(pentagram);
                oldPentagrams.Add(pentagram);//等待删除
            }
            base.OnPointerMoved(e);
        }

        public override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (this.points != null)
            {
                GeoRegion geoRegion = new GeoRegion();
                points.Add(a0);
                geoRegion.Parts.Add(points);

                PolygonElement pPen = new PolygonElement()
                {
                    Point2Ds = points,
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

                DrawEventArgs args2 = new DrawEventArgs
                {
                    DrawName = Name,
                    Element = pPen,
                    Geometry = geoRegion
                };

                Deactivate();
                OnDrawComplete(args2);
            }
            e.Handled = true;
            base.OnPointerReleased(e);
        }

        public override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnDoubleTapped(e);
        }

        private void OnDrawComplete(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this, args);
            }
        }

        internal ElementsLayer DrawLayer { get; private set; }
        public FillRule FillRule { get; set; }

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
