using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace REST_SampleCode.Controls
{
    // 摘要:
    //     指定显示消息框时要包含的按钮。
    public enum MessageBoxButton
    {
        // 摘要:
        //     仅显示“确定”按钮。
        OK = 0,
        //
        // 摘要:
        //     同时显示“确定”和“取消”按钮。
        OKCancel = 1,
    }
    // 摘要:
    //     表示对消息框的用户响应。
    public enum MessageBoxResult
    {
        // 摘要:
        //     当前未使用此值。
        None = 0,
        //
        // 摘要:
        //     用户单击了“确定”按钮。
        OK = 1,
        //
        // 摘要:
        //     用户单击了“取消”按钮或按下了 Esc。
        Cancel = 2,
        //
        // 摘要:
        //     当前未使用此值。
        Yes = 6,
        //
        // 摘要:
        //     当前未使用此值。
        No = 7,
    }
  
    public class MessageBox
    {
        static string okStr = "确定", cancelStr = "取消", captionStr = "提示";

        public async static Task<IUICommand> Show(string message)
        {
            MessageBox msg = new MessageBox();
            return await msg.ShowMessage(message);
        }


        public async static Task<MessageBoxResult> Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBox box = new MessageBox();

            var result = await box.ShowMessage(messageBoxText, caption, MessageBoxButton.OKCancel);

            return getResult(result);
        }

        public async Task<IUICommand> ShowMessage(string message)
        {
            return await ShowMessage(message, captionStr, MessageBoxButton.OK);
        }


        public async Task<IUICommand> ShowMessage(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageDialog msg = new MessageDialog(messageBoxText, caption);

            msg.Commands.Add(new UICommand(okStr, CommandInvokedHandler));
            if (button == MessageBoxButton.OKCancel)
            {
                msg.Commands.Add(new UICommand(cancelStr, CommandInvokedHandler));
            }
            msg.DefaultCommandIndex = 1;
            msg.CancelCommandIndex = 1;
            IUICommand result = await msg.ShowAsync();
            return result;
        }

        public delegate void CompleteHandler(MessageBoxResult result);

        public CompleteHandler Complete;


        private void CommandInvokedHandler(IUICommand command)
        {
            if (Complete != null)
            {
                Complete(getResult(command));

            }
        }

        private static MessageBoxResult getResult(IUICommand command)
        {
            MessageBoxResult msgresult = MessageBoxResult.Cancel;
            if (command.Label == okStr)
            {
                msgresult = MessageBoxResult.OK;
            }
            else if (command.Label == cancelStr)
            {
                msgresult = MessageBoxResult.Cancel;
            }
            else
            {
                msgresult = MessageBoxResult.None;
            }
            return msgresult;
        }


    }
}
