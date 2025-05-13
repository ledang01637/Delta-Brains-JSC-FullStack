using System;
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
using System.Windows.Shapes;

namespace DeltaBrainsJSCAppFE.Views
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            //if (Sidebar.Visibility == Visibility.Visible)
            //{
            //    Sidebar.Visibility = Visibility.Collapsed;
            //    Sidebar.Width = 0;
            //}
            //else
            //{
            //    Sidebar.Visibility = Visibility.Visible;
            //    Sidebar.Width = 200;
            //}
        }
    }
}
