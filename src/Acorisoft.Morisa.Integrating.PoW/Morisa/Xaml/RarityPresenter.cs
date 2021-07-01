using System.Windows;
using System.Windows.Controls;
using Acorisoft.Morisa.Documents;

namespace Acorisoft.Morisa.Xaml
{
    
    
    public class RarityPresenter : Control
    {
        static RarityPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RarityPresenter), new FrameworkPropertyMetadata(typeof(RarityPresenter)));
        }

        private static void OnRarityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not AbilityRarity rarity)
            {
                return;
            }
            
            switch (rarity.Value)
            {
                case 2:
                    d.SetValue(RarityProperty, Rarity.Two); break;
                case 3:
                    d.SetValue(RarityProperty, Rarity.Three); break;
                case 4:
                    d.SetValue(RarityProperty, Rarity.Four); break;
                case 5:
                    d.SetValue(RarityProperty, Rarity.Five); break;
                default:
                    d.SetValue(RarityProperty, Rarity.One); break;
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(AbilityRarity),
            typeof(RarityPresenter),
            new PropertyMetadata(default(AbilityRarity), OnRarityChanged));

        private static readonly DependencyPropertyKey RarityProperty = DependencyProperty.RegisterReadOnly("Rarity", 
            typeof(Rarity), 
            typeof(RarityPresenter), 
            new PropertyMetadata(default(Rarity)));
        
        public Rarity Rarity
        {
            get => (Rarity) GetValue(RarityProperty.DependencyProperty);
            private set => SetValue(RarityProperty, value);
        }

        public AbilityRarity Value
        {
            get => (AbilityRarity) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}