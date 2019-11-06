using System.Windows;

namespace ICMS_Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(e);

            IoC.Setup();

            // Show the main window



            Current.MainWindow = new MainWindow()
            {
                DataContext = IoC.Application,
                //Width = Math.Round(System.Windows.SystemParameters.PrimaryScreenWidth * 55 / 100),
                //Height = Math.Round(System.Windows.SystemParameters.PrimaryScreenHeight * 75 / 100)

            };

            IoC.Application.MainApp = Current.MainWindow;
            Current.MainWindow.Show();
        }
    }
}