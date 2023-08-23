using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.CompilerServices;
using FileSyncDesktop.Models;
using System.Reflection;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace FileSyncDesktop.Helpers
{
    internal class FileWatcher : IDisposable
    {
        private bool disposedValue;
        private Logger _logger;
        private FileWatcherSettings FileWatcherSettings { get; set; }
        private FileSystemWatcher FSWatcher { get; set; } = null;

        private static BindableCollection<string> _fileList = new BindableCollection<string>();
        public static BindableCollection<string> FileList
        {
            get { return _fileList; }
            set { _fileList = value; }
        }

        /// <summary>
        /// Constructor initializes a FileWatcher instance.
        /// </summary>
        /// <param name="fileWatcherSettings">FileWatcherSettings object</param>
        public FileWatcher()
        {
            FileWatcherSettings = new FileWatcherSettings();
            _logger = new Logger();
            _logger.Data("FileWatcher CTOR", "Initializing FileWatcher ** must be configured **.");
            _logger.Flush();
        }

        public bool Configure()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Acquiring FileWatcher Settings.");
            FileWatcherSettings.GetSettingsFromEnvVars();

            bool hasFilePathFilterSettings = HasFilePathFilterSettings();
            bool hasServerSettings = HasServerSettings();

            if (hasFilePathFilterSettings && hasServerSettings)
            {
                _logger.Data(methodName, "Client-side and Server-side variables set. Run as a Client-side instance.");
                FSWatcher = new FileSystemWatcher(FileWatcherSettings.FilePath, FileWatcherSettings.FileType);
                _logger.Data(methodName, FileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            if (hasFilePathFilterSettings && !hasServerSettings)
            {
                _logger.Data(methodName, "File path and filter settings found. Run as stand-alone watcher only.");
                FSWatcher = new FileSystemWatcher(FileWatcherSettings.FilePath, FileWatcherSettings.FileType);
                _logger.Data(methodName, FileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            _logger.Data(methodName, "Not enabling a server-settings only or no-settings configuration.");
            Stop(); // also calls Dispose() on FSWatcher
            _logger.Flush();
            return false;
        }

        private bool HasServerSettings()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            if (FileWatcherSettings.ServerAddress != string.Empty && FileWatcherSettings.ServerPort != string.Empty)
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

            if (FileWatcherSettings.FilePath != string.Empty && FileWatcherSettings.FileType != string.Empty)
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

        /// <summary>
        /// Returns an initialized FileSystemWatcher object.
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;

            if (FSWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized (Call Configure first)");
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Starting FileSystemWatcher");
                    FSWatcher.Created += OnCreated;
                    FSWatcher.Error += OnError;
                    FSWatcher.IncludeSubdirectories = false;
                    FSWatcher.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;

            if (FSWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized");
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Stopping FileSystemWatcher");
                    FSWatcher.Created -= OnCreated;
                    FSWatcher.Error -= OnError;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
                finally
                {
                    _logger.Data(methodName, "FileSystemWatcher Dispose() called");
                    FSWatcher.Dispose();
                }
            }
        }

        /// <summary>
        /// When a file is created, log the event to the common logging mechanism.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string message = $"Created: {e.FullPath}";
            Console.WriteLine(message);
            Logger log = new Logger();
            log.Data("OnCreated:", message);
            FileList.Add(e.FullPath); // static collection
            log.Flush();
            log.Dispose();
        }

        public string GetFilePath()
        {
            return FileWatcherSettings.FilePath;
        }

        public string GetFileType()
        {
            return FileWatcherSettings.FileType;
        }

        public string GetServerAddress()
        {
            return FileWatcherSettings.ServerAddress;
        }

        public string GetServerPort()
        {
            return FileWatcherSettings.ServerPort;
        }

        private static void OnError(object sender, ErrorEventArgs e) => LogException(e.GetException());

        private static void LogException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logger = null;
                    FileWatcherSettings = null;
                    FSWatcher = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileWatcher()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
