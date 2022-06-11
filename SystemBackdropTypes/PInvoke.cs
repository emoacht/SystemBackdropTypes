using System;
using System.Runtime.InteropServices;

namespace SystemBackdropTypes;

public class PInvoke
{
    public class ParameterTypes
    {
        [Flags]
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_SYSTEMBACKDROP_TYPE = 38
        }

        public enum DWMSYSTEMBACKDROPTYPE
        {
            DWMSBT_AUTO = 0,        // [Default] Let DWM automatically decide the system-drawn backdrop for this window.
            DWMSBT_NONE,            // Do not draw any system backdrop.
            DWMSBT_MAINWINDOW,      // Draw the backdrop material effect corresponding to a long-lived window.
            DWMSBT_TRANSIENTWINDOW, // Draw the backdrop material effect corresponding to a transient window.
            DWMSBT_TABBEDWINDOW     // Draw the backdrop material effect corresponding to a window with a tabbed title bar.
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;      // width of left border that retains its size
            public int cxRightWidth;     // width of right border that retains its size
            public int cyTopHeight;      // height of top border that retains its size
            public int cyBottomHeight;   // height of bottom border that retains its size
        };
    }

    public static class Methods
    {
        [DllImport("DwmApi.dll")]
        static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd,
            ref ParameterTypes.MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, ParameterTypes.DWMWINDOWATTRIBUTE dwAttribute, ref int pvAttribute, int cbAttribute);

        public static int ExtendFrame(IntPtr hwnd, ParameterTypes.MARGINS margins)
            => DwmExtendFrameIntoClientArea(hwnd, ref margins);

        public static int SetWindowAttribute(IntPtr hwnd, ParameterTypes.DWMWINDOWATTRIBUTE attribute, int parameter)
            => DwmSetWindowAttribute(hwnd, attribute, ref parameter, Marshal.SizeOf<int>());
    }
}
