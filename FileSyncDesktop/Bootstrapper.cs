using Caliburn.Micro;
using FileSyncDesktop.Collections;
using FileSyncDesktop.Helpers;
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
            _container.Instance(_container);
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILogger, Logger>()
                .Singleton<IFileWatcherSettings, FileWatcherSettings>()
                .Singleton<IFileDataProcessor, FileDataProcessor>()
                .Singleton<IBibRecordCollection, BibRecordCollection>();

            foreach (var assembly in SelectAssemblies())
            {
                assembly.GetTypes()
                    .Where(type => type.IsClass)
                    .Where(type => type.Name.EndsWith("ViewModel"))
                    .ToList()
                    .ForEach(viewModelType => _container.RegisterPerRequest(
                        viewModelType, viewModelType.ToString(), viewModelType));
            }
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
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
