using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.ViewModels;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow(TaskRes task = null)
        {
            InitializeComponent();

            if (task == null)
                DataContext = new TaskViewModel();
            else
                DataContext = new TaskViewModel(task);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
