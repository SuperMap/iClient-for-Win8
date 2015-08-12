using System.Windows;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace iServerJava6R_SampleCode
{
    public partial class InputWindow : ChildWindow
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            double Radiu;

            if (double.TryParse(Radius.Text, out Radiu))
            {
                if (Radiu <= 0)
                {
                    return;
                }
                this.Tag = Radius.Text;
                Close();
            }
        }
    }

    //此类对输入数据进行大于0验证
    public class CheckInput : IDataErrorInfo
    {
        public CheckInput(string radius)
        {
            Radius = double.Parse(radius);
        }

        public CheckInput()
        { }

        public double Radius
        {
            get;
            set;
        }

        //验证数据，如果错误并返回说明。
        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (Radius <= 0)
                {
                    result = "必须输入大于0的数字";
                }
                return result;
            }

        }

        public string Error
        {
            get { return null; }
        }
    }
}
