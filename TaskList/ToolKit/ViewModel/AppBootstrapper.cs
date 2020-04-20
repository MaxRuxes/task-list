using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using TaskList.ViewModels;

namespace TaskList.ToolKit.ViewModel
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<AuthorizationWindowViewModel>();
        }

        protected override void Configure()
        {
            _container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance
                .Select(x => new AssemblyCatalog(x))
                .OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type service, string key)
        {
            var contract = string.IsNullOrEmpty(key)
                ? AttributedModelServices.GetContractName(service)
                : key;
            var exports = _container.GetExportedValues<object>(contract).ToList();

            if(exports.Any())
            {
                return exports.First();
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }
    }
}
