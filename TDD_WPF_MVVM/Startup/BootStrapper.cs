using Autofac;
using TDD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.ViewModel;
using TDD_WPF_MVVM.View;
using Prism.Events;
using TDD_WPF_MVVM.Dialogs;

namespace TDD_WPF_MVVM.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>()
                .As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<FriendEditViewModel>()
                .As<IFriendEditViewModel>();

            builder.RegisterType<NavigationViewModel>()
                .As<INavigationViewModel>();

            builder.RegisterType<NavigationDataProvider>()
                .As<INavigationDataProvider>();
            builder.RegisterType<FriendDataProvider>()
                .As<IFriendDataProvider>();

            builder.RegisterType<FileDataService>()
                .As<IDataService>();

            builder.RegisterType<MessageDialogService>()
                .As<IMessageDialogService>();

            return builder.Build();
        }
    }
}
