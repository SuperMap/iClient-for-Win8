using Microsoft.Phone.Controls;
using SuperMap.WindowsPhone.Core;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();


            MyMap.ViewBounds = new Rectangle2D(-180, -90, 90, 180);
        }

    }
}