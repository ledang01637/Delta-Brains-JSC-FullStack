using CommunityToolkit.WinUI.Notifications;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace DeltaBrainsJSCAppFE.Notification
{
    public class SendToastNotification
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static void SendNotification(NotificationRes? notificationRes = default)
        {
            if (notificationRes != null)
                    ShowToastNotification(notificationRes.RelatedTaskId.ToString());
            else
                ShowToastNotification();

            Callback();
        }

        private static void ShowToastNotification(string taskId = null)
        {
            new ToastContentBuilder()
                .AddArgument("action", "openTask")
                .AddArgument("taskId", taskId)
                .AddText("🔔 Công việc mới được giao")
                .AddText("Nhấn để xem chi tiết công việc.")
                .Show();
        }


        public static void Callback()
        {
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                var args = toastArgs.Argument;
                var input = ToastArguments.Parse(args);

                string action = input["action"];
                string taskId = input["taskId"];

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (action == "openTask")
                    {
                        var existingWindow = Application.Current.Windows
                            .OfType<EmployeeWindow>()
                            .FirstOrDefault();

                        if (existingWindow != null)
                        {
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();
                        }
                        else
                        {
                            var window = new EmployeeWindow();
                            window.Show();
                        }
                    }
                });
            };
        }

        public static bool IsAppInBackground()
        {
            var window = Application.Current.MainWindow;

            if (window.WindowState == WindowState.Minimized)
                return true;

            IntPtr foregroundWindow = GetForegroundWindow();
            IntPtr appWindow = new WindowInteropHelper(window).Handle;

            return foregroundWindow != appWindow;
        }
    }
}
