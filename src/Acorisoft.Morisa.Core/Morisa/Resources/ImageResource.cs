using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using Acorisoft.ComponentModel;
// ReSharper disable ConvertToAutoProperty

namespace Acorisoft.Morisa.Resources
{
    [ExplicitSerializer]
    [ExplicitDeserializer]
    public class ImageResource : ObservableCollection<Guid>
    {
        protected override void InsertItem(int index, Guid item)
        {
            base.InsertItem(index, item);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMultipleValues)));
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMultipleValues)));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMultipleValues)));
        }

        public Guid First => Count > 0 ? this[0] : Guid.Empty;

        public bool HasMultipleValues => Count > 1;
    }
}