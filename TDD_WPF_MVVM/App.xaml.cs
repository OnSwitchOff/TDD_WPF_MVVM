using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.Startup;
using TDD_WPF_MVVM.View;
using TDD_WPF_MVVM.ViewModel;

namespace TDD_WPF_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootStraper = new BootStrapper();
            var container = bootStraper.BootStrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
