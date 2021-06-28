using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Acorisoft.Platform.Windows.Services;
using ReactiveUI;

namespace Acorisoft.Platform.Windows.Controls
{
    /// <summary>
    /// <see cref="AppServiceHost"/> 表示一个应用服务宿主控件。用于为应用程序提供对话框、提示、等待操作支持。
    /// </summary>
    [TemplatePart(Name = "")]
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public class AppServiceHost : ContentControl
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // IAwaitService Implementations
        //
        // 用于实现必要活动操作启动时，组织用户输入。
        //--------------------------------------------------------------------------------------------------------------
        #region IAwaitService

        private IDisposable _beginDisposable;
        private IDisposable _endDisposable;
        private IDisposable _updateDisposable;
        
        private static void OnAwaitServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //
            // 自身实例
            var host = (AppServiceHost) d;

            if (e.NewValue is not IAwaitService newServ)
            {
                return;
            }

            if (ReferenceEquals(e.OldValue, e.NewValue))
            {
                return;
            }
            
            host._beginDisposable?.Dispose();
            host._endDisposable?.Dispose();
            host._updateDisposable?.Dispose();
            host._beginDisposable = newServ.BeginAwait.ObserveOn(RxApp.MainThreadScheduler).Subscribe(host.OnStartAwaitView);
            host._endDisposable = newServ.EndAwait.ObserveOn(RxApp.MainThreadScheduler).Subscribe(host.OnCloseAwaitView);
            host._updateDisposable = newServ.UpdateAwaitContent.ObserveOn(RxApp.MainThreadScheduler).Subscribe(host.OnUpdateAwaitView);
        }

        private void OnUpdateAwaitView(string content)
        {
            //
            // 更新提示信息
            AwaitToolTips = content;
        }

        private void OnStartAwaitView(Unit unit)
        {
            RaiseEvent(new RoutedEventArgs {RoutedEvent = ActivityStartEvent});
        }
        
        private void OnCloseAwaitView(Unit unit)
        {
            RaiseEvent(new RoutedEventArgs {RoutedEvent = ActivityStopEvent});
        }

        public static readonly RoutedEvent ActivityStartEvent = EventManager.RegisterRoutedEvent("ActivityStart", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AppServiceHost));
        public static readonly RoutedEvent ActivityStopEvent = EventManager.RegisterRoutedEvent("ActivityStop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AppServiceHost));
        
        public static readonly DependencyProperty AwaitServiceProperty = DependencyProperty.Register("AwaitService",
            typeof(IAwaitService), 
            typeof(AppServiceHost),
            new PropertyMetadata(null, OnAwaitServiceChanged));


        public static readonly DependencyPropertyKey AwaitToolTipsProperty = DependencyProperty.RegisterReadOnly("AwaitToolTips",
            typeof(string), 
            typeof(AppServiceHost),
            new PropertyMetadata(null));

        public event RoutedEventHandler ActivityStart
        {
            add => AddHandler(ActivityStartEvent, value);
            remove => RemoveHandler(ActivityStartEvent, value);
        }
        
        public event RoutedEventHandler ActivityStop
        {
            add => AddHandler(ActivityStopEvent, value);
            remove => RemoveHandler(ActivityStopEvent, value);
        }
        
        public string AwaitToolTips
        {
            get => (string) GetValue(AwaitToolTipsProperty.DependencyProperty);
            private set => SetValue(AwaitToolTipsProperty, value);
        }
        
        public IAwaitService AwaitService
        {
            get => (IAwaitService) GetValue(AwaitServiceProperty);
            set => SetValue(AwaitServiceProperty, value);
        }

        #endregion
    }
}