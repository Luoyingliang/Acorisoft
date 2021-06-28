using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
                ServiceHost.Container.RegisterInstance(ViewModelLocator.AppViewModel);
                ServiceHost.Container.UseInstance<IScreen>(ViewModelLocator.AppViewModel);
            }
            
            //
            //
            ServiceHost.EnableLogger();
        }
    }
}
