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
    public class AbilityListItemView : Control
    {
        static AbilityListItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityListItemView), new FrameworkPropertyMetadata(typeof(AbilityListItemView)));
        }

        public static readonly DependencyProperty RarityProperty = DependencyProperty.Register(
            "Rarity",
            typeof(AbilityRarity),
            typeof(AbilityListItemView),
            new PropertyMetadata(default(AbilityRarity)));


        public static readonly DependencyProperty AbilityNameProperty = DependencyProperty.Register(
            "AbilityName",
            typeof(string),
            typeof(AbilityListItemView),
            new PropertyMetadata(null));

        
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", 
            typeof(ImageSource),
            typeof(AbilityListItemView),
            new PropertyMetadata(null));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public AbilityRarity Rarity
        {
            get => (AbilityRarity)GetValue(RarityProperty);
            set => SetValue(RarityProperty, value);
        }
        public string AbilityName
        {
            get { return (string)GetValue(AbilityNameProperty); }
            set { SetValue(AbilityNameProperty, value); }
        }

    }
}
