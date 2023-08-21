using file_sync_win.helpers;
using file_sync_win.models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace file_sync_win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger Logger = null;
        private FileWatcher FileWatcher = null;
        private FileWatcherSettings FileWatcherSettings { get; set; }

        public MainWindow()
        {
            Logger = new Logger();
            Logger.Data("MainWindow", "MainWindow constructor called");
            Logger.Flush();
            InitializeComponent();
        }

        private void SetupFileWatcher()
        {
            string currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Setting up FileWatcher");
            FileWatcher = new FileWatcher(Logger);
            FileWatcher.Configure();
            Logger.Flush();
        }

        private void ShutdownFileWatcher()
        {
        }

        /* from app.xaml.cs */

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Logger.IsEnabled)
            {
                Logger.Data("Application Startup", "Application Startup called");
                AppDomain.CurrentDomain.UnhandledException += (sig, exc) =>
                    LogUnhandledException((Exception)exc.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                TaskScheduler.UnobservedTaskException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "TaskScheduler.UnobservedTaskException");

                try
                {
                    Logger.Flush();
                }
                catch (Exception ex)
                {
                    Logger.Data("Application Startup", $"Exception thrown: {ex.Message}");
                    LogInnerExceptionMessages(ex, "WindowLoading InnerException");
                    Logger.Flush();
                    Logger.Dispose();
                    App.Current.Shutdown();
                }
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void LogInnerExceptionMessages(Exception e, string title)
        {
            if (e.InnerException != null)
            {
                LogInnerExceptionMessages(e.InnerException, title);
            }

            Logger.Data(title, e.Message);
            Logger.Flush();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // todo: add any final logging here before app exits
            Logger.Flush();
            Logger.Dispose();
            App.Current.Shutdown();
        }

        private void LogUnhandledException(Exception exception, string @event)
        {
            Logger.Data("Unhandled Exception Catcher", "Next log entry will have exception and atEvent.");
            LogInnerExceptionMessages(exception, @event);
            Logger.Flush();
        }

        /* end app.xaml.cs */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "called");
            // perform other tasks here
            Logger.Flush();
        }

        private void StartFileMonitor_Click(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Button clicked");

            try
            {
                SetupFileWatcher();
                Logger.Data(currentMethod, $"FileWatcher Settings: {FileWatcherSettings}");
                FileWatcher.Start();
                Logger.Data(currentMethod, "FileWatcher Start Called");
            } catch (Exception ex)
            {
                Logger.Data(currentMethod, $"Exception thrown: {ex.Message}");
            }

            Logger.Flush();
        }

        private void StopFileMonitor_Click(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Button clicked");

            try
            {
                FileWatcher.Stop();
                Logger.Data(currentMethod, "FileWatcher Stop Called");
                FileWatcher.Dispose();
                Logger.Data(currentMethod, "FileWatcher Dispose Called");
            }
            catch (Exception ex)
            {
                Logger.Data(currentMethod, $"Exception thrown: {ex.Message}");
            }

            Logger.Flush();
        }

        private void MenuAboutAppInfo_Click(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Menu clicked");
            Logger.Flush();
            
            MessageBox.Show(
                "File Sync Win\n\nVersion 0.0,1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron/file-sync-win", 
                "About File Sync Win", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Logger.Data(currentMethod, "Window Closing called");
            //ShutdownAppWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Button clicked");
            ShutdownAppWindow();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(currentMethod, "Menu clicked");
            ShutdownAppWindow();
        }

        private void ShutdownAppWindow()
        {
            Logger.Flush();
            Logger.Dispose();
            App.Current.Shutdown();
        }
    }
}
