using file_sync_win.helpers;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace file_sync_win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Logger logger { get; set; } = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  portions of code are thanks to a Gist by Ronnie Overby at https://gist.github.com/ronnieoverby/7568387 
            logger = new Logger();

            if (logger.IsEnabled)
            {
                logger.Data("Application Startup", "Application Startup called");
                AppDomain.CurrentDomain.UnhandledException += (sig, exc) =>
                    LogUnhandledException((Exception)exc.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                DispatcherUnhandledException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "TaskScheduler.UnobservedTaskException");

                try
                {
                    logger.Flush();
                }
                catch (Exception ex)
                {
                    logger.Data("Application Startup", $"Exception thrown: {ex.Message}");
                    LogInnerExceptionMessages(ex, "WindowLoading InnerException");
                    logger.Flush();
                    logger.Dispose();
                    App.Current.Shutdown();
                }
            }
        }

        private void LogInnerExceptionMessages(Exception e, string title)
        {
            if (e.InnerException != null)
            {
                LogInnerExceptionMessages(e.InnerException, title);
            }

            logger.Data(title, e.Message);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // todo: add any final logging here before app exits
            logger.Flush();
            logger.Dispose();
            App.Current.Shutdown();
        }

        private void LogUnhandledException(Exception exception, string @event)
        {
            logger.Data("Unhandled Exception Catcher", "Next log entry will have exception and atEvent.");
            LogInnerExceptionMessages(exception, @event);
            logger.Flush();
        }
    }
}
