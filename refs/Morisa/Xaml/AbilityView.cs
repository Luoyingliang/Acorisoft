using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Acorisoft.Morisa.Xaml
{
    public abstract class AbilityView : Control
    {
        private static void OnAbilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrapper = e.NewValue as AbilityWrapper;
            var ability = e.NewValue as Ability;

            if (wrapper == null && ability == null)
            {
                return;
            }

            if(ReferenceEquals(e.OldValue, e.NewValue))
            {
                return;
            }

            
            d.SetValue(TitleProperty, wrapper?.Source.Name ?? ability.Name);
            d.SetValue(CategoryProperty, wrapper?.Source.Category ?? ability.Category);
            d.SetValue(CostProperty, wrapper?.Source.Cost ?? ability.Cost);
            d.SetValue(RegularProperty, wrapper?.Source.Regular ?? ability.Regular);
            d.SetValue(HiddenProperty, wrapper?.Source.Hidden ?? ability.Hidden);
            d.SetValue(UnlockProperty, wrapper?.Source.Unlock ?? ability.Unlock);
            d.SetValue(EvolutionProperty, wrapper?.Source.Evolution ?? ability.Evolution);
            d.SetValue(ZoneProperty, wrapper?.Source.Zone ?? ability.Zone);
            d.SetValue(EmotionProperty, wrapper?.Source.Name ?? ability.Emotion);
            d.SetValue(StorySetProperty, wrapper?.Source.StorySet ?? ability.StorySet);
            d.SetValue(LabelsProperty, wrapper?.Source.Labels ?? ability.Labels);
            d.SetValue(IconProperty, wrapper?.Source.Icon ?? ability.Icon);
            d.SetValue(RarityProperty, wrapper?.Source.Rarity ?? ability.Rarity);
        }



        public object Ability
        {
            get { return (object)GetValue(AbilityProperty); }
            set { SetValue(AbilityProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty.DependencyProperty); }
            private set { SetValue(IconProperty, value); }
        }

        public AbilityRarity Rarity
        {
            get => (AbilityRarity)GetValue(RarityProperty.DependencyProperty);
            private set => SetValue(RarityProperty, value);
        }

        public IEnumerable<AbilityEntry> Cost
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(CostProperty.DependencyProperty); }
            set { SetValue(CostProperty, value); }
        }

        public IEnumerable<AbilityEntry> Regular
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(RegularProperty.DependencyProperty); }
            set { SetValue(RegularProperty, value); }
        }

        public IEnumerable<AbilityEntry> Hidden
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(HiddenProperty.DependencyProperty); }
            set { SetValue(HiddenProperty, value); }
        }
        public IEnumerable<AbilityEntry> Unlock
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(UnlockProperty.DependencyProperty); }
            set { SetValue(UnlockProperty, value); }
        }

        public IEnumerable<AbilityEntry> Evolution
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(EvolutionProperty.DependencyProperty); }
            set { SetValue(EvolutionProperty, value); }
        }

        public IEnumerable<AbilityEntry> Zone
        {
            get { return (IEnumerable<AbilityEntry>)GetValue(ZoneProperty.DependencyProperty); }
            set { SetValue(ZoneProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty.DependencyProperty); }
            private set { SetValue(TitleProperty, value); }
        }

        public string Labels
        {
            get { return (string)GetValue(LabelsProperty.DependencyProperty); }
            private set { SetValue(LabelsProperty, value); }
        }

        public string Emotion
        {
            get { return (string)GetValue(EmotionProperty.DependencyProperty); }
            private set { SetValue(EmotionProperty, value); }
        }

        public string StorySet
        {
            get { return (string)GetValue(StorySetProperty.DependencyProperty); }
            private set { SetValue(StorySetProperty, value); }
        }
        public AbilityCategory Category
        {
            get { return (AbilityCategory)GetValue(CategoryProperty.DependencyProperty); }
            private set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyPropertyKey CategoryProperty = DependencyProperty.RegisterReadOnly(
            "Category",
            typeof(AbilityCategory),
            typeof(AbilityView),
            new PropertyMetadata(default(AbilityCategory)));

        public static readonly DependencyPropertyKey LabelsProperty = DependencyProperty.RegisterReadOnly(
            "Labels", 
            typeof(string),
            typeof(AbilityView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey EmotionProperty = DependencyProperty.RegisterReadOnly(
            "Emotion",
            typeof(string),
            typeof(AbilityView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey StorySetProperty = DependencyProperty.RegisterReadOnly(
               "StorySet",
               typeof(AbilityStorySet),
               typeof(AbilityView),
               new PropertyMetadata(null));



        public static readonly DependencyProperty AbilityProperty = DependencyProperty.Register(
            "Ability",
            typeof(object), 
            typeof(AbilityView),
            new PropertyMetadata(null, OnAbilityChanged));



        public static readonly DependencyPropertyKey RarityProperty = DependencyProperty.RegisterReadOnly(
            "Rarity",
            typeof(AbilityRarity),
            typeof(AbilityView),
            new PropertyMetadata(default(AbilityRarity)));


        public static readonly DependencyPropertyKey TitleProperty = DependencyProperty.RegisterReadOnly(
            "Title",
            typeof(string),
            typeof(AbilityView),
            new PropertyMetadata(null));


        public static readonly DependencyPropertyKey IconProperty = DependencyProperty.RegisterReadOnly(
            "Icon",
            typeof(ImageSource),
            typeof(AbilityView),
            new PropertyMetadata(null));


        public static readonly DependencyPropertyKey CostProperty = DependencyProperty.RegisterReadOnly(
            "Cost",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey RegularProperty = DependencyProperty.RegisterReadOnly(
            "Regular",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey HiddenProperty = DependencyProperty.RegisterReadOnly(
            "Hidden",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));


        public static readonly DependencyPropertyKey UnlockProperty = DependencyProperty.RegisterReadOnly(
            "Unlock",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));

        public static readonly DependencyPropertyKey EvolutionProperty = DependencyProperty.RegisterReadOnly(
            "Evolution",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));



        public static readonly DependencyPropertyKey ZoneProperty = DependencyProperty.RegisterReadOnly(
            "Zone",
            typeof(IEnumerable<AbilityEntry>),
            typeof(AbilityView),
            new PropertyMetadata(null));
    }
}
