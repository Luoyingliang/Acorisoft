using System.Windows;
using System.Windows.Controls;
using Acorisoft.Morisa.Documents;

namespace Acorisoft.Morisa.Xaml
{
    
    
    public class RarityView : ContentControl
    {
        static RarityView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RarityView), new FrameworkPropertyMetadata(typeof(RarityView)));
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
                    d.SetValue(RarityValueProperty, Acorisoft.Morisa.Documents.Rarity.Two); break;
                case 3:
                    d.SetValue(RarityValueProperty, Acorisoft.Morisa.Documents.Rarity.Three); break;
                case 4:
                    d.SetValue(RarityValueProperty, Acorisoft.Morisa.Documents.Rarity.Four); break;
                case 5:
                    d.SetValue(RarityValueProperty, Acorisoft.Morisa.Documents.Rarity.Five); break;
                default:
                    d.SetValue(RarityValueProperty, Acorisoft.Morisa.Documents.Rarity.One); break;
            }
        }

        public static readonly DependencyProperty RarityProperty = DependencyProperty.Register("Rarity",
            typeof(AbilityRarity),
            typeof(RarityView),
            new PropertyMetadata(default(AbilityRarity), OnRarityChanged));

        private static readonly DependencyPropertyKey RarityValueProperty = DependencyProperty.RegisterReadOnly("RarityValue", 
            typeof(Rarity), 
            typeof(RarityView), 
            new PropertyMetadata(default(Rarity)));
        
        public AbilityRarity Rarity
        {
            get => (AbilityRarity) GetValue(RarityProperty);
            set => SetValue(RarityProperty, value);
        }

        public Rarity RarityValue
        {
            get => (Rarity) GetValue(RarityValueProperty.DependencyProperty);
            private set => SetValue(RarityValueProperty, value);
        }
    }
}