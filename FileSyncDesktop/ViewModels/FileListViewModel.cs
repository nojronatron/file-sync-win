﻿using Caliburn.Micro;
using FileSyncDesktop.Collections;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.ViewModels
{
    public class FileListViewModel : Screen
    {
        private ILogger _logger;
        private FileSystemWatcher _fsWatcher;
        private IFileWatcherSettings _fileWatcherSettings;

        private FileListCollection _fileList;
        public FileListCollection FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;
                NotifyOfPropertyChange(() => FileList);
            }
        }

        private bool _fsWatcherIsRunning;
        public bool FsWatcherIsRunning
        {
            get { return _fsWatcherIsRunning; }
            set
            {
                _fsWatcherIsRunning = value;
                NotifyOfPropertyChange(() => FsWatcherIsRunning);
                NotifyOfPropertyChange(() => CanStartFileMonitor);
                NotifyOfPropertyChange(() => CanStopFileMonitor);
            }
        }

        public FileListViewModel(IFileWatcherSettings fileWatcherSettings, ILogger logger)
        {
            _fileList = new FileListCollection();
            _logger = logger;
            _logger.Data("FileListViewModel", "Initializing.");
            _fileWatcherSettings = fileWatcherSettings;
            _logger.Flush();
        }

        public bool ConfigureFSWatcher()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Acquiring FileWatcher Settings.");
            _fileWatcherSettings.GetSettingsFromEnvVars();

            bool hasFilePathFilterSettings = HasFilePathFilterSettings();
            bool hasServerSettings = HasServerSettings();

            if (hasFilePathFilterSettings && hasServerSettings)
            {
                _logger.Data(methodName, "Client-side and Server-side variables set. Run as a Client-side instance.");
                _fsWatcher = new FileSystemWatcher(_fileWatcherSettings.FilePath, _fileWatcherSettings.FileType);
                _logger.Data(methodName, _fileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            if (hasFilePathFilterSettings && !hasServerSettings)
            {
                _logger.Data(methodName, "File path and filter settings found. Run as stand-alone watcher only.");
                _fsWatcher = new FileSystemWatcher(_fileWatcherSettings.FilePath, _fileWatcherSettings.FileType);
                _logger.Data(methodName, _fileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            _logger.Data(methodName, "Not enabling a server-settings only or no-settings configuration.");
            StopFSWatcher(); // also calls Dispose() on FSWatcher
            _logger.Flush();
            return false;
        }

        private bool HasServerSettings()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            if (_fileWatcherSettings.ServerAddress != string.Empty && _fileWatcherSettings.ServerPort != string.Empty)
            {
                _logger.Data(methodName, "Server address and port variables are set.");
                return true;
            }
            else
            {
                _logger.Data(methodName, "Server address and/or port variables are NOT set.");
                return false;
            }
        }

        private bool HasFilePathFilterSettings()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;

            if (_fileWatcherSettings.FilePath != string.Empty && _fileWatcherSettings.FileType != string.Empty)
            {
                _logger.Data(methodName, "File path and file type settings found!");
                return true;
            }
            else
            {
                _logger.Data(methodName, "File path and/or file type settings MISSING!");
                return false;
            }
        }

        public bool CanStartFileMonitor
        {
            get { return FsWatcherIsRunning ? false : true; }
        }

        public bool CanStopFileMonitor
        {
            get { return FsWatcherIsRunning ? true : false; }
        }

        public void StartFSWatcher()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            ConfigureFSWatcher();

            if (_fsWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized (Call Configure first)");
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Starting FileSystemWatcher");
                    _fsWatcher.Created += OnCreated;
                    _fsWatcher.Error += OnError;
                    _fsWatcher.IncludeSubdirectories = false;
                    _fsWatcher.EnableRaisingEvents = true;
                    FsWatcherIsRunning = true;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                    FsWatcherIsRunning = false;
                }
            }

            _logger.Flush();
        }

        public void StopFSWatcher()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");

            if (_fsWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized");
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Stopping FileSystemWatcher");
                    _fsWatcher.Created -= OnCreated;
                    _fsWatcher.Error -= OnError;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
                finally
                {
                    _logger.Data(methodName, "FileSystemWatcher Dispose() called");
                    _fsWatcher.Dispose();
                }
            }

            _logger.Flush();
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string message = $"Created: {e.FullPath}";
            Console.WriteLine(message);
            Logger log = new Logger();
            log.Data("OnCreated:", message);
            log.Flush();
            log.Dispose();
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            if (sender is FileListViewModel)
            {
                (sender as FileListViewModel).LogException(e.GetException());
            } 
            else
            {
                // I'm not sure how this would happen, but if it does it will get logged
                Logger log = new Logger();
                log.Data("OnError - UNKNOWN CALLER", e.GetException().Message);
                log.Flush();
                log.Dispose();
            }
        }

        public string GetFilePath()
        {
            return _fileWatcherSettings.FilePath;
        }

        public string GetFileType()
        {
            return _fileWatcherSettings.FileType;
        }

        public string GetServerAddress()
        {
            return _fileWatcherSettings.ServerAddress;
        }

        public string GetServerPort()
        {
            return _fileWatcherSettings.ServerPort;
        }

        private void LogException(Exception ex)
        {
            if (ex != null)
            {
                _logger.Data("[LogException]", $"Received exception {ex.Message}");
            }
        }

    }
}