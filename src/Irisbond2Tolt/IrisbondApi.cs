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
        private GazeData _latestGazeData = new GazeData();
        private bool _callbackSet = false;

        public bool Connect()
        {
            // Check if the tracker hardware is present
            if (!IrisbondHiru.trackerIsPresent())
                return false;
            // Check if the tracker is reachable (0 = OK)
            int connStatus = IrisbondHiru.checkTrackerConnection();
            if (connStatus != 0)
                return false;
            // Only call start if not already started
            if (!connected)
            {
                var status = IrisbondHiru.start();
                connected = (status == START_STATUS.START_OK);
            }
            else
            {
                connected = true;
            }
            // Set up the gaze data callback once
            if (!_callbackSet)
            {
                IrisbondHiru.setDataCallback(OnGazeData);
                _callbackSet = true;
            }
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
            return _latestGazeData;
        }

        public bool IsConnected => connected;

        // Callback for real gaze data
        private void OnGazeData(
            long timestamp,
            float mouseX,
            float mouseY,
            float mouseRawX,
            float mouseRawY,
            int screenWidth,
            int screenHeight,
            bool leftEyeDetected,
            bool rightEyeDetected,
            int imageWidth,
            int imageHeight,
            float leftEyeX,
            float leftEyeY,
            float leftEyeSize,
            float rightEyeX,
            float rightEyeY,
            float rightEyeSize,
            float distanceFactor)
        {
            _latestGazeData = new GazeData
            {
                X = mouseX,
                Y = mouseY,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime
            };
        }
    }
} 