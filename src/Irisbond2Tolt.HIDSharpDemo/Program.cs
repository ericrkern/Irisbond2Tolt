using HidSharp.Experimental;
using System;
using System.Threading;
using Irisbond2Tolt;

class Program
{
    static void Main()
    {
        // Mouse report descriptor (change as needed for other devices)
        byte[] reportDescriptor = new byte[]
        {
            0x05, 0x01, // Usage Page (Generic Desktop)
            0x09, 0x02, // Usage (Mouse)
            0xA1, 0x01, // Collection (Application)
            0x09, 0x01, //   Usage (Pointer)
            0xA1, 0x00, //   Collection (Physical)
            0x05, 0x09, //     Usage Page (Button)
            0x19, 0x01, //     Usage Minimum (1)
            0x29, 0x03, //     Usage Maximum (3)
            0x15, 0x00, //     Logical Minimum (0)
            0x25, 0x01, //     Logical Maximum (1)
            0x95, 0x03, //     Report Count (3)
            0x75, 0x01, //     Report Size (1)
            0x81, 0x02, //     Input (Data, Variable, Absolute)
            0x95, 0x01, //     Report Count (1)
            0x75, 0x05, //     Report Size (5)
            0x81, 0x03, //     Input (Constant)
            0x05, 0x01, //     Usage Page (Generic Desktop)
            0x09, 0x30, //     Usage (X)
            0x09, 0x31, //     Usage (Y)
            0x15, 0x81, //     Logical Minimum (-127)
            0x25, 0x7F, //     Logical Maximum (127)
            0x75, 0x08, //     Report Size (8)
            0x95, 0x02, //     Report Count (2)
            0x81, 0x06, //     Input (Data, Variable, Relative)
            0xC0,       //   End Collection
            0xC0        // End Collection
        };

        var device = DeviceFactory.CreateHidDevice(
            vendorID: 0x1234, // Example vendor ID
            productID: 0x5678, // Example product ID
            reportDescriptor: reportDescriptor
        );

        var irisbond = new IrisbondApi();
        if (!irisbond.Connect())
        {
            Console.WriteLine("Failed to connect to Irisbond device.");
            return;
        }

        device.Open();
        Console.WriteLine("Virtual HID device running with live gaze data. Press Ctrl+C to exit.");

        // Assume 1920x1080 screen, center at (960,540)
        double lastX = 960, lastY = 540;
        while (true)
        {
            var gaze = irisbond.GetGazeData();
            // Calculate relative movement from last position
            int dx = (int)((gaze.X - lastX) / 5.0); // scale down for smoothness
            int dy = (int)((gaze.Y - lastY) / 5.0);
            // Clamp to HID limits
            dx = Math.Max(-127, Math.Min(127, dx));
            dy = Math.Max(-127, Math.Min(127, dy));
            // Send report only if there is movement
            if (dx != 0 || dy != 0)
            {
                byte[] report = new byte[] { 0, (byte)dx, (byte)dy };
                device.Write(report, 0, report.Length);
                lastX += dx * 5.0;
                lastY += dy * 5.0;
            }
            Thread.Sleep(20); // 50 Hz update
        }
    }
}
