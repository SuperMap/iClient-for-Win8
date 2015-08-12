using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SuperMap.WindowsPhone.Mapping;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// ${WP_mapping_Pushpin_Title}<br/>
    /// ${WP_mapping_Pushpin_Description}
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    public class Pushpin : ContentControl
    {
        /// <summary>
        ///     ${WP_pubilc_Constructors_Initializes} <see cref="Pushpin">Pushpin</see>
        ///     ${WP_pubilc_Constructors_instance}
        /// </summary>
        public Pushpin()
        {
            base.DefaultStyleKey = typeof(Pushpin);
            base.MouseEnter += new MouseEventHandler(Pushpin_MouseEnter);
            base.MouseLeave += new MouseEventHandler(Pushpin_MouseLeave);
            base.VerticalAlignment = VerticalAlignment.Top;
        }
        private void Pushpin_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!DisableAnimation)
            {
                GoToState("Normal");
            }

        }
        private void Pushpin_MouseEnter(object sender, MouseEventArgs e)
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
        /// <summary>${WP_mapping_Pushpin_attribute_Location_D}</summary>
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

        /// <summary>${WP_mapping_Pushpin_attribute_LocationDependencyProperty_D}</summary>
        public static readonly DependencyProperty LocationDependencyProperty = DependencyProperty.Register("Location", typeof(Point2D), typeof(Pushpin), new PropertyMetadata(Point2D.Empty, new PropertyChangedCallback(Pushpin.OnLocationChangedCallback)));
        /// <summary>${WP_mapping_Pushpin_attribute_DisableAnimation_D}</summary>
        public bool DisableAnimation { get; set; }
    }
}
