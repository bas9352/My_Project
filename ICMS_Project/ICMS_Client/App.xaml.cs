using System;
using System.Windows;

namespace ICMS_Client
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
                Width = System.Windows.SystemParameters.PrimaryScreenWidth,
                Height = System.Windows.SystemParameters.PrimaryScreenHeight,
                UseNoneWindowStyle = true,
                IgnoreTaskbarOnMaximize = true,
                Topmost = true,
                //TitleAlignment = ,
                //WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                ResizeMode = ResizeMode.NoResize,
                WindowState = WindowState.Maximized,
                ShowInTaskbar = false
            };

            IoC.Application.MainApp = Current.MainWindow;
            Current.MainWindow.Show();
        }
    }
}
