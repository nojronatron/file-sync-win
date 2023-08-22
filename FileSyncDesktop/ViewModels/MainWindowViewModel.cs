﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Models;

namespace FileSyncDesktop.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private FileWatcher _fileWatcher;
        private Logger _logger;

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

        private bool _fileWatcherRunning;
        public bool FileWatcherRunning
        {
            get { return _fileWatcherRunning; }
            set { _fileWatcherRunning = value;
                NotifyOfPropertyChange(() => FileWatcherRunning);
                NotifyOfPropertyChange(() => CanStartFileMonitor);
                NotifyOfPropertyChange(() => CanStopFileMonitor);
            }
        }

        public MainWindowViewModel()
        {
            _logger = new Logger();
            _logger.Data("MainWindowViewModel:", "MainWindowViewModel created.");
            _logger.Flush();
            FileWatcherRunning = false;
        }

        public bool CanStartFileMonitor
        {
            get { return FileWatcherRunning ? false : true; }
        }

        public bool CanStopFileMonitor
        { 
            get { return FileWatcherRunning ? true : false; } 
        }

        public void StartFileMonitor()
        {
            _logger.Data("StartFileMonitor:", "Initializing file monitor.");
            _fileWatcher = new FileWatcher();
            _fileWatcher.Configure();

            _logger.Data("StartFileMonitor:", "tbd.");
            FileWatcherRunning = true;
            _logger.Flush();
            return;
        }

        public void StopFileMonitor()
        {
            _logger.Data("StopFileMonitor:", "Stopping file monitor.");
            FileWatcherRunning = false;
            _logger.Flush();
        }

        public void MenuFileExit()
        {
            _logger.Data("MenuFileExit:", "Exiting MainWindowView.");
            _logger.Flush();
            _logger.Dispose();
            Application.Current.Shutdown();
        }

        public void MenuAbout()
        {
            string messageBoxTitle = "About File Sync Win";
            string messageBoxText = "File Sync Win\n\nVersion 0.0,1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron/file-sync-win";
            _logger.Data(messageBoxText, messageBoxTitle);
            MessageBox.Show(
                messageBoxText,
                messageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            _logger.Flush();
        }

    }
}
