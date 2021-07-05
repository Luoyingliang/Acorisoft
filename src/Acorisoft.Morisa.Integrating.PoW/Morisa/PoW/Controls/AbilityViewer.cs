using System.ComponentModel;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Morisa.Resources;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable UnusedMember.Local

namespace Acorisoft.Morisa.PoW.Controls
{
    public class AbilityRenderer : AbilityViewer
    {

    }

    public class AbilityViewer : Control
    {
        static AbilityViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityViewer), new FrameworkPropertyMetadata(typeof(AbilityViewer)));
        }

        private static void OnAbilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (AbilityViewer) d;
            
            if(e.NewValue is not IAbilityDocument ability)
            {
                d.SetValue(AbilityNameProperty, "技能名称");
                d.SetValue(AbilityRarityProperty, new Rarity { Rank = 5 });
            }
            else
            {
                if(e.OldValue is AbilityDocumentWrapper oldWrapper)
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
            SetValue(AbilityIconProperty, ability.Icon);
            SetValue(AbilityNameProperty, ability.Name);
            SetValue(AbilityRarityProperty, ability.Rarity);
            SetValue(AbiltiyStoryProperty, ability.Storyboard);
            SetValue(AbilityCostProperty, ability.Cost);
            SetValue(AbilityEvolutionProperty, ability.Evolution);
            SetValue(AbilityGeneralProperty, ability.General);
            SetValue(AbilityHiddenProperty, ability.Hidden);
            SetValue(AbilityUnlockedProperty, ability.Unlocked);
            SetValue(AbilityCategoryProperty, ability.Category);
            SetValue(AbilityLabelsProperty, ability.Labels);
            SetValue(AbilityWhisperProperty, ability.Whisper);
            SetValue(AbilityTypeProperty, ability.Type);
            SetValue(AbilityMotionProperty, ability.Motion);
            SetValue(AbilitySubjectivityProperty, ability.Subjectivity);
        }

        #region DependencyProperty

        public IAbilityDocument Ability
        {
            get => (IAbilityDocument)GetValue(AbilityProperty);
            set => SetValue(AbilityProperty, value);
        }

        public ControlSize SizeMode
        {
            get => (ControlSize)GetValue(SizeModeProperty);
            set => SetValue(SizeModeProperty, value);
        }

        public static readonly DependencyProperty AbilityProperty = DependencyProperty.Register(
            "Ability",
            typeof(IAbilityDocument),
             typeof(AbilityViewer),
             new PropertyMetadata(null, OnAbilityChanged));


        public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(
            "SizeMode",
            typeof(ControlSize),
            typeof(AbilityViewer),
            new PropertyMetadata(default(ControlSize)));

        #endregion

        #region ReadonlyDependencyProperty



        public string AbilityName
        {
            get => (string)GetValue(AbilityNameProperty.DependencyProperty);
            private set => SetValue(AbilityNameProperty, value);
        }


        public ImageResource AbilityIcon
        {
            get => (ImageResource)GetValue(AbilityIconProperty.DependencyProperty);
            private set => SetValue(AbilityIconProperty, value);
        }


        public Storyboard AbiltiyStory
        {
            get => (Storyboard)GetValue(AbiltiyStoryProperty.DependencyProperty);
            private set => SetValue(AbiltiyStoryProperty, value);
        }
        
        public Rarity AbilityRarity
        {
            get => (Rarity)GetValue(AbilityRarityProperty.DependencyProperty);
            private set => SetValue(AbilityRarityProperty, value);
        }
        
        public AbilityEntryPart AbilityGeneral
        {
            get => (AbilityEntryPart)GetValue(AbilityGeneralProperty.DependencyProperty);
            private set => SetValue(AbilityGeneralProperty, value);
        }
        
        public AbilityEntryPart AbilityCost
        {
            get => (AbilityEntryPart)GetValue(AbilityCostProperty.DependencyProperty);
            private set => SetValue(AbilityCostProperty, value);
        }
        
        public AbilityEntryPart AbilityHidden
        {
            get => (AbilityEntryPart)GetValue(AbilityHiddenProperty.DependencyProperty);
            private set => SetValue(AbilityHiddenProperty, value);
        }
        
        public AbilityEntryPart AbilityEvolution
        {
            get => (AbilityEntryPart)GetValue(AbilityEvolutionProperty.DependencyProperty);
            private set => SetValue(AbilityEvolutionProperty, value);
        }
        
        public AbilityEntryPart AbilityUnlocked
        {
            get => (AbilityEntryPart)GetValue(AbilityUnlockedProperty.DependencyProperty);
            private set => SetValue(AbilityUnlockedProperty, value);
        }
        
        public string AbilityWhisper
        {
            get => (string)GetValue(AbilityWhisperProperty.DependencyProperty);
            private set => SetValue(AbilityWhisperProperty, value);
        }
        
        public string AbilityLabels
        {
            get => (string)GetValue(AbilityLabelsProperty.DependencyProperty);
            private set => SetValue(AbilityLabelsProperty, value);
        }
        
        public Category AbilityCategory
        {
            get => (Category)GetValue(AbilityCategoryProperty.DependencyProperty);
            private set => SetValue(AbilityCategoryProperty, value);
        }

        public AbilityType AbilityType
        {
            get => (AbilityType)GetValue(AbilityTypeProperty.DependencyProperty);
            private set => SetValue(AbilityTypeProperty, value);
        }

        public string AbilityMotion
        {
            get => (string)GetValue(AbilityMotionProperty.DependencyProperty);
            private set => SetValue(AbilityMotionProperty, value);
        }

        public string AbilitySubjectivity
        {
            get => (string)GetValue(AbilitySubjectivityProperty.DependencyProperty);
            private set => SetValue(AbilitySubjectivityProperty, value);
        }

        public static readonly DependencyPropertyKey AbilitySubjectivityProperty = DependencyProperty.RegisterReadOnly(
            "AbilitySubjectivity",
            typeof(string),
             typeof(AbilityViewer),
             new PropertyMetadata(null));

        public static readonly DependencyPropertyKey AbilityMotionProperty = DependencyProperty.RegisterReadOnly(
            "AbilityMotion",
            typeof(string),
             typeof(AbilityViewer),
             new PropertyMetadata(null));


        public static readonly DependencyPropertyKey AbilityTypeProperty = DependencyProperty.RegisterReadOnly(
            "AbilityType",
            typeof(AbilityType),
             typeof(AbilityViewer),
             new PropertyMetadata(default(AbilityType)));



        private static readonly DependencyPropertyKey AbilityCategoryProperty = DependencyProperty.RegisterReadOnly(
            "AbilityCategory",
            typeof(Category),
            typeof(AbilityViewer),
            new PropertyMetadata(default(Category)));
        
        private static readonly DependencyPropertyKey AbilityLabelsProperty = DependencyProperty.RegisterReadOnly(
            "AbilityLabels",
            typeof(string),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityWhisperProperty = DependencyProperty.RegisterReadOnly(
            "AbilityWhisper",
            typeof(string),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityUnlockedProperty = DependencyProperty.RegisterReadOnly(
            "AbilityUnlocked",
            typeof(AbilityEntryPart),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityEvolutionProperty = DependencyProperty.RegisterReadOnly(
            "AbilityEvolution",
            typeof(AbilityEntryPart),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityHiddenProperty = DependencyProperty.RegisterReadOnly(
            "AbilityHidden",
            typeof(AbilityEntryPart),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityCostProperty = DependencyProperty.RegisterReadOnly(
            "AbilityCost",
            typeof(AbilityEntryPart),
            typeof(AbilityViewer),
            new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityGeneralProperty = DependencyProperty.RegisterReadOnly(
            "AbilityGeneral",
            typeof(AbilityEntryPart),
            typeof(AbilityViewer),
            new PropertyMetadata(null));

        private static readonly DependencyPropertyKey AbilityRarityProperty = DependencyProperty.RegisterReadOnly(
            "AbilityRarity",
            typeof(Rarity),
             typeof(AbilityViewer),
             new PropertyMetadata(new Rarity { Rank = 1 }));

        private static readonly DependencyPropertyKey AbiltiyStoryProperty = DependencyProperty.RegisterReadOnly(
            "AbiltiyStory",
            typeof(Storyboard),
             typeof(AbilityViewer),
             new PropertyMetadata(null));
        
        private static readonly DependencyPropertyKey AbilityIconProperty = DependencyProperty.RegisterReadOnly(
            "AbilityIcon",
            typeof(ImageResource),
             typeof(AbilityViewer),
             new PropertyMetadata(null));

        private static readonly DependencyPropertyKey AbilityNameProperty = DependencyProperty.RegisterReadOnly(
            "AbilityName",
            typeof(string),
             typeof(AbilityViewer),
             new PropertyMetadata("技能名称"));



        #endregion
    }
}
