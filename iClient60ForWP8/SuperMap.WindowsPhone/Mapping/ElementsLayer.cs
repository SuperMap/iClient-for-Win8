using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Resources;
using System.Collections.Generic;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_ArbitraryLayer_Title}</para>
    /// 	<para>${WP_mapping_ArbitraryLayer_Description}</para>
    /// </summary>
    [ContentProperty("Children")]
    public class ElementsLayer : Layer
    {
        private static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children", typeof(ObservableCollection<UIElement>), typeof(ElementsLayer), null);
        /// <example>
        /// 	<code title="Example1" description="" lang="C#">ElementsLayer.Children.Add(UIElement);</code>
        /// 	<code title="Example2" description="" lang="XAML">&lt;namespace:ElemntsLayer&gt;
        ///   &lt;UIElement/&gt;
        /// &lt;/namespace:ElemntsLayer&gt;</code>
        /// </example>
        /// <summary>${WP_mapping_ArbitraryLayer_attribute_children_D}</summary>
        /// <remarks>${WP_mapping_ArbitraryLayer_attribute_children_Remarks}</remarks>
        public ObservableCollection<UIElement> Children
        {
            get
            {
                return (ObservableCollection<UIElement>)base.GetValue(ChildrenProperty);
            }
            private set { base.SetValue(ChildrenProperty, value); }
        }

        //是否启用避让
        /// <summary>${WP_mapping_ArbitraryLayer_attribute_IsAutoAvoidance_D}
        /// <para>${WP_mapping_ArbitraryLayer_attribute_IsAutoAvoidance_D_1}</para>
        ///  <para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><img src="avoidPrinciple.png"/></term>
        /// 				<description><img src="avoidAlgorithm.png"/></description>
        /// 			</item>
        /// 		</list>
        ///  </para>
        /// 	<para>${WP_mapping_ArbitraryLayer_attribute_IsAutoAvoidance_D_2}</para>
        ///     <para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><img src="avoidProcess.png"/></term>
        /// 			</item>
        /// 		</list>
        /// 	</para>
        /// </summary>
        public bool IsAutoAvoidance { get; set; }

        //每个所指示地物的参考长度，单位是像素。
        private static readonly DependencyProperty ReferLengthProperty = DependencyProperty.Register("ReferenceLength", typeof(Double), typeof(UIElement), null);

        //获取设置的地物的参考长度

        /// <summary>${WP_mapping_FeaturesLayer_method_getReferenceLength_D}</summary>
        /// <returns>${WP_mapping_FeaturesLayer_method_ggetReferenceLength_return}</returns>
        /// <param name="elem">${WP_mapping_FeaturesLayer_method_ggetReferenceLength_param_elem}</param>
        internal double GetReferenceLength(UIElement elem)
        {
            return (double)elem.GetValue(ReferLengthProperty);
        }

        //标签本人的参考尺寸，如果不设置，则为标签的默认尺寸，单位是像素。
        private static readonly DependencyProperty ReferSizeProperty = DependencyProperty.Register("ReferenceSize", typeof(Size), typeof(UIElement), null);

        //获取设置的标签本人的参考尺寸
        /// <summary>${WP_mapping_FeaturesLayer_method_getReferenceSize_D}</summary>
        /// <returns>${WP_mapping_FeaturesLayer_method_getReferenceSize_return}</returns>
        /// <param name="elem">${WP_mapping_FeaturesLayer_method_getReferenceSize_param_elem}</param>
        internal Size GetReferenceSize(UIElement elem)
        {
            return (Size)elem.GetValue(ReferSizeProperty);
        }

        //标签的地理范围，启动避让则是避让后的地理范围。
        /// <summary>${WP_mapping_ArbitraryLayer_method_BoundsCollection_D}</summary>
        public IObservableDictionary<Object, Rectangle2D> BoundsCollection
        {
            get;
            set;
        }


        #region BBOX附加属性
        /// <summary>${WP_mapping_ArbitraryLayer_method_getbbox_D}</summary>
        /// <param name="obj">${WP_mapping_ArbitraryLayer_method_getbbox_param_obj}</param>
        public static Rectangle2D GetBBox(DependencyObject obj)
        {
            return (Rectangle2D)obj.GetValue(BBoxProperty);
        }
        /// <example>${WP_mapping_ArbitraryLayer_method_setbbox_Example_D}<code inline="false" title="" description="" lang="CS"></code><code title="Example1" description="" lang="C#">Ellipse myellipse = new Ellipse();     
        /// Rectangle2D ellbounds = new Rectangle2D(left, bottom, right, top);
        /// myellipse.SetValue(ElementsLayer.BBoxProperty, ellbounds);
        /// this.ElementsLayer.AddChild(myellipse);</code><code title="Example2" description="" lang="CS">&lt;ELLIPSE id=dxCrLf &lt;SPAN x:Name="MyEllipse"&gt;&lt;/SPAN&gt; namespace:ElementsLayer.BBox="left, bottom, right, top"&amp;gt; 
        /// &lt;/ELLIPSE&gt;</code></example>
        /// <summary>${WP_mapping_ArbitraryLayer_method_setbbox_D}</summary>
        /// <param name="obj">${WP_mapping_ArbitraryLayer_method_setbbox_param_obj}</param>
        /// <param name="value">${WP_mapping_ArbitraryLayer_method_setbbox_param_value}</param>
        public static void SetBBox(DependencyObject obj, Rectangle2D value)
        {
            obj.SetValue(BBoxProperty, value);
        }
        /// <summary>${WP_mapping_ArbitraryLayer_attribute_BBoxProperty_D}</summary>
        public static readonly DependencyProperty BBoxProperty =
            DependencyProperty.RegisterAttached("BBox", typeof(Rectangle2D), typeof(ElementsLayer), new PropertyMetadata(Rectangle2D.Empty, new PropertyChangedCallback(OnBBoxChanged)));
        /// <summary>${WP_mapping_ArbitraryLayer_method_onbboxchanged_D}</summary>
        /// <param name="dependencyObject">${WP_mapping_ArbitraryLayer_method_onbboxchanged_param_dependencyObject}</param>
        /// <param name="e">${WP_mapping_ArbitraryLayer_method_onbboxchanged_param_e}</param>
        private static void OnBBoxChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            //dependencyObject.SetValue(LayerContainer.BoundsProperty, e.NewValue);//ok
            LayerContainer.SetBounds(dependencyObject, (Rectangle2D)e.NewValue);
        }
        #endregion

        /// <summary>${WP_pubilc_Constructors_Initializes} <see cref="ElementsLayer">ElementsLayer</see>
        ///     ${WP_pubilc_Constructors_instance}</summary>
        public ElementsLayer()
        {
            Children = new ObservableCollection<UIElement>();
            Children.CollectionChanged += new NotifyCollectionChangedEventHandler(children_CollectionChanged);
            IsAutoAvoidance = false;
            BoundsCollection = new TObservableDictionary<object, Rectangle2D>();
        }

        internal override void Draw(DrawParameter drawParameter)
        {
            base.Draw(drawParameter);
            if (base.Container != null)
            {
                Rectangle2D bounds = ViewBounds;
                double num = 20.0 * Resolution;
                Rectangle2D biggerBounds = bounds.Inflate(num, num);//上下左右扩张20个像素的距离
                foreach (UIElement element in this.Children)
                {
                    if (element == null)
                    {
                        return;
                    }
                    if (!(element.GetValue(BBoxProperty) is Rectangle2D))
                    {
                        throw new ArgumentException(ExceptionStrings.BboxIsNotSet);
                    }

                    Rectangle2D rect = GetBBox(element);
                    if (rect.IntersectsWith(biggerBounds) && !base.Container.Children.Contains(element))
                    {
                        if (element is ShapeElement)
                        {
                            ShapeElement sb = element as ShapeElement;
                            sb.SetPath();
                            sb.InvalidatePath(base.Container.Resolution, base.Container.OriginX, base.Container.OriginY);
                            ElementsLayer.SetBBox(sb, sb.ClippedBounds);
                        }
                        base.Container.Children.Add(element);
                    }
                }
            }
        }

        /// <overloads>${WP_mapping_ArbitraryLayer_method_addChild_element_overloads}</overloads>
        /// <summary>${WP_mapping_ArbitraryLayer_method_addChild_element_D}</summary>
        /// <param name="element">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_pararm_element}</param>
        public void AddChild(UIElement element)
        {
            Children.Add(element);
        }

        /// <summary>${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_D}</summary>
        /// <param name="element">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_pararm_element}</param>
        /// <param name="location">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_param_location}</param>
        public void AddChild(UIElement element, Point2D location)
        {
            AddChild(element, new Rectangle2D(location, location));
        }
        /// <summary>${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_double_Size_D}</summary>
        /// <param name="element">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_pararm_element}</param>
        /// <param name="location">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_param_location}</param>
        /// <param name="referenceLength">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_double_param_referenceLength}</param>
        /// <param name="referenceSize">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_double_Size_param_referenceSize}</param>
        public void AddChild(UIElement element, Point2D location, double referenceLength, Size referenceSize)
        {
            element.SetValue(ReferLengthProperty, referenceLength);
            element.SetValue(ReferSizeProperty, referenceSize);
            AddChild(element, new Rectangle2D(location, location));
        }
        /// <summary>${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_double_D}</summary>
        /// <param name="element">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_pararm_element}</param>
        /// <param name="location">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_param_location}</param>
        /// <param name="referenceLength">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Point2D_double_param_referenceLength}</param>
        public void AddChild(UIElement element, Point2D location, double referenceLength)
        {
            element.SetValue(ReferLengthProperty, referenceLength);
            AddChild(element, new Rectangle2D(location, location));
        }

        /// <summary>${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Rectangle2D_D}</summary>
        /// <param name="element">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_pararm_element}</param>
        /// <param name="bbox">${WP_mapping_ArbitraryLayer_method_addChild_UIElement_Rectangle2D_param_bbox}</param>
        public void AddChild(UIElement element, Rectangle2D bbox)
        {
            if (!(element is ShapeElement))
            {
                ElementsLayer.SetBBox(element, bbox);
            }
            Children.Add(element);
        }

        private Rectangle2D fullBounds = Rectangle2D.Empty;

        private void children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!Rectangle2D.IsNullOrEmpty(this.fullBounds))
                {
                    UpdateFullBounds(e.NewItems);
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    base.Container.Children.Clear();
                }
                else
                {
                    base.Container.Children.Remove(e.OldItems[0] as UIElement);
                }
                this.fullBounds = Rectangle2D.Empty;
            }
            this.Invalidate();

        }

        private bool isInvalidated;
        private void Invalidate()
        {
            if (!this.isInvalidated)
            {
                CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
                this.isInvalidated = true;
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            CompositionTarget.Rendering -= new EventHandler(this.CompositionTarget_Rendering);
            base.Dispatcher.BeginInvoke(delegate
            {
                base.OnLayerChanged();
                this.isInvalidated = false;
            });
        }

        private void UpdateFullBounds(IEnumerable items)
        {
            foreach (object obj2 in items)
            {
                UIElement element = (UIElement)obj2;
                Rectangle2D rect2D = GetBBox(element);
                if (element is ShapeElement)
                {
                    rect2D = ((ShapeElement)element).Bounds;
                }

                if (Rectangle2D.IsNullOrEmpty(fullBounds))
                {
                    fullBounds = rect2D;
                }
                else
                {
                    fullBounds=fullBounds.Union(rect2D);
                }
            }
        }

        /// <example>
        /// 	<code title="Example1" description="" lang="XAML">&lt;namespace:ElementsLayer id="myElementsLayer" Bounds="left,bottom,right,top"&gt;
        /// &lt;/namespace:ElementsLayer&gt;</code>
        /// </example>
        /// <summary>${WP_mapping_ArbitraryLayer_attribute_bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                if ((Rectangle2D.IsNullOrEmpty(this.fullBounds)) && (this.Children.Count > 0))
                {
                    UpdateFullBounds(this.Children);
                }
                return this.fullBounds;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        internal override bool ContinuousDraw
        {
            get
            {
                return true;
            }
        }
    }
}
