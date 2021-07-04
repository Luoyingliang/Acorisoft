using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Acorisoft.Platform.Windows.Services;
using DryIoc;
using ReactiveUI;
using Splat;

namespace Acorisoft.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        public App()
        {
            if (ViewModelGenerated.Version < 1)
            {
                ViewModelGenerated.Initialize();
                Acorisoft.Morisa.ViewModelGenerated.Initialize();
                ServiceHost.Container.RegisterInstance(ViewModelLocator.AppViewModel);
                ServiceHost.Container.UseInstance<IScreen>(ViewModelLocator.AppViewModel);
            }

            //
            // 无法捕捉的错误
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            
            //
            //
            ServiceHost.EnableLogger();
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
        }
    }
}
