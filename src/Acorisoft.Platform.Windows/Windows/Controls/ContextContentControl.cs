using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Acorisoft.Platform.Windows.Controls
{
    public enum HeadlineLevel
    {
        Headline1,
        Headline2,
        Headline3,
        Headline4,
        Headline5

    }

    public class ContextContentControl : ContentControl
    {
        static ContextContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextContentControl), new FrameworkPropertyMetadata(typeof(ContextContentControl)));
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public HeadlineLevel Headline
        {
            get => (HeadlineLevel)GetValue(HeadlineProperty);
            set => SetValue(HeadlineProperty, value);
        }


        public static readonly DependencyProperty HeadlineProperty = DependencyProperty.Register(
            "Headline",
            typeof(HeadlineLevel), 
            typeof(ContextContentControl), 
            new PropertyMetadata(default(HeadlineLevel)));



        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(ContextContentControl), 
            new PropertyMetadata(null));


    }
}
