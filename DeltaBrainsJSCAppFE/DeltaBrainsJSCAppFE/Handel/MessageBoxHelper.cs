using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeltaBrainsJSCAppFE.Handel
{
    //Custom MessageBox
    public static class MessageBoxHelper
    {
        public static void ShowError(string message)
        {
            Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowWarning(string message)
        {
            Show(message, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowInfo(string message)
        {
            Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public static MessageBoxResult ShowQuestion(string message)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() =>
                {
                    return MessageBox.Show(message, "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                });
            }
            else
            {
                return MessageBox.Show(message, "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
        }

        private static void Show(string message, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, title, button, icon);
                });
            }
            else
            {
                MessageBox.Show(message, title, button, icon);
            }
        }
    }
}
