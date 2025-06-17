using Irisbond2Tolt;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Irisbond2Tolt.GuiDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource? _gazeCts;

    public MainWindow()
    {
        _gazeCts = null;
        GenerateBanner.CreateBanner();
        InitializeComponent();
    }

    private void TestIrisbondButton_Click(object sender, RoutedEventArgs e)
    {
        var sb = new StringBuilder();
        var irisbond = new IrisbondApi();
        sb.AppendLine($"IrisbondApi Connected: {irisbond.Connect()}");
        sb.AppendLine($"IrisbondApi IsConnected: {irisbond.IsConnected}");
        sb.AppendLine($"IrisbondApi Calibrate: {irisbond.Calibrate()}");
        var gaze = irisbond.GetGazeData();
        sb.AppendLine($"Gaze Data: X={gaze.X}, Y={gaze.Y}, Timestamp={gaze.Timestamp}");
        irisbond.Disconnect();
        sb.AppendLine($"IrisbondApi IsConnected after disconnect: {irisbond.IsConnected}");
        OutputText.Text = sb.ToString();
    }

    private void TestToltButton_Click(object sender, RoutedEventArgs e)
    {
        var sb = new StringBuilder();
        var tolt = new ToltAbilityDriveApi();
        sb.AppendLine($"ToltAbilityDriveApi Connected: {tolt.Connect()}");
        sb.AppendLine($"ToltAbilityDriveApi IsConnected: {tolt.IsConnected}");
        sb.AppendLine($"Set Drive Mode to Outdoor: {tolt.SetDriveMode(DriveMode.Outdoor)}");
        var command = new DriveCommand { Forward = 1.0, Turn = 0.5, Speed = 0.8 };
        sb.AppendLine($"Send Drive Command: {tolt.SendDriveCommand(command)}");
        sb.AppendLine($"Set Seating Position to 2: {tolt.SetSeatingPosition(2)}");
        var status = tolt.GetStatus();
        sb.AppendLine($"Drive Status: Mode={status.CurrentMode}, IsMoving={status.IsMoving}, Battery={status.BatteryLevel}, Error={status.Error}");
        tolt.Disconnect();
        sb.AppendLine($"ToltAbilityDriveApi IsConnected after disconnect: {tolt.IsConnected}");
        OutputText.Text = sb.ToString();
    }

    private void StartGazeStreamButton_Click(object sender, RoutedEventArgs e)
    {
        if (_gazeCts != null)
            return;
        _gazeCts = new CancellationTokenSource();
        var irisbond = new IrisbondApi();
        irisbond.Connect();
        LiveGazeStream.Text = "";
        Task.Run(() =>
        {
            while (!_gazeCts.IsCancellationRequested)
            {
                var gaze = irisbond.GetGazeData();
                var line = $"Gaze Data: X={gaze.X}, Y={gaze.Y}, Timestamp={gaze.Timestamp:O}\n";
                Dispatcher.Invoke(() =>
                {
                    LiveGazeStream.Text += line;
                    if (LiveGazeStream.Text.Length > 5000)
                        LiveGazeStream.Text = LiveGazeStream.Text.Substring(LiveGazeStream.Text.Length - 5000);
                });
                Thread.Sleep(50);
            }
            irisbond.Disconnect();
        }, _gazeCts.Token);
    }

    private void StopGazeStreamButton_Click(object sender, RoutedEventArgs e)
    {
        _gazeCts?.Cancel();
        _gazeCts = null;
    }
}