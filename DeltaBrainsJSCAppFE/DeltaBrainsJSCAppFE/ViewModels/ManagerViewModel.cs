using DeltaBrainsJSCAppFE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class ManagerViewModel : BaseViewModel
    {
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }

        public ManagerViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                p.Hide();
                LoginWindow loginWindow = new();
                loginWindow.ShowDialog();
                p.Close();
            });
        }
    }
}
