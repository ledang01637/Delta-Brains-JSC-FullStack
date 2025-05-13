using DeltaBrainsJSCAppFE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }

        public LoginViewModel() 
        {
            LoadedWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;

            });
            //MessageBox.Show("Login model");
        }
    }
}
