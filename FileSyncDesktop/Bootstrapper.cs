using Caliburn.Micro;
using FileSyncDesktop.Collections;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Library.Api;
using FileSyncDesktop.Models;
using FileSyncDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSyncDesktop
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            LogManager.GetLog = type => new DebugLog(type);
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IBibReportEndpoint, BibReportEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILogger, Logger>()
                .Singleton<IFileWatcherSettings, FileWatcherSettings>()
                .Singleton<IFileDataProcessor, FileDataProcessor>()
                .Singleton<IBibRecordCollection, BibRecordCollection>()
                .Singleton<IAPIHelper, APIHelper>();

            foreach (var assembly in SelectAssemblies())
            {
                assembly.GetTypes()
                    .Where(type => type.IsClass)
                    .Where(type => type.Name.EndsWith("ViewModel"))
                    .ToList()
                    .ForEach(viewModelType => _container.RegisterPerRequest(
                        viewModelType, viewModelType.ToString(), viewModelType));
            }

            // todo: try base.configure()
            base.Configure();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var instance = _container.GetInstance(serviceType, key);
            if (instance != null)
            {
                return instance;
            } else
            {
                throw new InvalidOperationException("Could not locate any instances.");
            }

            //return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        //protected async override void OnStartup(object sender, StartupEventArgs e)
        //{
        //    await DisplayRootViewForAsync(typeof(MainWindowViewModel));
        //}

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

    }
}
