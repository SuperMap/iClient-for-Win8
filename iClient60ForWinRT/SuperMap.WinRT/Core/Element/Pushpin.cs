using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// ${mapping_Pushpin_Title}<br/>
    /// ${mapping_Pushpin_Description}
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    public class Pushpin : ContentControl
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="Pushpin">Pushpin</see>
        ///     ${pubilc_Constructors_instance}
        /// </summary>
        public Pushpin()
        {
            base.DefaultStyleKey = typeof(Pushpin);
            base.PointerEntered += Pushpin_PointerEntered;
            base.PointerExited += Pushpin_PointerExited;
            base.VerticalAlignment = VerticalAlignment.Top;
        }

        void Pushpin_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!DisableAnimation)
            {
                GoToState("Normal");
            }
        }

        void Pushpin_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!DisableAnimation)
            {
                GoToState("MouseOver");
            }
        }

        private void GoToState(string stateName)
        {
            VisualStateManager.GoToState(this, stateName, true);
        }
        private static void OnLocationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            Rectangle2D bounds = new Rectangle2D((Point2D)eventArgs.NewValue, (Point2D)eventArgs.NewValue);
            ElementsLayer.SetBBox(d, bounds);
        }
        /// <summary>${mapping_Pushpin_attribute_Location_D}</summary>
        public Point2D Location
        {
            get
            {
                return (Point2D)base.GetValue(LocationDependencyProperty);
            }
            set
            {
                base.SetValue(LocationDependencyProperty, value);
            }
        }

        /// <summary>${mapping_Pushpin_attribute_LocationDependencyProperty_D}</summary>
        public static readonly DependencyProperty LocationDependencyProperty = DependencyProperty.Register("Location", typeof(Point2D), typeof(Pushpin), new PropertyMetadata(Point2D.Empty, new PropertyChangedCallback(Pushpin.OnLocationChangedCallback)));
        /// <summary>${mapping_Pushpin_attribute_DisableAnimation_D}</summary>
        public bool DisableAnimation { get; set; }
    }
}
