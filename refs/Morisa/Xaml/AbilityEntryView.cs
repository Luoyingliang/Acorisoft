using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.Morisa.Xaml
{
    public class AbilityEntryView : Control
    {
        static AbilityEntryView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityEntryView), new FrameworkPropertyMetadata(typeof(AbilityEntryView)));
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

        public string Motion
        {
            get { return (string)GetValue(MotionProperty.DependencyProperty); }
            private set { SetValue(MotionProperty, value); }
        }

        public string Subjectivity
        {
            get { return (string)GetValue(SubjectivityProperty.DependencyProperty); }
            private set { SetValue(SubjectivityProperty, value); }
        }


        public static readonly DependencyPropertyKey SubjectivityProperty = DependencyProperty.RegisterReadOnly(
            "Subjectivity",
            typeof(string),
            typeof(AbilityEntryView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey MotionProperty = DependencyProperty.RegisterReadOnly(
            "Motion",
            typeof(string),
            typeof(AbilityEntryView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey DescriptionProperty = DependencyProperty.RegisterReadOnly(
            "Description",
            typeof(string),
            typeof(AbilityEntryView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey EntryNameProperty = DependencyProperty.RegisterReadOnly(
            "EntryName",
            typeof(string),
            typeof(AbilityEntryView),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EntryProperty = DependencyProperty.Register(
            "Entry",
            typeof(AbilityEntry), 
            typeof(AbilityEntryView), 
            new PropertyMetadata(null, OnEntryChanged));

        private static void OnEntryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is not AbilityEntry entry)
            {
                return;
            }

            d.SetValue(EntryNameProperty, entry.Name);
            d.SetValue(MotionProperty, entry.Motion);
            d.SetValue(DescriptionProperty, entry.Description);
            d.SetValue(SubjectivityProperty, entry.Subjectivity);
        }
    }
}
