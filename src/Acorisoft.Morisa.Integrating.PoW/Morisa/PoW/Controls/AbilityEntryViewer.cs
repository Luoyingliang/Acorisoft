using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.Morisa.PoW.Items.Abilities;

namespace Acorisoft.Morisa.PoW.Controls
{
   public class AbilityEntryViewer : Control
    {
        static AbilityEntryViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityEntryViewer), new FrameworkPropertyMetadata(typeof(AbilityEntryViewer)));
        }

        public AbilityEntry Entry
        {
            get { return (AbilityEntry)GetValue(EntryProperty); }
            set { SetValue(EntryProperty, value); }
        }

        public string EntryName
        {
            get { return (string)GetValue(EntryNameProperty.DependencyProperty); }
            private set { SetValue(EntryNameProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty.DependencyProperty); }
            private set { SetValue(DescriptionProperty, value); }
        }
        

        public static readonly DependencyPropertyKey DescriptionProperty = DependencyProperty.RegisterReadOnly(
            "Description",
            typeof(string),
            typeof(AbilityEntryViewer),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey EntryNameProperty = DependencyProperty.RegisterReadOnly(
            "EntryName",
            typeof(string),
            typeof(AbilityEntryViewer),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EntryProperty = DependencyProperty.Register(
            "Entry",
            typeof(AbilityEntry), 
            typeof(AbilityEntryViewer), 
            new PropertyMetadata(null, OnEntryChanged));

        private static void OnEntryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is not AbilityEntry entry)
            {
                return;
            }
            Debug.WriteLine(entry.Name);
            d.SetValue(EntryNameProperty, entry.Name);
            d.SetValue(DescriptionProperty, entry.Description);
        }
    }
}