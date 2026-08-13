using System;
using System.Windows;
using System.Windows.Media.Animation;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Key = System.Windows.Input.Key;

namespace StudioCameraViewer
{
    public partial class OverheadViewerWindow : Window
    {
        private string? _selectedCamera;

        public OverheadViewerWindow(string? cameraName)
        {
            InitializeComponent();
            _selectedCamera = cameraName;
            TxtStatus.Text = $"Feed: {cameraName ?? "Default Stream"}\nPress ESC to return to Control Panel";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start a 1-second fade out animation after a 3-second delay
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromSeconds(3),
                Duration = TimeSpan.FromSeconds(1)
            };

            TxtStatus.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}