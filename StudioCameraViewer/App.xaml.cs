using System.Runtime.InteropServices;
using System.Windows;

namespace StudioCameraViewer
{
    public partial class App : Application
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern void SetCurrentProcessExplicitAppUserModelID(
            [MarshalAs(UnmanagedType.LPWStr)] string AppID);

        protected override void OnStartup(StartupEventArgs e)
        {
            SetCurrentProcessExplicitAppUserModelID("OnTheProwl.StudioControlPanel.v1");
            base.OnStartup(e);
        }
    }
}