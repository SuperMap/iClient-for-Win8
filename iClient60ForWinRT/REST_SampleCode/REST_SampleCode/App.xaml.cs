using REST_SampleCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “网格应用程序”模板在 http://go.microsoft.com/fwlink/?LinkId=234226 上有介绍

namespace REST_SampleCode
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        Popup settingsPopup;

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        void App_CommandsRequested(Windows.UI.ApplicationSettings.SettingsPane sender, Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);
            SettingsCommand aboutCommand = new SettingsCommand("about", "关于", handler);
            args.Request.ApplicationCommands.Add(aboutCommand);

            SettingsCommand cacheCommand = new SettingsCommand("clearcache", "清空缓存", handler);
            args.Request.ApplicationCommands.Add(cacheCommand);

            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacypolicy", "隐私声明", handler);
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);

        }

        async void onSettingsCommand(IUICommand command)
        {
            switch (command.Id.ToString().ToLower())
            {
                case "clearcache":
                    foreach (var folder in await ApplicationData.Current.LocalFolder.GetFoldersAsync())
                    {
                        await folder.DeleteAsync();
                    }
                    REST_SampleCode.Controls.MessageBox.Show("清除成功！");
                    break;
                case "privacypolicy":
                    settingsPopup = new Popup();
                    settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;
                    settingsPopup.IsLightDismissEnabled = true;
                    settingsPopup.Width = 646;
                    settingsPopup.Height = Window.Current.Bounds.Height;

                    // Add the proper animation for the panel.
                    settingsPopup.ChildTransitions = new TransitionCollection();
                    settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
                    {
                        Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                               EdgeTransitionLocation.Right :
                               EdgeTransitionLocation.Left
                    });

                    // Create a SettingsFlyout the same dimenssions as the Popup.
                    PrivacyPolicyPage mypane = new PrivacyPolicyPage();
                    mypane.Width = 646;
                    mypane.Height = Window.Current.Bounds.Height;

                    // Place the SettingsFlyout inside our Popup window.
                    settingsPopup.Child = mypane;

                    // Let's define the location of our Popup.
                    settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - 646) : 0);
                    settingsPopup.SetValue(Canvas.TopProperty, 0);
                    settingsPopup.IsOpen = true;
                    break;

                case "about":
                    settingsPopup = new Popup();
                    settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;
                    settingsPopup.IsLightDismissEnabled = true;
                    settingsPopup.Width = 646;
                    settingsPopup.Height = Window.Current.Bounds.Height;

                    // Add the proper animation for the panel.
                    settingsPopup.ChildTransitions = new TransitionCollection();
                    settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
                    {
                        Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                               EdgeTransitionLocation.Right :
                               EdgeTransitionLocation.Left
                    });

                    // Create a SettingsFlyout the same dimenssions as the Popup.
                    AboutPage about = new AboutPage();
                    about.Width = 646;
                    about.Height = Window.Current.Bounds.Height;

                    // Place the SettingsFlyout inside our Popup window.
                    settingsPopup.Child = about;

                    // Let's define the location of our Popup.
                    settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - 646) : 0);
                    settingsPopup.SetValue(Canvas.TopProperty, 0);
                    settingsPopup.IsOpen = true;
                    break;
            }
        }

        /// <summary>
        /// This event is generated when the user opens the settings pane. During this event, append your
        /// SettingsCommand objects to the available ApplicationCommands vector to make them available to the
        /// SettingsPange UI.
        /// </summary>
        /// <param name="settingsPane">Instance that triggered the event.</param>
        /// <param name="eventArgs">Event data describing the conditions that led to the event.</param>
        void onCommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);

            SettingsCommand generalCommand = new SettingsCommand("DefaultsId", "Defaults", handler);
            eventArgs.Request.ApplicationCommands.Add(generalCommand);
        }

        /// <summary>
        /// We use the window's activated event to force closing the Popup since a user maybe interacted with
        /// something that didn't normally trigger an obvious dismiss.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        /// <summary>
        /// When the Popup closes we no longer need to monitor activation changes.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以执行打开特定的文件或显示搜索结果等操作时
        /// 将使用其他入口点。
        /// </summary>
        /// <param name="args">有关启动请求和过程的详细信息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态

            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();
                //将框架与 SuspensionManager 键关联                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 仅当合适时才还原保存的会话状态
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //还原状态时出现问题。
                        //假定没有状态并继续
                    }
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
                this.Suspending += OnSuspending;
            }
            if (rootFrame.Content == null)
            {
                // 当未还原导航堆栈时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 新页
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。在不知道应用程序
        /// 将被终止还是恢复的情况下保存应用程序状态，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起的请求的详细信息。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
