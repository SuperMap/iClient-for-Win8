using SuperMap.WinRT.Mapping;
using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace iServerJava6R_SampleCode
{
    public partial class AddImagesAndMedia : Page
    {
        public AddImagesAndMedia()
        {
            InitializeComponent();       
        }
  
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = sender as MediaElement;
            media.Position = TimeSpan.FromSeconds(0);
            media.Play();
        }
    }
}
