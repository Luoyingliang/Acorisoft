using Acorisoft.Morisa.Controls;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Morisa.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var viewer = (ItemIconViewer)d;

            if (e.NewValue is not IAbilityDocument ability)
            {
                d.SetValue(RarityProperty, Rank.One);
            }
            else
            {
                if (e.OldValue is AbilityDocumentWrapper oldWrapper)
                {
                    oldWrapper.PropertyChanged -= viewer.PerformanceDocumentChanged;
                }

                if (e.NewValue is AbilityDocumentWrapper newWrapper)
                {
                    newWrapper.PropertyChanged += viewer.PerformanceDocumentChanged;
                }
                viewer.PerformanceDocumentChanged(ability, new PropertyChangedEventArgs(""));
            }
        }
        
        private void PerformanceDocumentChanged(object sender, PropertyChangedEventArgs e)
        {
            var ability = (IAbilityDocument) sender;
            var rank = ability?.Rarity?.Rank ?? 1;
            switch (rank)
            {
                case 2:
                    SetValue(RarityProperty, Rank.Two); break;
                case 3:
                    SetValue(RarityProperty, Rank.Three); break;
                case 4:
                    SetValue(RarityProperty, Rank.Four); break;
                case 5:
                    SetValue(RarityProperty, Rank.Five); break;
                case 6:
                    SetValue(RarityProperty, Rank.Six); break;
                case 7:
                    SetValue(RarityProperty, Rank.Seven); break;
                case 8:
                    SetValue(RarityProperty, Rank.Eight); break;
                case 9:
                    SetValue(RarityProperty, Rank.Nine); break;
                case 10:
                    SetValue(RarityProperty, Rank.Ten); break;
                default:
                    SetValue(RarityProperty, Rank.One); break;
            }

            SetValue(CategoryProperty, ability.Category);
            SetValue(IconProperty, ability.Icon);
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
