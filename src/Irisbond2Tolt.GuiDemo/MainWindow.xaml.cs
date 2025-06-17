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
using System.Windows.Forms;
using IrisbondAPI;

namespace Irisbond2Tolt.GuiDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource? _gazeCts;
    private IrisbondApi _irisbond; // Shared instance
    private bool _eyeGazeMouseEnabled = true;

    public MainWindow()
    {
        _gazeCts = null;
        _irisbond = new IrisbondApi();
        GenerateBanner.CreateBanner();
        InitializeComponent();
        this.KeyDown += MainWindow_KeyDown;
        this.PreviewKeyDown += MainWindow_KeyDown;
        UpdateTitle();
    }

    private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.E && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
        {
            _eyeGazeMouseEnabled = !_eyeGazeMouseEnabled;
            UpdateTitle();
            e.Handled = true;
        }
    }

    private void UpdateTitle()
    {
        this.Title = _eyeGazeMouseEnabled ? "Irisbond2Tolt GUI Demo (Eye Gaze Mouse: ON)" : "Irisbond2Tolt GUI Demo (Eye Gaze Mouse: OFF)";
    }

    private void TestIrisbondButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine($"IrisbondApi IsConnected (before): {_irisbond.IsConnected}");
            if (!_irisbond.IsConnected)
            {
                sb.AppendLine($"IrisbondApi Connect(): {_irisbond.Connect()}");
            }
            else
            {
                sb.AppendLine("IrisbondApi already connected.");
            }
            sb.AppendLine($"IrisbondApi IsConnected (after): {_irisbond.IsConnected}");
            sb.AppendLine($"IrisbondApi Calibrate: {_irisbond.Calibrate()}");
            var gaze = _irisbond.GetGazeData();
            sb.AppendLine($"Gaze Data: X={gaze.X}, Y={gaze.Y}, Timestamp={gaze.Timestamp}");
            OutputText.Text = sb.ToString();
        }
        catch (Exception ex)
        {
            OutputText.Text = $"Exception: {ex.Message}\n{ex.StackTrace}";
        }
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
        _irisbond.Connect();
        LiveGazeStream.Text = "";
        Task.Run(() =>
        {
            while (!_gazeCts.IsCancellationRequested)
            {
                var gaze = _irisbond.GetGazeData();
                var line = $"Gaze Data: X={gaze.X}, Y={gaze.Y}, Timestamp={gaze.Timestamp:O}\n";
                Dispatcher.Invoke(() =>
                {
                    LiveGazeStream.Text += line;
                    if (LiveGazeStream.Text.Length > 5000)
                        LiveGazeStream.Text = LiveGazeStream.Text.Substring(LiveGazeStream.Text.Length - 5000);
                    // Move mouse pointer to gaze location if enabled
                    if (_eyeGazeMouseEnabled)
                    {
                        int screenX = (int)gaze.X;
                        int screenY = (int)gaze.Y;
                        WinAPI.SetCursorPos(screenX, screenY);
                    }
                });
                Thread.Sleep(50);
            }
            _irisbond.Disconnect();
        }, _gazeCts.Token);
    }

    private void StopGazeStreamButton_Click(object sender, RoutedEventArgs e)
    {
        _gazeCts?.Cancel();
        _gazeCts = null;
    }
}