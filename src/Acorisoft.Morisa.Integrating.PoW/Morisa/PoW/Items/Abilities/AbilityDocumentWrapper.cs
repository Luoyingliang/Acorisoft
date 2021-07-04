using System;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.Resources;
// ReSharper disable MemberCanBePrivate.Global

namespace Acorisoft.Morisa.PoW.Items.Abilities
{
    public class AbilityDocumentWrapper : ItemDocumentWrapper , IAbilityDocument
    {
        public AbilityDocumentWrapper(AbilityDocument document)
        {
            Source = document;
        }
        
        protected AbilityDocument Source { get; }

        public sealed override string ToString()
        {
            return Source.Name;
        }

        public Guid Id
        {
            get => Source.Id;
            set
            {
                
            }
        }

        public string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            } 
        }

        public ImageResource Icon
        {
            get => Source.Icon;
            set {
                Source.Icon = value;
                RaiseUpdated();
            } 
        }

        public string Whisper
        {
            get => Source.Whisper;
            set {
                Source.Whisper = value;
                RaiseUpdated();
            } 
        }

        public Rarity Rarity
        {
            get => Source.Rarity;
            set {
                Source.Rarity = value;
                RaiseUpdated();
            } 
        }

        public string Labels
        {
            get => Source.Labels;
            set {
                Source.Labels = value;
                RaiseUpdated();
            } 
        }

        public Category Category
        {
            get => Source.Category;
            set {
                Source.Category = value;
                RaiseUpdated();
            } 
        }

        public Storyboard Storyboard
        {
            get => Source.Storyboard;
            set {
                Source.Storyboard = value;
                RaiseUpdated();
            } 
        }

        public AbilityEntryPart Cost
        {
            get => Source.Cost;
            set {
                Source.Cost = value;
                RaiseUpdated();
            } 
        }

        public AbilityEntryPart General
        {
            get => Source.General;
            set {
                Source.General = value;
                RaiseUpdated();
            } 
        }

        public AbilityEntryPart Evolution
        {
            get => Source.Evolution;
            set {
                Source.Evolution = value;
                RaiseUpdated();
            } 
        }

        public AbilityEntryPart Unlocked
        {
            get => Source.Unlocked;
            set {
                Source.Unlocked = value;
                RaiseUpdated();
            } 
        }

        public AbilityEntryPart Hidden
        {
            get => Source.Hidden;
            set {
                Source.Hidden = value;
                RaiseUpdated();
            } 
        }

        public AbilitySpritCore Sprit
        {
            get => Source.Sprit;
            set {
                Source.Sprit = value;
                RaiseUpdated();
            } 
        }

        /// <summary>
        ///
        /// </summary>
        public string Motion
        {
            get => Source.Motion;
            set
            {
                Source.Motion = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string Subjectivity
        {
            get => Source.Subjectivity;
            set
            {
                Source.Subjectivity = value;
                RaiseUpdated();
            }
        }
    }
}