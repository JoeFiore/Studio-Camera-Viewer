using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StudioCameraViewer
{
    public partial class MultiCamWindow : Window
    {
        private DispatcherTimer? overlayTimer;

        public MultiCamWindow(string cam1, string cam2, string cam3, string cam4, int presetIndex)
        {
            InitializeComponent();
            ApplyLayoutPreset(presetIndex);
            SetupOverlayTimer();
        }

        private void ApplyLayoutPreset(int presetIndex)
        {
            if (MainMultiCamGrid == null) return;

            if (presetIndex == 0) // 4-Camera Grid (2x2)
            {
                MainMultiCamGrid.Columns = 2;
                MainMultiCamGrid.Rows = 2;

                if (Quad1Frame != null) Quad1Frame.Visibility = Visibility.Visible;
                if (Quad2Frame != null) Quad2Frame.Visibility = Visibility.Visible;
                if (Quad3Frame != null) Quad3Frame.Visibility = Visibility.Visible;
                if (Quad4Frame != null) Quad4Frame.Visibility = Visibility.Visible;
            }
            else if (presetIndex == 1) // 2-Camera Side-by-Side (Full 1080p)
            {
                MainMultiCamGrid.Columns = 2;
                MainMultiCamGrid.Rows = 1;

                if (Quad1Frame != null) Quad1Frame.Visibility = Visibility.Visible;
                if (Quad2Frame != null) Quad2Frame.Visibility = Visibility.Visible;
                if (Quad3Frame != null) Quad3Frame.Visibility = Visibility.Collapsed;
                if (Quad4Frame != null) Quad4Frame.Visibility = Visibility.Collapsed;
            }
            else if (presetIndex == 2) // Single Camera Focus
            {
                MainMultiCamGrid.Columns = 1;
                MainMultiCamGrid.Rows = 1;

                if (Quad1Frame != null) Quad1Frame.Visibility = Visibility.Visible;
                if (Quad2Frame != null) Quad2Frame.Visibility = Visibility.Collapsed;
                if (Quad3Frame != null) Quad3Frame.Visibility = Visibility.Collapsed;
                if (Quad4Frame != null) Quad4Frame.Visibility = Visibility.Collapsed;
            }
        }

        private void SetupOverlayTimer()
        {
            overlayTimer = new DispatcherTimer();
            overlayTimer.Interval = TimeSpan.FromSeconds(3);
            overlayTimer.Tick += OverlayTimer_Tick;
            overlayTimer.Start();
        }

        private void OverlayTimer_Tick(object? sender, EventArgs e)
        {
            if (OverlayInfoText != null)
            {
                OverlayInfoText.Visibility = Visibility.Collapsed;
            }
            overlayTimer?.Stop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}