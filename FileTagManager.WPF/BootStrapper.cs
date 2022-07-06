using Autofac;
using Autofac.Core;
using FileTagManager.Database.UoW;
using FileTagManager.Domain.Interfaces;
using FileTagManager.WPF.Services;
using FileTagManager.WPF.ViewModels;
using FileTagManager.WPF.Views;
using System;
using System.IO.Abstractions;

namespace FileTagManager.WPF
{
    public static class BootStrapper
    {
        private static ILifetimeScope _rootScope;

        public static void Start()
        {
            if (_rootScope != null) return;

            var builder = new ContainerBuilder();

            builder.RegisterType<MainView>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SideBarViewModel>().SingleInstance();
            builder.RegisterType<FileDetailViewModel>().SingleInstance();
            builder.RegisterType<InitDirViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<HomeViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<FileListViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SearchViewModel>().InstancePerLifetimeScope();

            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().SingleInstance();
            builder.RegisterType<NavigatorService>().As<INavigatorService>().SingleInstance();
            builder.RegisterType<CreateViewModelService>().As<ICreateViewModelService>().SingleInstance();
            builder.RegisterType<OpenFileDialogService>().As<IOpenFileDialogService>().SingleInstance();
            builder.RegisterType<FileService>().As<IFileService>().SingleInstance();
            builder.RegisterType<MapperService>().As<IMapperService>().SingleInstance();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<MessengerService>().As<IMessengerService>().InstancePerLifetimeScope();

            builder.Register<CreateViewModel<InitDirViewModel>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () => context.Resolve<InitDirViewModel>();
            });

            builder.Register<CreateViewModel<HomeViewModel>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () => context.Resolve<HomeViewModel>();
            });

            builder.Register<CreateViewModel<FileListViewModel>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () => context.Resolve<FileListViewModel>();
            });

            builder.Register<CreateViewModel<SearchViewModel>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () => context.Resolve<SearchViewModel>();
            });

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
                throw new Exception("Bootstrapper hasn't been started!");

            return _rootScope.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null)
                throw new Exception("Bootstrapper hasn't been started!");

            return _rootScope.Resolve<T>(parameters);
        }
    }
}
