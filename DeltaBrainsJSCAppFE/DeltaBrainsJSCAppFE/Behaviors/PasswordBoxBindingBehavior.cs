using System.Windows;
using System.Windows.Controls;

namespace DeltaBrainsJSCAppFE.Behaviors
{
    public static class PasswordBoxBindingBehavior
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBindingBehavior),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static string GetPassword(DependencyObject d) =>
            (string)d.GetValue(PasswordProperty);

        public static void SetPassword(DependencyObject d, string value) =>
            d.SetValue(PasswordProperty, value);

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!(bool)passwordBox.GetValue(IsUpdatingProperty))
                    passwordBox.Password = (string)e.NewValue;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxBindingBehavior));

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.SetValue(IsUpdatingProperty, true);
                SetPassword(passwordBox, passwordBox.Password);
                passwordBox.SetValue(IsUpdatingProperty, false);
            }
        }
    }
}
