using file_sync_win.helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace file_sync_win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = null;
        
        public MainWindow()
        {
            logger = new Logger();
            logger.Data("MainWindow", "MainWindow constructor called");
            logger.Flush();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "called");
            // perform other tasks here
            logger.Flush();
        }

        private void StartFileMonitor_Click(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "Button clicked");
            logger.Flush();
        }

        private void StopFileMonitor_Click(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "Button clicked");
        }

        private void MenuAboutAppInfo_Click(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "About App Info clicked");
            logger.Flush();
            MessageBox.Show("File Sync Win\n\nVersion 0.0,1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron", "About File Sync Win", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "Window Closing called");
            //ShutdownAppWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "Button clicked");
            ShutdownAppWindow();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), "Menu clicked");
            ShutdownAppWindow();
        }

        private void ShutdownAppWindow()
        {
            logger.Flush();
            logger.Dispose();
            App.Current.Shutdown();
        }
    }
}
