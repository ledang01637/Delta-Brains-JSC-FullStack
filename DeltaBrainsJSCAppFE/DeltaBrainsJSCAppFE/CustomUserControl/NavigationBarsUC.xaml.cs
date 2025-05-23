﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeltaBrainsJSCAppFE.CustomUserControl
{
    /// <summary>
    /// Interaction logic for NavigationBarsUC.xaml
    /// </summary>
    public partial class NavigationBarsUC : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(NavigationBarsUC),
                new PropertyMetadata("Default Title", OnTitleChanged));

        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(
                  nameof(TargetElement),
                  typeof(UIElement),
                  typeof(NavigationBarsUC),
                  new PropertyMetadata(null));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public UIElement TargetElement
        {
            get => (UIElement)GetValue(TargetElementProperty);
            set => SetValue(TargetElementProperty, value);
        }


        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NavigationBarsUC;
            control?.OnTitleChanged(e);
        }

        private void OnTitleChanged(DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Title changed to: {e.NewValue}");
        }

        public NavigationBarsUC()
        {
            InitializeComponent();
            Loaded += (s, e) => { this.DataContext = this; };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }



        private void ToggleSliderButton_Click(object sender, RoutedEventArgs e)
        {
            if (TargetElement != null)
            {
                TargetElement.Visibility = TargetElement.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}
