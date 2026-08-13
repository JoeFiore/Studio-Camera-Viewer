// #define TESTING_MODE
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace StudioCameraViewer
{
    public partial class MainWindow : Window
    {
        private List<string> availableNdiSources = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeStudioSetup();
        }

        private void InitializeStudioSetup()
        {
            PopulateMonitors();
            DiscoverNdiFeeds();
        }

        private void PopulateMonitors()
        {
            var screens = Screen.AllScreens;
            var monitorList = new List<string>();

            for (int i = 0; i < screens.Length; i++)
            {
                var s = screens[i];
                string primaryLabel = s.Primary ? " (Primary)" : "";
                monitorList.Add($"Display {i + 1}: {s.DeviceName.Replace(@"\.\", "")}{primaryLabel} [{s.Bounds.Width}x{s.Bounds.Height}]");
            }

            ComboDisplayA.ItemsSource = null;
            ComboDisplayA.ItemsSource = monitorList;

            ComboDisplayB.ItemsSource = null;
            ComboDisplayB.ItemsSource = monitorList;

            ComboMultiCamDisplay.ItemsSource = null;
            ComboMultiCamDisplay.ItemsSource = monitorList;

            if (monitorList.Count > 0)
            {
                ComboDisplayA.SelectedIndex = 0;
                ComboDisplayB.SelectedIndex = monitorList.Count > 1 ? 1 : 0;
                ComboMultiCamDisplay.SelectedIndex = monitorList.Count > 1 ? 1 : 0;
            }
        }

        private void DiscoverNdiFeeds()
        {
            availableNdiSources.Clear();

#if TESTING_MODE
            availableNdiSources = new List<string>
            {
                "Cam 1",
                "Cam 2",
                "Cam 3",
                "Cam 4",
                "Test Pattern"
            };
#else
            try
            {
                List<string> discoveredSources = ScanNetworkForNdiSources();

                if (discoveredSources != null && discoveredSources.Count > 0)
                {
                    availableNdiSources = discoveredSources;
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        "No live NDI camera sources or hardware video feeds were detected on the studio network.\n\n" +
                        "If you are testing the software without hardware connected, please launch the Testing App build instead.",
                        "No Live Hardware Detected",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );

                    availableNdiSources.Add("No Active Feeds Found");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Hardware Discovery Error: {ex.Message}\n\nPlease verify network connection or use Testing Mode.",
                    "Hardware Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                availableNdiSources.Add("No Active Feeds Found");
            }
#endif

            ComboStreamA.ItemsSource = null;
            ComboStreamA.ItemsSource = availableNdiSources;

            ComboStreamB.ItemsSource = null;
            ComboStreamB.ItemsSource = availableNdiSources;

            ComboMultiCam1.ItemsSource = null;
            ComboMultiCam1.ItemsSource = availableNdiSources;

            ComboMultiCam2.ItemsSource = null;
            ComboMultiCam2.ItemsSource = availableNdiSources;

            ComboMultiCam3.ItemsSource = null;
            ComboMultiCam3.ItemsSource = availableNdiSources;

            ComboMultiCam4.ItemsSource = null;
            ComboMultiCam4.ItemsSource = availableNdiSources;

            if (availableNdiSources.Count > 0)
            {
                ComboStreamA.SelectedIndex = 0;
                ComboStreamB.SelectedIndex = availableNdiSources.Count > 1 ? 1 : 0;

                ComboMultiCam1.SelectedIndex = 0;
                ComboMultiCam2.SelectedIndex = availableNdiSources.Count > 1 ? 1 : 0;
                ComboMultiCam3.SelectedIndex = availableNdiSources.Count > 2 ? 2 : 0;
                ComboMultiCam4.SelectedIndex = availableNdiSources.Count > 3 ? 3 : 0;
            }
        }

        private List<string> ScanNetworkForNdiSources()
        {
            return new List<string>();
        }

        private void PositionWindowOnScreen(Window window, int screenIndex)
        {
            var screens = Screen.AllScreens;
            if (screenIndex >= 0 && screenIndex < screens.Length)
            {
                var targetScreen = screens[screenIndex];

                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.WindowState = WindowState.Normal;

                window.Left = targetScreen.Bounds.Left;
                window.Top = targetScreen.Bounds.Top;
                window.Width = targetScreen.Bounds.Width;
                window.Height = targetScreen.Bounds.Height;

                window.Show();
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.Show();
            }
        }

        private void BtnOpenOverheadA_Click(object sender, RoutedEventArgs e)
        {
            string selectedCam = ComboStreamA.SelectedItem?.ToString() ?? "Cam 1";
            OverheadViewerWindow win = new OverheadViewerWindow(selectedCam);
            PositionWindowOnScreen(win, ComboDisplayA.SelectedIndex);
        }

        private void BtnOpenOverheadB_Click(object sender, RoutedEventArgs e)
        {
            string selectedCam = ComboStreamB.SelectedItem?.ToString() ?? "Cam 2";
            OverheadViewerWindow win = new OverheadViewerWindow(selectedCam);
            PositionWindowOnScreen(win, ComboDisplayB.SelectedIndex);
        }

        private void BtnOpenBothOverhead_Click(object sender, RoutedEventArgs e)
        {
            BtnOpenOverheadA_Click(sender, e);
            BtnOpenOverheadB_Click(sender, e);
        }

        private void ComboMultiCamPreset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridMultiCamLayout == null) return;

            int presetIndex = ComboMultiCamPreset.SelectedIndex;

            if (presetIndex == 0) // 4-Camera Grid
            {
                GridMultiCamLayout.Columns = 2;
                GridMultiCamLayout.Rows = 2;
                BoxQuad1.Visibility = Visibility.Visible;
                BoxQuad2.Visibility = Visibility.Visible;
                BoxQuad3.Visibility = Visibility.Visible;
                BoxQuad4.Visibility = Visibility.Visible;
            }
            else if (presetIndex == 1) // 2-Camera Side-by-Side
            {
                GridMultiCamLayout.Columns = 2;
                GridMultiCamLayout.Rows = 1;
                BoxQuad1.Visibility = Visibility.Visible;
                BoxQuad2.Visibility = Visibility.Visible;
                BoxQuad3.Visibility = Visibility.Collapsed;
                BoxQuad4.Visibility = Visibility.Collapsed;
            }
            else if (presetIndex == 2) // Single Camera Focus
            {
                GridMultiCamLayout.Columns = 1;
                GridMultiCamLayout.Rows = 1;
                BoxQuad1.Visibility = Visibility.Visible;
                BoxQuad2.Visibility = Visibility.Collapsed;
                BoxQuad3.Visibility = Visibility.Collapsed;
                BoxQuad4.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnOpenMulticam_Click(object sender, RoutedEventArgs e)
        {
            string c1 = ComboMultiCam1.SelectedItem?.ToString() ?? "Cam 1";
            string c2 = ComboMultiCam2.SelectedItem?.ToString() ?? "Cam 2";
            string c3 = ComboMultiCam3.SelectedItem?.ToString() ?? "Cam 3";
            string c4 = ComboMultiCam4.SelectedItem?.ToString() ?? "Cam 4";

            int preset = ComboMultiCamPreset.SelectedIndex;

            MultiCamWindow win = new MultiCamWindow(c1, c2, c3, c4, preset);
            PositionWindowOnScreen(win, ComboMultiCamDisplay.SelectedIndex);
        }

        private void BtnRefreshFeeds_Click(object sender, RoutedEventArgs e)
        {
            InitializeStudioSetup();
        }
    }
}