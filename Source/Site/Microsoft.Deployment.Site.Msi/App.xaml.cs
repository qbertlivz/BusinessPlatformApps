using System;
using System.Windows;
using Microsoft.Bpst.App.Msi;

//using PowerBI.SolutionTemplates.Base;

namespace Installer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int exitCode = 0;

        public Logger logger = new Logger();
        public App()
        {
            this.Exit += App_Exit;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            //logger.WriteMessage("Started");
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            //logger.WriteException(e.Exception);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //logger.WriteException(e.Exception);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            base.Shutdown(e.ApplicationExitCode);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
        }
    }
}