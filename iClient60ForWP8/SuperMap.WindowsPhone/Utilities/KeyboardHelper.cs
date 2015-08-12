using System.Windows.Input;

namespace SuperMap.WindowsPhone.Utilities
{
    //来源：System.Windows.Controls.Data程序集，System.Windows.Controls命名空间
    internal static class KeyboardHelper
    {
        public static bool ShiftIsDown()
        {
            return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }

        //internal类中没有用到的就先注释了
        //public static bool CtrlIsDown()
        //{
        //    bool ctrl;
        //    ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //    ctrl |= (Keyboard.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows;
        //    return ctrl;
        //}

        //public static bool AltIsDown()
        //{
        //    return (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        //}
        //public static void GetMetaKeyState(out bool ctrl, out bool shift)
        //{
        //    ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //    ctrl |= (Keyboard.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows;
        //    shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        //}

        //public static void GetMetaKeyState(out bool ctrl, out bool shift, out bool alt)
        //{
        //    GetMetaKeyState(out ctrl, out shift);
        //    alt = (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        //}
    }
}
