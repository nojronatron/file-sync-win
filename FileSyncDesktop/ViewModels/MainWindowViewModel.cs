using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace FileSyncDesktop.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private string _fileSourcePath;

        public string FileSourcePath
        {
            get { return _fileSourcePath; }
            set { _fileSourcePath = value;
                NotifyOfPropertyChange(() => FileSourcePath);
            }
        }

        private string _filterArgument;

        public string FilterArgument
        {
            get { return _filterArgument; }
            set { _filterArgument = value;
                NotifyOfPropertyChange(() => FilterArgument);
            }
        }

        private string _serverAddress;

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value;
                NotifyOfPropertyChange(() => ServerAddress);
            }
        }

        private int _serverPort;

        public int ServerPort
        {
            get { return _serverPort; }
            set { _serverPort = value;
                NotifyOfPropertyChange(() => ServerPort);
            }
        }

        private bool _fileMonitorRunning;

        public bool FileMonitorRunning
        {
            get { return _fileMonitorRunning; }
            set { _fileMonitorRunning = value;
                NotifyOfPropertyChange(() => FileMonitorRunning);
                NotifyOfPropertyChange(() => CanStartFileMonitor);
                NotifyOfPropertyChange(() => CanStopFileMonitor);
            }
        }

        public MainWindowViewModel()
        {
            FileMonitorRunning = false;
        }

        public bool CanStartFileMonitor
        {
            get { return FileMonitorRunning ? false : true; }
        }

        public bool CanStopFileMonitor
        { 
            get { return FileMonitorRunning ? true : false; } 
        }

        public void StartFileMonitor()
        {
            FileMonitorRunning = true;
        }

        public void StopFileMonitor()
        {
            FileMonitorRunning = false;
        }

        public void MenuFileExit()
        {
            Application.Current.Shutdown();
        }

        public void MenuAbout()
        {
            MessageBox.Show(
                "File Sync Win\n\nVersion 0.0,1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron/file-sync-win",
                "About File Sync Win",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

    }
}
