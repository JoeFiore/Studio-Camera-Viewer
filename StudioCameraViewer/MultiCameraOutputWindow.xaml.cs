using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Key = System.Windows.Input.Key;

namespace StudioCameraViewer
{
    public partial class MultiCameraOutputWindow : Window
    {
        private List<string>? _selectedCameras;
        private List<Image> _cameraImages = new List<Image>();
        private List<TextBlock> _cameraLabels = new List<TextBlock>();

        public MultiCameraOutputWindow(List<string>? selectedCameras)
        {
            InitializeComponent();
            _selectedCameras = selectedCameras ?? new List<string>();
            BuildCameraGrid();
            UpdateCameraLabels();
        }

        private void BuildCameraGrid()
        {
            CameraGrid.Children.Clear();
            _cameraImages.Clear();
            _cameraLabels.Clear();

            int cameraCount = _selectedCameras?.Count ?? 0;

            if (cameraCount == 0)
            {
                TxtStatusOverlay.Visibility = Visibility.Visible;
                return;
            }

            TxtStatusOverlay.Visibility = Visibility.Hidden;

            // Clear and configure grid
            CameraGrid.RowDefinitions.Clear();
            CameraGrid.ColumnDefinitions.Clear();

            int rows, cols;

            // Determine grid layout based on camera count
            if (cameraCount == 1)
            {
                rows = 1;
                cols = 1;
            }
            else if (cameraCount == 2)
            {
                rows = 1;
                cols = 2;
            }
            else if (cameraCount == 3)
            {
                rows = 2;
                cols = 2;
            }
            else // 4 cameras
            {
                rows = 2;
                cols = 2;
            }

            // Create grid definitions
            for (int i = 0; i < rows; i++)
            {
                CameraGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < cols; i++)
            {
                CameraGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Add camera feeds
            for (int i = 0; i < cameraCount; i++)
            {
                int row = i / cols;
                int col = i % cols;

                // Create container border
                Border cameraBorder = new Border
                {
                    Background = Brushes.Black,
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51)),
                    BorderThickness = new Thickness(2)
                };

                Grid.SetRow(cameraBorder, row);
                Grid.SetColumn(cameraBorder, col);

                // Create grid inside border for image and label
                Grid cameraContainer = new Grid();

                // Add image
                Image cameraImage = new Image
                {
                    Stretch = Stretch.Uniform,
                    Name = $"ImgCamera{i + 1}"
                };
                cameraContainer.Children.Add(cameraImage);
                _cameraImages.Add(cameraImage);

                // Add label
                TextBlock cameraLabel = new TextBlock
                {
                    Foreground = Brushes.White,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Name = $"TxtCamera{i + 1}"
                };
                cameraContainer.Children.Add(cameraLabel);
                _cameraLabels.Add(cameraLabel);

                cameraBorder.Child = cameraContainer;
                CameraGrid.Children.Add(cameraBorder);
            }
        }

        private void UpdateCameraLabels()
        {
            for (int i = 0; i < _cameraLabels.Count; i++)
            {
                if (i < _selectedCameras?.Count)
                {
                    _cameraLabels[i].Text = _selectedCameras[i];
                    _cameraLabels[i].Opacity = 0; // Hide after fade
                }
                else
                {
                    _cameraLabels[i].Text = "No Feed";
                    _cameraLabels[i].Opacity = 1;
                }
            }

            // Update status overlay
            bool hasFeeds = (_selectedCameras?.Count ?? 0) > 0;
            TxtStatusOverlay.Visibility = hasFeeds ? Visibility.Hidden : Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade out camera labels after 3 seconds
            if (_cameraLabels.Count > 0)
            {
                foreach (var label in _cameraLabels)
                {
                    if (label.Opacity > 0)
                    {
                        DoubleAnimation fadeAnimation = new DoubleAnimation
                        {
                            From = 1.0,
                            To = 0.0,
                            BeginTime = TimeSpan.FromSeconds(3),
                            Duration = TimeSpan.FromSeconds(1)
                        };
                        label.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Press ESC to close and return to control panel
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Update the displayed cameras and rebuild grid
        /// </summary>
        public void UpdateCameras(List<string>? newCameras)
        {
            _selectedCameras = newCameras ?? new List<string>();
            BuildCameraGrid();
            UpdateCameraLabels();
        }
    }
}
