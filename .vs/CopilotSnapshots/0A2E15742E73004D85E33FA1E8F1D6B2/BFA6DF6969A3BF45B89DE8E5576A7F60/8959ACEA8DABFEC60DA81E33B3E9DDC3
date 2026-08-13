using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;
using Screen = System.Windows.Forms.Screen;

namespace StudioCameraViewer
{
    public partial class MainWindow : Window
    {
        private List<string> _discoveredNdiSources = new List<string>();
        private List<OverheadViewerWindow> _activeViewerWindows = new List<OverheadViewerWindow>();
        private MultiCameraOutputWindow? _activeOutputWindow;

        public MainWindow()
        {
            InitializeComponent();
            LoadDisplays();
            ScanNdiFeeds();
        }

        private void LoadDisplays()
        {
            CmbDisplay1.Items.Clear();
            CmbDisplay2.Items.Clear();

            // Also populate multi-camera display combo
            if (CmbMultiCamDisplay != null)
            {
                CmbMultiCamDisplay.Items.Clear();
            }

            int index = 1;
            foreach (var screen in Screen.AllScreens)
            {
                string displayLabel = $"Display {index} ({screen.Bounds.Width}x{screen.Bounds.Height}) {(screen.Primary ? "[Primary]" : "")}";
                CmbDisplay1.Items.Add(displayLabel);
                CmbDisplay2.Items.Add(displayLabel);

                if (CmbMultiCamDisplay != null)
                {
                    CmbMultiCamDisplay.Items.Add(displayLabel);
                }

                index++;
            }

            if (CmbDisplay1.Items.Count > 1) CmbDisplay1.SelectedIndex = 1;
            if (CmbDisplay2.Items.Count > 2) CmbDisplay2.SelectedIndex = 2;
            else if (CmbDisplay2.Items.Count > 0) CmbDisplay2.SelectedIndex = 0;

            if (CmbMultiCamDisplay != null && CmbMultiCamDisplay.Items.Count > 0)
            {
                CmbMultiCamDisplay.SelectedIndex = 0;
            }
        }

