using System;

namespace Irisbond2Tolt
{
    /// <summary>
    /// Interface for interacting with the Tolt Ability Drive system.
    /// </summary>
    public interface IToltAbilityDriveApi
    {
        bool Connect();
        void Disconnect();
        bool IsConnected { get; }
        bool SetDriveMode(DriveMode mode);
        bool SendDriveCommand(DriveCommand command);
        bool SetSeatingPosition(int position);
        DriveStatus GetStatus();
    }

    /// <summary>
    /// Enum for drive modes (e.g., Indoor, Outdoor, Seating, etc.)
    /// </summary>
    public enum DriveMode
    {
        Indoor,
        Outdoor,
        Seating,
        Programming
    }

    /// <summary>
    /// Represents a drive command (e.g., direction, speed).
    /// </summary>
    public class DriveCommand
    {
        public double Forward { get; set; }
        public double Turn { get; set; }
        public double Speed { get; set; }
    }

    /// <summary>
    /// Represents the current status of the drive system.
    /// </summary>
    public class DriveStatus
    {
        public DriveMode CurrentMode { get; set; }
        public bool IsMoving { get; set; }
        public int BatteryLevel { get; set; }
        public string? Error { get; set; }
    }

    /// <summary>
    /// Basic implementation of the Tolt Ability Drive API interface.
    /// </summary>
    public class ToltAbilityDriveApi : IToltAbilityDriveApi
    {
        private bool connected = false;

        public bool Connect()
        {
            // TODO: Implement connection logic to Tolt Ability Drive system
            connected = true;
            return connected;
        }

        public void Disconnect()
        {
            // TODO: Implement disconnection logic
            connected = false;
        }

        public bool IsConnected => connected;

        public bool SetDriveMode(DriveMode mode)
        {
            // TODO: Implement drive mode switching logic
            return true;
        }

        public bool SendDriveCommand(DriveCommand command)
        {
            // TODO: Implement drive command sending logic
            return true;
        }

        public bool SetSeatingPosition(int position)
        {
            // TODO: Implement seating position logic
            return true;
        }

        public DriveStatus GetStatus()
        {
            // TODO: Implement status retrieval logic
            return new DriveStatus
            {
                CurrentMode = DriveMode.Indoor,
                IsMoving = false,
                BatteryLevel = 100,
                Error = null
            };
        }
    }
} 