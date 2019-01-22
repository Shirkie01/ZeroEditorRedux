using log4net;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ZeroEditorRedux
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string text = string.Format("An unhandled exception has occurred." + Environment.NewLine + Environment.NewLine + "{0}" + Environment.NewLine + "{1}", e.Exception.Message, e.Exception.StackTrace);
            MessageBox.Show(text, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            log.Debug("Starting ZeroEditorRedux...");
        }
    }
}