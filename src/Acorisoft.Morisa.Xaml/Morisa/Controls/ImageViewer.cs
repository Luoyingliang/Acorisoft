using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.Morisa.Resources;
using Acorisoft.Platform;

namespace Acorisoft.Morisa.Controls
{
    public class ImageViewer : Control
    {
        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (ImageViewer)d;

            if(e.NewValue is not ImageResource newRes)
            {
                return;
            }

            if(e.OldValue is ImageResource oldRes)
            {
                oldRes.CollectionChanged -= viewer.OnCollectionChanged;
            }

            newRes.CollectionChanged += viewer.OnCollectionChanged;
            d.SetValue(HasMultiValuesProperty, newRes.HasMultipleValues);
            d.SetValue(ImageSourcesProperty, newRes);
            d.SetValue(FirstImageProperty, newRes.First);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                FirstImage = Image.First;
            }

            HasMultiValues = Image.HasMultipleValues;
        }


        public ImageResource Image
        {
            get { return (ImageResource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public bool HasMultiValues
        {
            get { return (bool)GetValue(HasMultiValuesProperty.DependencyProperty); }
            private set { SetValue(HasMultiValuesProperty, value); }
        }

        public IEnumerable<Guid> ImageSources
        {
            get { return (IEnumerable<Guid>)GetValue(ImageSourcesProperty.DependencyProperty); }
            private set { SetValue(ImageSourcesProperty, value); }
        }

        public Guid FirstImage
        {
            get { return (Guid)GetValue(FirstImageProperty.DependencyProperty); }
            private set { SetValue(FirstImageProperty, value); }
        }

        public static readonly DependencyPropertyKey HasMultiValuesProperty = DependencyProperty.RegisterReadOnly(
            "HasMultiValues",
            typeof(bool),
            typeof(ImageViewer),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey FirstImageProperty = DependencyProperty.RegisterReadOnly(
              "FirstImage",
              typeof(Guid),
              typeof(ImageViewer),
              new PropertyMetadata(null));

        public static readonly DependencyPropertyKey ImageSourcesProperty = DependencyProperty.RegisterReadOnly(
              "ImageSources",
              typeof(IEnumerable<Guid>),
              typeof(ImageViewer),
              new PropertyMetadata(null));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageResource), typeof(ImageViewer), new PropertyMetadata(null, OnImageChanged));


    }
}