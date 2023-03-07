using Caliburn.Micro;
using ToolboxUI.Helpers;
using ToolboxUI.Library.Api;
using ToolboxUI.Library.Helpers;
using ToolboxUI.Library.Models;
using ToolboxUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ToolboxUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged"
            );
        }

        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IServerEndpoint, ServerEndpoint>()
                .PerRequest<IDatabaseEndpoint, DatabaseEndpoint>()
                .PerRequest<ISchemaEndpoint, SchemaEndpoint>()
                .PerRequest<ITableEndpoint, TableEndpoint>()
                .PerRequest<IFieldEndpoint, FieldEndpoint>()
                .PerRequest<IDatabaseObjectEndpoint, DatabaseObjectEndpoint>()
                .PerRequest<IDataTableLineageEndpoint, DataTableLineageEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IAPIHelper, APIHelper>();

            // Directly wires instance to view model
            // Will need to change in order to unit test later
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
