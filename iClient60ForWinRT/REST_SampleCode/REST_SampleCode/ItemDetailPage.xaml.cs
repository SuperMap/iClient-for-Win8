using REST_SampleCode.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “项详细信息页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234232 上提供

namespace REST_SampleCode
{
    /// <summary>
    /// 显示组内单个项的详细信息同时允许使用手势
    /// 浏览同一组的其他项的页。
    /// </summary>
    public sealed partial class ItemDetailPage : REST_SampleCode.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        public ItemShow ItemShowData
        {
            get;
            set;
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // 允许已保存页状态重写要显示的初始项
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: 创建适用于问题域的合适数据模型以替换示例数据
            var item = SampleDataSource.GetItem((String)navigationParameter);
            ItemShowData = new ItemShow();

            //此处先注释掉。通过路径获取详细的文本信息，并导航到指定的页面。等迁移完成后再逐步恢复。
            try
            {
                ItemShowData.CS = await PathIO.ReadTextAsync(string.Format("ms-appx:///{0}", item.CSCode));
            }
            catch { }
            ItemShowData.Description = item.Description;
            try
            {
                ItemShowData.Xaml = await PathIO.ReadTextAsync(string.Format("ms-appx:///{0}", item.XamlCode));
            }
            catch { }
            ItemShowData.ItemData = item;
            try
            {
                MapContent.Navigate(Type.GetType(item.Content));
            }
            catch { }
            this.DefaultViewModel["Item"] = ItemShowData;
        

        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HideAll();
            MapContent.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HideAll();
            XamlContent.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            HideAll();
            CSContent.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            HideAll();
            DescriptionContent.Visibility = Visibility.Visible;
        }

        private void HideAll()
        {
            MapContent.Visibility = Visibility.Collapsed;
            XamlContent.Visibility = Visibility.Collapsed;
            CSContent.Visibility = Visibility.Collapsed;
            DescriptionContent.Visibility = Visibility.Collapsed;
        }

    }
}
