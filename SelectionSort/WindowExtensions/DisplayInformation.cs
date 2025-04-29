using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SelectionSort.WindowExtensions
{

    public class DisplayInfo
    {
        public string Availability { get; set; }
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenEfectiveHeight
        {
            get
            {
                uint widthDPI;
                _ = Vanara.PInvoke.SHCore.GetDpiForMonitor(hMonitor,
                    Vanara.PInvoke.SHCore.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out widthDPI, out _);
                float scalingFactor = (float)widthDPI / 96;
                return (int)(ScreenHeight / scalingFactor);
            }
        }
        public int ScreenEfectiveWidth
        {
            get
            {
                uint heightDPI;
                _ = Vanara.PInvoke.SHCore.GetDpiForMonitor(hMonitor,
                    Vanara.PInvoke.SHCore.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out _, out heightDPI);
                float scalingFactor = (float)heightDPI / 96;
                return (int)(ScreenWidth / scalingFactor);
            }
        }
        public string DeviceName { get; set; }
        public Vanara.PInvoke.RECT WorkArea { get; set; }
        public IntPtr hMonitor { get; set; }
    }

    public class DisplayInformation
    {
        public static int ConvertEpxToPixel(IntPtr hwnd, int effectivePixels)
        {
            float scalingFactor = GetScalingFactor(hwnd);
            return (int)(effectivePixels * scalingFactor);
        }

        public static int ConvertPixelToEpx(IntPtr hwnd, int pixels)
        {
            float scalingFactor = GetScalingFactor(hwnd);
            return (int)(pixels / scalingFactor);
        }
        
        public static float GetScalingFactor(IntPtr hwnd)
        {
            var dpi = Vanara.PInvoke.User32.GetDpiForWindow(hwnd);
            float scalingFactor = (float)dpi / 96;
            return scalingFactor;
        }

        public static DisplayInfo GetDisplay(IntPtr hwnd)
        {
            DisplayInfo di = null;
            IntPtr hMonitor;
           Vanara.PInvoke.RECT rc;
           Vanara.PInvoke.User32.GetWindowRect(hwnd, out rc);
            hMonitor = (nint)Vanara.PInvoke.User32.MonitorFromRect(ref rc, Vanara.PInvoke.User32.MonitorFlags.MONITOR_DEFAULTTONEAREST);

            Vanara.PInvoke.User32.MONITORINFOEX mi = new();
            mi.cbSize = (uint)Marshal.SizeOf(mi);
            bool success = Vanara.PInvoke.User32.GetMonitorInfo(hMonitor, ref mi);
            if (success)
            {
                di = ConvertMonitorInfoToDisplayInfo(hMonitor, mi);
            }
            return di;
        }

        private static DisplayInfo ConvertMonitorInfoToDisplayInfo(IntPtr hMonitor, Vanara.PInvoke.User32.MONITORINFOEX mi)
        {
            return new DisplayInfo
            {
                ScreenWidth = mi.rcMonitor.right - mi.rcMonitor.left,
                ScreenHeight = mi.rcMonitor.bottom - mi.rcMonitor.top,
                DeviceName = mi.szDevice,
                WorkArea = new Vanara.PInvoke.RECT(mi.rcMonitor.left, mi.rcMonitor.top, mi.rcMonitor.right, mi.rcMonitor.bottom),
                Availability = mi.dwFlags.ToString(),
                hMonitor = hMonitor
            };
        }

        public static List<DisplayInfo> GetDisplays()
        {
            List<DisplayInfo> col = new();

            _ = EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Vanara.PInvoke.RECT lprcMonitor, IntPtr dwData)
                {
                    Vanara.PInvoke.User32.MONITORINFOEX mi = new Vanara.PInvoke.User32.MONITORINFOEX();
                    mi.cbSize = (uint)Marshal.SizeOf(mi);
                    bool success = Vanara.PInvoke.User32.GetMonitorInfo(hMonitor, ref mi);
                    if (success)
                    {
                        DisplayInfo di = ConvertMonitorInfoToDisplayInfo(hMonitor, mi);
                        col.Add(di);
                    }
                    return true;
                }, IntPtr.Zero);
            return col;
        }

        public enum UserInteractionModeEnum { Touch, Mouse };
        public static UserInteractionModeEnum UserInteractionMode
        {
            get
            {
                // TODO: Have a counterpart event listeining the message WM_SETTINGCHANGE
                UserInteractionModeEnum userInteractionMode = UserInteractionModeEnum.Mouse;
                int SM_CONVERTIBLESLATEMODE = 0x2003;
                int state = GetSystemMetrics(SM_CONVERTIBLESLATEMODE);//O for tablet
                if(state == 0)
                {
                    userInteractionMode = UserInteractionModeEnum.Touch;
                }
                return userInteractionMode;
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "GetSystemMetrics")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);
        delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Vanara.PInvoke.RECT lprcMonitor, IntPtr dwData);

    }
}
