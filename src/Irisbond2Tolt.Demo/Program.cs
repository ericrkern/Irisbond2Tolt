using Irisbond2Tolt;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Testing Irisbond2Tolt APIs\n");

        // Test IrisbondApi
        var irisbond = new IrisbondApi();
        Console.WriteLine($"IrisbondApi Connected: {irisbond.Connect()}");
        Console.WriteLine($"IrisbondApi IsConnected: {irisbond.IsConnected}");
        Console.WriteLine($"IrisbondApi Calibrate: {irisbond.Calibrate()}");
        Console.WriteLine("Streaming live gaze data. Press any key to stop...");
        while (!Console.KeyAvailable)
        {
            var gaze = irisbond.GetGazeData();
            Console.WriteLine($"Gaze Data: X={gaze.X}, Y={gaze.Y}, Timestamp={gaze.Timestamp:O}");
            System.Threading.Thread.Sleep(50);
        }
        irisbond.Disconnect();
        Console.WriteLine($"IrisbondApi IsConnected after disconnect: {irisbond.IsConnected}\n");

        // Test ToltAbilityDriveApi
        var tolt = new ToltAbilityDriveApi();
        Console.WriteLine($"ToltAbilityDriveApi Connected: {tolt.Connect()}");
        Console.WriteLine($"ToltAbilityDriveApi IsConnected: {tolt.IsConnected}");
        Console.WriteLine($"Set Drive Mode to Outdoor: {tolt.SetDriveMode(DriveMode.Outdoor)}");
        var command = new DriveCommand { Forward = 1.0, Turn = 0.5, Speed = 0.8 };
        Console.WriteLine($"Send Drive Command: {tolt.SendDriveCommand(command)}");
        Console.WriteLine($"Set Seating Position to 2: {tolt.SetSeatingPosition(2)}");
        var status = tolt.GetStatus();
        Console.WriteLine($"Drive Status: Mode={status.CurrentMode}, IsMoving={status.IsMoving}, Battery={status.BatteryLevel}, Error={status.Error}");
        tolt.Disconnect();
        Console.WriteLine($"ToltAbilityDriveApi IsConnected after disconnect: {tolt.IsConnected}");
    }
}
