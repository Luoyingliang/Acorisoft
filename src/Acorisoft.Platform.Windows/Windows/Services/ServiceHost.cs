using System;
using DryIoc;
using MediatR;
using Splat.DryIoc;

namespace Acorisoft.Platform.Windows.Services
{
    /// <summary>
    /// <see cref="ServiceHost"/> 类型表示一个服务宿主，用于为应用程序提供一个服务定位器支持。
    /// </summary>
    public static class ServiceHost
    {
        private static readonly Lazy<IDialogSupportService> DialogSupportServiceLazyInstance = new Lazy<IDialogSupportService>();
        private static readonly Lazy<INavigateSupportService> NavigateSupportServiceLazyInstance = new Lazy<INavigateSupportService>(new NavigateSupportService());
        private static readonly Lazy<IExtraViewSupportService> ExtraViewSupportServiceLazyInstance = new Lazy<IExtraViewSupportService>(new ExtraViewSupportService());
        private static readonly Lazy<IPromptSupportService> PromptSupportServiceLazyInstance = new Lazy<IPromptSupportService>();
        private static readonly Lazy<IContainer> ContainerLazyInstance = new Lazy<IContainer>(()=>{ 
            var container = new Container(Rules.Default.WithTrackingDisposableTransients());
            container.UseDryIocDependencyResolver();
            return container;
        });

        public static void Initialize()
        {
            //
            // 注册中介者。
            var serviceFactory = new ServiceFactory(Container.Resolve);
            var mediator = new Mediator(serviceFactory);
            Container.RegisterInstance<IMediator>(mediator);
            
            Container.RegisterInstance<INavigateSupportService>(NavigateSupportService);
            // Container.RegisterInstance<IDialogSupportService>(DialogSupportService);
            // Container.RegisterInstance<IPromptSupportService>(PromptSupportService);
            Container.RegisterInstance<IExtraViewSupportService>(ExtraViewSupportService);

        }

        public static void EnableLogger()
        {
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public static IContainer Container => ContainerLazyInstance.Value;
        
        /// <summary>
        /// 获取全局的 <see cref="IDialogSupportService"/> 接口服务。
        /// </summary>
        internal static IDialogSupportService DialogSupportService => DialogSupportServiceLazyInstance.Value;

        /// <summary>
        /// 获取全局的 <see cref="INavigateSupportService"/> 接口服务。
        /// </summary>
        internal static INavigateSupportService NavigateSupportService => NavigateSupportServiceLazyInstance.Value;
        
        
        /// <summary>
        /// 获取全局的 <see cref="IPromptSupportService"/> 接口服务。
        /// </summary>
        internal static IPromptSupportService PromptSupportService => PromptSupportServiceLazyInstance.Value;
        
        /// <summary>
        /// 获取全局的 <see cref="IExtraViewSupportService"/> 接口服务。
        /// </summary>
        internal static IExtraViewSupportService ExtraViewSupportService => ExtraViewSupportServiceLazyInstance.Value;
    }
}