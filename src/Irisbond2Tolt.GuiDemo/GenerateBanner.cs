using System.Drawing;
using System.Windows;

namespace Irisbond2Tolt.GuiDemo
{
    public static class GenerateBanner
    {
        public static void CreateBanner()
        {
            try
            {
                int width = 600, height = 200;
                using var bmp = new Bitmap(width, height);
                using var g = Graphics.FromImage(bmp);
                g.Clear(Color.White);

                // Use the output directory for the logo
                var logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScottMorganFoundationLogo.png");
                if (!System.IO.File.Exists(logoPath))
                {
                    MessageBox.Show($"ScottMorganFoundationLogo.png not found at {logoPath}. Please add the logo to the output directory.", "Image Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var logo = Image.FromFile(logoPath);
                g.DrawImage(logo, 20, 20, 120, 120);

                // Draw the text
                var font = new System.Drawing.Font("Segoe UI", 24, System.Drawing.FontStyle.Bold);
                var brush = new SolidBrush(Color.Black);
                g.DrawString("Powered by The Scott-Morgan Foundation", font, brush, 160, 70);

                bmp.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SplashBanner.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating banner: {ex.Message}", "Banner Generation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 