        private void ScanNdiFeeds()
        {
            CmbCamera1.Items.Clear();
            CmbCamera2.Items.Clear();

            // Also populate multi-camera combo boxes
            if (CmbMultiCamera1 != null) CmbMultiCamera1.Items.Clear();
            if (CmbMultiCamera2 != null) CmbMultiCamera2.Items.Clear();
            if (CmbMultiCamera3 != null) CmbMultiCamera3.Items.Clear();
            if (CmbMultiCamera4 != null) CmbMultiCamera4.Items.Clear();

            _discoveredNdiSources.Clear();

            // Simulated test feeds for off-network development
            _discoveredNdiSources.Add("vMix - Camera 1 (TEST)");
            _discoveredNdiSources.Add("vMix - Camera 2 (TEST)");

            if (_discoveredNdiSources.Count == 0)
            {
                BannerWarning.Visibility = Visibility.Visible;
                TxtWarning.Text = "No active vMix NDI feeds found. Check the 'Camera Setup' tab for instructions.";
                CmbCamera1.Items.Add("No Feeds Detected");
                CmbCamera2.Items.Add("No Feeds Detected");
            }
            else
            {
                BannerWarning.Visibility = Visibility.Collapsed;

                // Add empty option first for multi-camera combos
                string emptyOption = "-- Select Camera --";
                if (CmbMultiCamera1 != null) CmbMultiCamera1.Items.Add(emptyOption);
                if (CmbMultiCamera2 != null) CmbMultiCamera2.Items.Add(emptyOption);
                if (CmbMultiCamera3 != null) CmbMultiCamera3.Items.Add(emptyOption);
                if (CmbMultiCamera4 != null) CmbMultiCamera4.Items.Add(emptyOption);

                foreach (var src in _discoveredNdiSources)
                {
                    CmbCamera1.Items.Add(src);
                    CmbCamera2.Items.Add(src);

                    // Add to multi-camera combos too
                    if (CmbMultiCamera1 != null) CmbMultiCamera1.Items.Add(src);
                    if (CmbMultiCamera2 != null) CmbMultiCamera2.Items.Add(src);
                    if (CmbMultiCamera3 != null) CmbMultiCamera3.Items.Add(src);
                    if (CmbMultiCamera4 != null) CmbMultiCamera4.Items.Add(src);
                }
                CmbCamera1.SelectedIndex = 0;
                if (_discoveredNdiSources.Count > 1) CmbCamera2.SelectedIndex = 1;

                // Set multi-camera combos to empty option
                if (CmbMultiCamera1 != null && CmbMultiCamera1.Items.Count > 0) CmbMultiCamera1.SelectedIndex = 0;
                if (CmbMultiCamera2 != null && CmbMultiCamera2.Items.Count > 0) CmbMultiCamera2.SelectedIndex = 0;
                if (CmbMultiCamera3 != null && CmbMultiCamera3.Items.Count > 0) CmbMultiCamera3.SelectedIndex = 0;
                if (CmbMultiCamera4 != null && CmbMultiCamera4.Items.Count > 0) CmbMultiCamera4.SelectedIndex = 0;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ScanNdiFeeds();
        }

        private void ChkMultiCamMode_Changed(object sender, RoutedEventArgs e)
        {
            // Handle multi-camera mode toggle
            // When checked, show multi-camera controls
            // When unchecked, show single camera controls
            if (sender is CheckBox checkBox && checkBox.IsChecked == true)
            {
                // Multi-camera mode enabled
                if (CmbMultiCamDisplay != null)
                {
                    CmbMultiCamDisplay.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // Single camera mode
                if (CmbMultiCamDisplay != null)
                {
                    CmbMultiCamDisplay.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void BtnSaveDefaults_Click(object sender, RoutedEventArgs e)
        {
            // WPF uses 'IsEnabled', not 'Enabled'
            BtnSaveDefaults.IsEnabled = false;

            try
            {
                // 1. Get the AppData folder path
                string appDataPath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "OnTheProwl"
                );

                System.IO.Directory.CreateDirectory(appDataPath);
                string filePath = System.IO.Path.Combine(appDataPath, "preset.json");

                // 2. Gather your actual UI values here (replace drop-down names if different)
                var settingsToSave = new
                {
                    MonitorA_Display = CmbDisplay1?.SelectedItem?.ToString(),
                    MonitorA_Stream = CmbCamera1?.SelectedItem?.ToString(),
                    MonitorB_Display = CmbDisplay2?.SelectedItem?.ToString(),
                    MonitorB_Stream = CmbCamera2?.SelectedItem?.ToString()
                };

                // 3. Save asynchronously using System.Text.Json with proper error handling
                await Task.Run(() =>
                {
                    try
                    {
                        string json = System.Text.Json.JsonSerializer.Serialize(settingsToSave);
                        System.IO.File.WriteAllText(filePath, json);
                    }
                    catch (Exception innerEx)
                    {
                        // Log to file for debugging Release builds
                        string logPath = System.IO.Path.Combine(appDataPath, "error.log");
                        string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error saving preset: {innerEx}";
                        System.IO.File.AppendAllText(logPath, logMessage + Environment.NewLine);
                        throw;
                    }
                });

                System.Windows.MessageBox.Show("Preset saved successfully!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to save preset: {ex.Message}");
            }
            finally
            {
                BtnSaveDefaults.IsEnabled = true;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            CloseActiveOverheads();

            int display1Idx = CmbDisplay1.SelectedIndex;
            int display2Idx = CmbDisplay2.SelectedIndex;

            if (display1Idx >= 0 && display1Idx < Screen.AllScreens.Length)
            {
                LaunchOverheadWindow(Screen.AllScreens[display1Idx], CmbCamera1.SelectedItem?.ToString());
            }

            if (display2Idx >= 0 && display2Idx < Screen.AllScreens.Length)
            {
                LaunchOverheadWindow(Screen.AllScreens[display2Idx], CmbCamera2.SelectedItem?.ToString());
            }

            // Launch multi-camera output if enabled
            if (ChkMultiCamMode?.IsChecked == true)
            {
                LaunchMultiCameraOutput();
            }

            // Hide the Control Panel once windows are launched
            if (_activeViewerWindows.Count > 0 || _activeOutputWindow != null)
            {
                this.Hide();
            }
        }

        private void LaunchMultiCameraOutput()
        {
            try
            {
                // Close existing output window if any
                CloseActiveOutputWindow();

                // Gather selected cameras
                List<string> selectedCameras = new List<string>();

                if (CmbMultiCamera1?.SelectedItem != null && !string.IsNullOrEmpty(CmbMultiCamera1.SelectedItem.ToString()))
                    selectedCameras.Add(CmbMultiCamera1.SelectedItem.ToString() ?? "");
                if (CmbMultiCamera2?.SelectedItem != null && !string.IsNullOrEmpty(CmbMultiCamera2.SelectedItem.ToString()))
                    selectedCameras.Add(CmbMultiCamera2.SelectedItem.ToString() ?? "");
                if (CmbMultiCamera3?.SelectedItem != null && !string.IsNullOrEmpty(CmbMultiCamera3.SelectedItem.ToString()))
                    selectedCameras.Add(CmbMultiCamera3.SelectedItem.ToString() ?? "");
                if (CmbMultiCamera4?.SelectedItem != null && !string.IsNullOrEmpty(CmbMultiCamera4.SelectedItem.ToString()))
                    selectedCameras.Add(CmbMultiCamera4.SelectedItem.ToString() ?? "");

                // Filter out empty selections
                selectedCameras.RemoveAll(s => string.IsNullOrWhiteSpace(s));

                if (selectedCameras.Count == 0)
                {
                    System.Windows.MessageBox.Show("Please select at least one camera for multi-camera mode.");
                    return;
                }

                // Get target display
                int displayIdx = CmbMultiCamDisplay?.SelectedIndex ?? 0;
                if (displayIdx < 0 || displayIdx >= Screen.AllScreens.Length) displayIdx = 0;

                Screen targetScreen = Screen.AllScreens[displayIdx];

                // Create and launch output window
                _activeOutputWindow = new MultiCameraOutputWindow(selectedCameras);
                _activeOutputWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                _activeOutputWindow.WindowState = WindowState.Normal;

                _activeOutputWindow.Left = targetScreen.Bounds.Left;
                _activeOutputWindow.Top = targetScreen.Bounds.Top;
                _activeOutputWindow.Width = targetScreen.Bounds.Width;
                _activeOutputWindow.Height = targetScreen.Bounds.Height;

                // Handle window close
                _activeOutputWindow.Closed += (s, closedArgs) =>
                {
                    _activeOutputWindow = null;
                };

                _activeOutputWindow.Show();
                _activeOutputWindow.WindowState = WindowState.Maximized;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error launching multi-camera output: {ex.Message}");
                _activeOutputWindow = null;
            }
        }

        private void LaunchOverheadWindow(Screen targetScreen, string? cameraName)
        {
            var viewer = new OverheadViewerWindow(cameraName ?? "vMix Camera");

            viewer.WindowStartupLocation = WindowStartupLocation.Manual;
            viewer.WindowState = WindowState.Normal;

            viewer.Left = targetScreen.Bounds.Left;
            viewer.Top = targetScreen.Bounds.Top;
            viewer.Width = targetScreen.Bounds.Width;
            viewer.Height = targetScreen.Bounds.Height;

            // Show the Control Panel again when all viewer windows are closed
            viewer.Closed += (s, e) =>
            {
                _activeViewerWindows.Remove(viewer);
                if (_activeViewerWindows.Count == 0)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                }
            };

            viewer.Show();
            viewer.WindowState = WindowState.Maximized;

            _activeViewerWindows.Add(viewer);
        }

        private void CloseActiveOverheads()
        {
            foreach (var win in _activeViewerWindows)
            {
                win.Close();
            }
            _activeViewerWindows.Clear();
        }

        private void CloseActiveOutputWindow()
        {
            if (_activeOutputWindow != null)
            {
                try
                {
                    _activeOutputWindow.Close();
                }
                catch { }
                finally
                {
                    _activeOutputWindow = null;
                }
            }
        }

        private void ChkMultiCamMode_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isMultiCamEnabled = ChkMultiCamMode?.IsChecked == true;

                // Show/hide multi-camera configuration section
                if (BorderMultiCam != null)
                {
                    BorderMultiCam.Visibility = isMultiCamEnabled ? Visibility.Visible : Visibility.Collapsed;
                }

                // Update button text and size
                if (BtnStart != null)
                {
                    if (isMultiCamEnabled)
                    {
                        BtnStart.Content = "Launch Overheads + Multicam";
                        BtnStart.Width = Double.NaN; // Auto-size
                    }
                    else
                    {
                        BtnStart.Content = "Launch Overheads";
                        BtnStart.Width = 170;
                    }
                }

                // Populate multi-camera display combo if showing
                if (isMultiCamEnabled && CmbMultiCamDisplay != null && CmbMultiCamDisplay.Items.Count == 0)
                {
                    int index = 1;
                    foreach (var screen in Screen.AllScreens)
                    {
                        string displayLabel = $"Display {index} ({screen.Bounds.Width}x{screen.Bounds.Height}) {(screen.Primary ? "[Primary]" : "")}";
                        CmbMultiCamDisplay.Items.Add(displayLabel);
                        index++;
                    }
                    if (CmbMultiCamDisplay.Items.Count > 0) CmbMultiCamDisplay.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error in CheckBox handler: {ex.Message}");
            }
        }
    }
}