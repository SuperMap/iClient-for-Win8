using Microsoft.Phone.Controls;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            MyMap.Scales = new double[] { 6.4e-9, 1.1e-8, 3.0e-8, 2.4e-7 };
            MyMap.Scales = new double[] { 5e-5, 1.25e-4, 2e-4, 5e-4 };
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyMap.ZoomIn();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            MyMap.ZoomOut();
        }

    }
}