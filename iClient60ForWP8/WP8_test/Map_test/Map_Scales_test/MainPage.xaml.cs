using Microsoft.Phone.Controls;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            double[] Scales = new double[] { 1.9e-9, 3.0e-9, 6.4e-9, 1.1e-8, 3.0e-8, 5.0e-8 };
            MyMap.Scales = Scales;
        }

    }
}