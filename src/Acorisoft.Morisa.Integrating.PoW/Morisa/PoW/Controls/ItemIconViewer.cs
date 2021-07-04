using Acorisoft.Morisa.Controls;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Morisa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Acorisoft.Morisa.PoW.Controls
{
    /// <summary>
    /// 表示一个技能图标查看器。
    /// </summary>
    public class ItemIconViewer : Control
    {
        static ItemIconViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemIconViewer), new FrameworkPropertyMetadata(typeof(ItemIconViewer)));
        }

        private static void OnAbilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not IAbilityDocument ability)
            {
                return;
            }

            switch (ability.Rarity.Rank)
            {
                case 2:
                    d.SetValue(RarityProperty, Rank.Two); break;
                case 3:
                    d.SetValue(RarityProperty, Rank.Three); break;
                case 4:
                    d.SetValue(RarityProperty, Rank.Four); break;
                case 5:
                    d.SetValue(RarityProperty, Rank.Five); break;
                case 6:
                    d.SetValue(RarityProperty, Rank.Six); break;
                case 7:
                    d.SetValue(RarityProperty, Rank.Seven); break;
                case 8:
                    d.SetValue(RarityProperty, Rank.Eight); break;
                case 9:
                    d.SetValue(RarityProperty, Rank.Nine); break;
                case 10:
                    d.SetValue(RarityProperty, Rank.Ten); break;
                default:
                    d.SetValue(RarityProperty, Rank.One); break;
            }

            d.SetValue(CategoryProperty, ability.Category);
            d.SetValue(IconProperty, ability.Icon);
        }

        public ControlSize SizeMode
        {
            get => (ControlSize)GetValue(SizeModeProperty);
            set => SetValue(SizeModeProperty, value);
        }

        public ImageResource Icon
        {
            get => (ImageResource)GetValue(IconProperty.DependencyProperty);
            private set => SetValue(IconProperty, value);
        }


        public Rank Rarity
        {
            get => (Rank)GetValue(RarityProperty.DependencyProperty);
            private set => SetValue(RarityProperty, value);
        }



        public Category Category
        {
            get => (Category)GetValue(CategoryProperty.DependencyProperty);
            private set => SetValue(CategoryProperty, value);
        }




        public IAbilityDocument Ability
        {
            get => (IAbilityDocument)GetValue(AbilityProperty);
            set => SetValue(AbilityProperty, value);
        }

        public static readonly DependencyProperty AbilityProperty = DependencyProperty.Register(
            "Ability",
            typeof(IAbilityDocument),
             typeof(ItemIconViewer),
             new PropertyMetadata(null, OnAbilityChanged));



        public static readonly DependencyPropertyKey CategoryProperty = DependencyProperty.RegisterReadOnly(
            "Category",
            typeof(Category),
             typeof(ItemIconViewer),
             new PropertyMetadata(default(Category)));



        public static readonly DependencyPropertyKey RarityProperty = DependencyProperty.RegisterReadOnly(
            "Rarity",
            typeof(Rank),
             typeof(ItemIconViewer),
             new PropertyMetadata(Rank.One));


        public static readonly DependencyPropertyKey IconProperty = DependencyProperty.RegisterReadOnly(
            "Icon",
            typeof(ImageResource),
             typeof(ItemIconViewer),
             new PropertyMetadata(null));



        public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(
            "SizeMode", 
            typeof(ControlSize),
            typeof(ItemIconViewer), 
            new PropertyMetadata(default(ControlSize)));
    }
}
