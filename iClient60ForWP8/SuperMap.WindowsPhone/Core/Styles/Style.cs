using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_Style_Title}</para>
    /// 	<para>${WP_core_Style_Description}</para>
    /// </summary>
    [KnownType(typeof(MarkerStyle)), KnownType(typeof(FillStyle)), KnownType(typeof(LineStyle))]
    public abstract class Style : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>${WP_core_Style_field_controlTemplateProperty}</summary>
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty ControlTemplateProperty = DependencyProperty.Register("ControlTemplate", typeof(ControlTemplate), typeof(Style), new PropertyMetadata(new PropertyChangedCallback(OnControlTemplatePropertyChanged)));

        /// <summary>${WP_core_Style_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>${WP_core_Style_constructor_None_D}</summary>
        protected Style()
        {
        }

        // <summary>${WP_core_Style_method_LoadTemplateFromXamlFile_D}</summary>
        // <param name="fileUri">${WP_core_Style_method_LoadTemplateFromXamlFile_param_fileUri_D}</param>
        //protected void LoadTemplateFromXamlFile(string fileUri)
        //{
        //    string xaml = StyleUtility.XamlFileToString(fileUri);
        //    this.ControlTemplate = XamlReader.Load(xaml) as ControlTemplate;
        //}


        private static void OnControlTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Style).OnPropertyChanged("ControlTemplate");
        }

        /// <summary>${WP_core_Style_method_OnPropertyChanged_D}</summary>
        /// <param name="propertyName">${WP_core_Style_method_OnPropertyChanged_param_propertyName_D}</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <remarks>${WP_core_Style_attribute_ControlTemplate_Remarks}</remarks>
        /// <summary>${WP_core_Style_attribute_ControlTemplate_D}</summary>
        public ControlTemplate ControlTemplate
        {
            get
            {
                return (base.GetValue(ControlTemplateProperty) as ControlTemplate);
            }
            set
            {
                base.SetValue(ControlTemplateProperty, value);
            }
        }
    }
}
