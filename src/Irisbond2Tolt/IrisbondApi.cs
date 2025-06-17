using System;
using IrisbondAPI; // Reference to the official Irisbond C# API wrapper

namespace Irisbond2Tolt
{
    /// <summary>
    /// Interface for interacting with the Irisbond Hiro eye tracker.
    /// </summary>
    public interface IIrisbondApi
    {
        bool Connect();
        void Disconnect();
        bool Calibrate();
        GazeData GetGazeData();
        bool IsConnected { get; }
    }

    /// <summary>
    /// Represents gaze data from the eye tracker.
    /// </summary>
    public class GazeData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Implementation of the Irisbond Hiro API interface using the official SDK.
    /// Requires IrisbondHiruAPI.dll and PGRFlyCapture.dll to be present in the output directory.
    /// </summary>
    public class IrisbondApi : IIrisbondApi
    {
        private bool connected = false;

        public bool Connect()
        {
            // Check if the tracker is present and start the API
            if (!IrisbondHiru.trackerIsPresent())
                return false;
            var status = IrisbondHiru.start();
            connected = (status == START_STATUS.START_OK);
            return connected;
        }

        public void Disconnect()
        {
            if (connected)
            {
                IrisbondHiru.stop();
                connected = false;
            }
        }

        public bool Calibrate()
        {
            if (!connected)
                return false;
            // Start a 5-point calibration and wait for it to finish (timeout 2 minutes)
            IrisbondHiru.startCalibration(5);
            var result = CALIBRATION_STATUS.CALIBRATION_FINISHED;
            try
            {
                result = IrisbondHiru.waitForCalibrationToEnd(2);
            }
            catch { return false; }
            return result == CALIBRATION_STATUS.CALIBRATION_FINISHED;
        }

        public GazeData GetGazeData()
        {
            // This is a placeholder. Actual gaze data requires setting up a callback in the SDK.
            // For a real implementation, you would subscribe to the data callback and update a field.
            return new GazeData
            {
                X = 0.0,
                Y = 0.0,
                Timestamp = DateTime.Now
            };
        }

        public bool IsConnected => connected;
    }
} 