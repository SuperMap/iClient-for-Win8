using iServerJava6R_SampleCode.Controls;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace iServerJava6R_SampleCode
{
    public partial class Xaml : Page
    {
        public Xaml()
        {
            InitializeComponent();
        }

       async private void btn_Click(object sender, RoutedEventArgs e)
        {
            await MessageBox.Show("欢迎来到中国！");
        }
    }
}