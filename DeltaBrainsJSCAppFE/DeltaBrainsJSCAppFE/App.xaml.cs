using DeltaBrainsJSCAppFE.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DeltaBrainsJSCAppFE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }

}
