using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Utils
{
    public class MouseInput
    {
        public enum mouseOption { MOUSE_LEFT_CLIC, MOUSE_DOUBLE_CLIC, MOUSE_RIGHT_CLIC, MOUSE_DRAG, MOUSE_DROP }

        public void MoveMouse(int X, int Y)
        {
            // mouse co-ords: top left is (0,0), bottom right is (65535, 65535)
            // convert screen co-ord to mouse co-ords...
            if (Screen.PrimaryScreen == null) return;
            int mouseCoordX = X * 65535 / Screen.PrimaryScreen.Bounds.Width;
            int mouseCoordY = Y * 65535 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine("+ new mouse coord: " + mouseCoordX+","+ mouseCoordY);
            DoMouse(NativeMethods.MOUSEEVENTF.MOVE | NativeMethods.MOUSEEVENTF.ABSOLUTE, new Point(mouseCoordX, mouseCoordY));
        }

        public void MoveMouseFromMouseCoords(Point p)
        {
            DoMouse(NativeMethods.MOUSEEVENTF.MOVE | NativeMethods.MOUSEEVENTF.ABSOLUTE, new Point(p.X, p.Y));
        }

        public void LeftClick()
        {
            DoMouse(NativeMethods.MOUSEEVENTF.LEFTDOWN, new System.Drawing.Point(0, 0));
            DoMouse(NativeMethods.MOUSEEVENTF.LEFTUP, new System.Drawing.Point(0, 0));
        }

        public void LeftClick(Point p)
        {
            LeftClick(p.X, p.Y);
        }

        public void RightClick()
        {
            DoMouse(NativeMethods.MOUSEEVENTF.RIGHTDOWN, new System.Drawing.Point(0, 0));
            DoMouse(NativeMethods.MOUSEEVENTF.RIGHTUP, new System.Drawing.Point(0, 0));
        }

        public System.Drawing.Point GetMousePosition()
        {
            return Cursor.Position;
        }

        public void ScrollWheel(int scrollSize)
        {
            DoMouse(NativeMethods.MOUSEEVENTF.WHEEL, new System.Drawing.Point(0, 0));
        }
 
        public static void LeftClick(int x, int y)
        {
            DoMouse(NativeMethods.MOUSEEVENTF.MOVE | NativeMethods.MOUSEEVENTF.ABSOLUTE, new System.Drawing.Point(x, y));
            DoMouse(NativeMethods.MOUSEEVENTF.LEFTDOWN, new System.Drawing.Point(x, y));
            DoMouse(NativeMethods.MOUSEEVENTF.LEFTUP, new System.Drawing.Point(x, y));
        }  
        
        public static void RightClick(int x, int y)
        {
            DoMouse(NativeMethods.MOUSEEVENTF.MOVE | NativeMethods.MOUSEEVENTF.ABSOLUTE, new System.Drawing.Point(x, y));
            DoMouse(NativeMethods.MOUSEEVENTF.RIGHTDOWN, new System.Drawing.Point(x, y));
            DoMouse(NativeMethods.MOUSEEVENTF.RIGHTUP, new System.Drawing.Point(x, y));
        }      
        
        public static void ClickBoundingRectangleByPercentage(int xPercentage, int yPercentage, System.Drawing.Rectangle bounds)
        {
            double additional = 0.0;
            if (xPercentage == 99)
                additional = 0.5;
            int xPixel = Convert.ToInt32(bounds.Left + bounds.Width * (xPercentage + additional) / 100);
            int yPixel = Convert.ToInt32(bounds.Top + bounds.Height * (yPercentage) / 100);
            LeftClick(xPixel, yPixel);
        }

        private static void DoMouse(NativeMethods.MOUSEEVENTF flags, Point mouseCoords)
        {
            int scrollSize = 0;
            NativeMethods.INPUT input = new NativeMethods.INPUT();
            NativeMethods.MOUSEINPUT mi = new NativeMethods.MOUSEINPUT();
            input.dwType = NativeMethods.InputType.Mouse;
            input.mi = mi;
            input.mi.dwExtraInfo = IntPtr.Zero;
            input.mi.dx = mouseCoords.X;
            input.mi.dy = mouseCoords.Y;
            input.mi.time = 0;
            input.mi.mouseData = scrollSize * 120;
            // can be used for WHEEL event see msdn
            input.mi.dwFlags = flags;
            int cbSize = Marshal.SizeOf(typeof(NativeMethods.INPUT));
            int result = NativeMethods.SendInput(1, ref input, cbSize);
            if (result == 0)
                Debug.WriteLine("[SendInput Result 0] Error info: " + Marshal.GetLastWin32Error());
        }       
    }
}
