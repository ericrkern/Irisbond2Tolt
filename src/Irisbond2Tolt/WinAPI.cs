using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IrisbondAPI
{
    class WinAPI
    {
        #region Mouse

        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void SendClick(int x, int y)
        {
            uint X = (uint)x;
            uint Y = (uint)y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        #endregion

        #region Dynamic load

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Check that all the API dependencies are properly located next to it.
        /// </summary>
        /// <returns>True when the API has all the required dependencies. False when at least 1 dependency is missing</returns>
        public static bool DuoApiHasAllDependencies()
        {
            IntPtr pDll = WinAPI.LoadLibrary("IrisbondAPI.dll");
            if (pDll == IntPtr.Zero)
            {
                string ret = Marshal.GetLastWin32Error().ToString();
                return false;
            }

            WinAPI.FreeLibrary(pDll);
            return true;
        }

        public static bool HiruApiHasAllDependencies()
        {
            IntPtr pDll = WinAPI.LoadLibrary("IrisbondHiruAPI.dll");
            if (pDll == IntPtr.Zero)
            {
                string ret = Marshal.GetLastWin32Error().ToString();
                return false;
            }

            WinAPI.FreeLibrary(pDll);
            return true;
        }

        #endregion
    }
}
