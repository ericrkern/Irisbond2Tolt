using System;

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
    /// Basic implementation of the Irisbond Hiro API interface.
    /// </summary>
    public class IrisbondApi : IIrisbondApi
    {
        private bool connected = false;

        public bool Connect()
        {
            // TODO: Implement connection logic to Irisbond Hiro device
            connected = true;
            return connected;
        }

        public void Disconnect()
        {
            // TODO: Implement disconnection logic
            connected = false;
        }

        public bool Calibrate()
        {
            // TODO: Implement calibration logic
            return true;
        }

        public GazeData GetGazeData()
        {
            // TODO: Implement gaze data retrieval logic
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