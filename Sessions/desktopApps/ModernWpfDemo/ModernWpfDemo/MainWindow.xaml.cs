using Microsoft.Toolkit.Wpf.UI.XamlHost;
using System;
using Windows.Devices.Geolocation;

namespace ModernWpfDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator() { DesiredAccuracyInMeters = 5 };
            Geoposition pos = await geolocator.GetGeopositionAsync();
            string longitude = pos.Coordinate.Point.Position.Longitude.ToString();
            string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
            LongBlock.Text = longitude.Substring(0, longitude.IndexOf('.') + 3);
            LatBlock.Text = latitude.Substring(0, latitude.IndexOf('.') + 3);

            await TheMap.TrySetViewAsync(pos.Coordinate.Point, 15.0);
        }

        private void WindowsXamlHost_ChildChanged(object sender, EventArgs e)
        {
            if (sender is WindowsXamlHost windowsXamlHost &&
                windowsXamlHost.Child is Windows.UI.Xaml.Controls.TextBox textBox)
            {
                textBox.FontSize = 24;
                textBox.Text = ".NET Core + Win10 + (WPF || WinForms) == ❤ 🐱‍🐉";
            }
        }
    }
}
