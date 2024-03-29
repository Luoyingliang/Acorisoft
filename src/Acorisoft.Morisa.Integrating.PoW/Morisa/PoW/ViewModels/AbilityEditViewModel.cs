﻿using Acorisoft.Platform.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Morisa.Resources;
using Acorisoft.Platform.Windows.Controls;
using Newtonsoft.Json;
using ReactiveUI;
using System.Reactive.Subjects;
using System.Reactive;
// ReSharper disable ValueParameterNotUsed
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

namespace Acorisoft.Morisa.PoW.ViewModels
{
    public class AbilityEditViewModel : PageViewModel
    {
        private readonly ICommand _newCostEntryCommand;
        private readonly ICommand _newGeneralEntryCommand;
        private readonly ICommand _newUnlockedEntryCommand;
        private readonly ICommand _newEvolutionEntryCommand;
        private readonly ICommand _newHiddenEntryCommand;
        private readonly ICommand _removeEntryCommand;
        private readonly ICommand _clearCostEntryCommand;
        private readonly ICommand _clearGeneralEntryCommand;
        private readonly ICommand _clearUnlockedEntryCommand;
        private readonly ICommand _clearEvolutionEntryCommand;
        private readonly ICommand _clearHiddenEntryCommand;
        private readonly ICommand _loadAsJsonCommand;
        private readonly ICommand _saveAsJsonCommand;
        private readonly ISubject<Unit> _update;
        
        private AbilityDocument _document;
        private IAbilityDocument _documentWrapper;
        private DataOption _category;
        private DataOption _rarity;
        private DataOption _type;

        public AbilityEditViewModel()
        {
            _newCostEntryCommand = ReactiveCommand.Create(() => OnNewAbilityEntry(_documentWrapper.Cost));
            _newGeneralEntryCommand = ReactiveCommand.Create(() => OnNewAbilityEntry(_documentWrapper.General));
            _newUnlockedEntryCommand = ReactiveCommand.Create(() => OnNewAbilityEntry(_documentWrapper.Unlocked));
            _newEvolutionEntryCommand = ReactiveCommand.Create(() => OnNewAbilityEntry(_documentWrapper.Evolution));
            _newHiddenEntryCommand = ReactiveCommand.Create(() => OnNewAbilityEntry(_documentWrapper.Hidden));
            _clearCostEntryCommand = ReactiveCommand.Create(() => OnClearAbilityEntry(_documentWrapper.Cost));
            _clearGeneralEntryCommand = ReactiveCommand.Create(() => OnClearAbilityEntry(_documentWrapper.General));
            _clearUnlockedEntryCommand = ReactiveCommand.Create(() => OnClearAbilityEntry(_documentWrapper.Unlocked));
            _clearEvolutionEntryCommand = ReactiveCommand.Create(() => OnClearAbilityEntry(_documentWrapper.Evolution));
            _clearHiddenEntryCommand = ReactiveCommand.Create(() => OnClearAbilityEntry(_documentWrapper.Hidden));
            _removeEntryCommand = ReactiveCommand.Create<AbilityEntry>(OnRemoveAbilityEntry);
            _loadAsJsonCommand = ReactiveCommand.Create(OnLoadAsJson);
            _saveAsJsonCommand = ReactiveCommand.Create(OnSaveAsJson);
            _update = new Subject<Unit>();

            //
            // 创建一个新的能力
            OnNewAbility(new AbilityDocument
            {
                Id = Guid.NewGuid(),
                Cost = new AbilityEntryPart(),
                Hidden = new AbilityEntryPart(),
                Evolution = new AbilityEntryPart(),
                General = new AbilityEntryPart(),
                Unlocked = new AbilityEntryPart(),
            });
        }
        protected void OnLoadAsJson()
        {
            var opendlg = new OpenFileDialog
            {
                Filter = "ason|*.ason"
            };

            if (opendlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            var fileName = opendlg.FileName;
            var json = File.ReadAllText(fileName);
            try
            {
                var document = JsonConvert.DeserializeObject<AbilityDocument>(json);
                OnNewAbility(document);
            }
            catch
            {
                Debug.WriteLine("发生意外的错误");
            }
        }
        protected void OnSaveAsJson()
        {
            //
            //
            var json = JsonConvert.SerializeObject(_document);
            var savedlg = new SaveFileDialog
            {
                Filter = "ason|*.ason"
            };

            if (string.IsNullOrEmpty(json) || savedlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            var fileName = savedlg.FileName;
            File.WriteAllText(fileName, json);
        }

        protected virtual void OnChangeRarity(DataOption value)
        {
            if (value?.Data is not Rarity newRarity)
            {
                return;
            }

            _documentWrapper.Rarity = newRarity;
        }

        protected virtual void OnChangeCategory(DataOption value)
        {
            if (value?.Data is not Category newCategory)
            {
                return;
            }

            _documentWrapper.Category = newCategory;
        }
        
        protected async void OnNewAbilityEntry(AbilityEntryPart part)
        {
            var entry = await Dialog<NewAbilityEntryViewModel, AbilityEntry>();

            if (entry != null)
            {
                part.Add(entry);
            }
        }
        
        protected void OnRemoveAbilityEntry(AbilityEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            if (_document.Cost.Remove(entry))
            {
                return;
            }
            
            if (_document.General.Remove(entry))
            {
                return;
            }
            
            if (_document.Unlocked.Remove(entry))
            {
                return;
            }
            
            if (_document.Evolution.Remove(entry))
            {
                return;
            }
            
            if (_document.Hidden.Remove(entry))
            {
                return;
            }
        }
        
        protected void OnClearAbilityEntry(AbilityEntryPart part)
        {
            if (part == null)
            {
                return;
            }

            part.Clear();
        }
        

        /// <summary>
        /// 当有新的能力更新的时候使用该方法。
        /// </summary>
        /// <param name="document">新的能力文档。</param>
        protected void OnNewAbility(AbilityDocument document)
        {
            _document = document;
            _documentWrapper = new AbilityDocumentWrapper(_document);
            RaiseUpdated(nameof(Document));
            RaiseUpdated(nameof(Rarity));
            RaiseUpdated(nameof(Category));
            RaiseUpdated(nameof(Name));
            RaiseUpdated(nameof(Icon));
            RaiseUpdated(nameof(Whisper));
            RaiseUpdated(nameof(Labels));
            RaiseUpdated(nameof(Storyboard));
            RaiseUpdated(nameof(Sprit));
            RaiseUpdated(nameof(Subjectivity));
            RaiseUpdated(nameof(Cost));
            RaiseUpdated(nameof(General));
            RaiseUpdated(nameof(Hidden));
            RaiseUpdated(nameof(Unlocked));
            RaiseUpdated(nameof(Evolution));
            RaiseUpdated(nameof(Motion));
            _update.OnNext(Unit.Default);
        }

        /// <summary>
        /// 获取当前文档。
        /// </summary>
        public IAbilityDocument Document => _documentWrapper;
        public ICommand LoadAsJsonCommand => _loadAsJsonCommand;
        public ICommand SaveAsJsonCommand => _saveAsJsonCommand;
        public ICommand NewCostEntryCommand => _newCostEntryCommand;
        public ICommand NewGeneralEntryCommand => _newGeneralEntryCommand;
        public ICommand NewUnlockedEntryCommand => _newUnlockedEntryCommand;
        public ICommand NewEvolutionEntryCommand => _newEvolutionEntryCommand;
        public ICommand NewHiddenEntryCommand => _newHiddenEntryCommand;
        public ICommand RemoveEntryCommand => _removeEntryCommand;
        public ICommand ClearCostEntryCommand => _clearCostEntryCommand;
        public ICommand ClearGeneralEntryCommand => _clearGeneralEntryCommand;
        public ICommand ClearUnlockedEntryCommand => _clearUnlockedEntryCommand;
        public ICommand ClearEvolutionEntryCommand => _clearEvolutionEntryCommand;
        public ICommand ClearHiddenEntryCommand => _clearHiddenEntryCommand;
        public IObservable<Unit> Update => _update;

        public DataOption Rarity
        {
            get => _rarity;
            set
            {
                if (value?.Data is not Rarity newRarity)
                {
                    return;
                }

                _documentWrapper.Rarity = newRarity;
                _rarity = value;
            }
        }

        public DataOption Category
        {
            get => _category;
            set
            {
                if (value?.Data is not Category newCategory)
                {
                    return;
                }

                _documentWrapper.Category = newCategory;
                _category = value;
            }
        }

        public DataOption Type
        {
            get => _category;
            set
            {
                if (value?.Data is not AbilityType newType)
                {
                    return;
                }

                _documentWrapper.Type = newType;
                _type = value;
            }
        }

        #region IAbilityDocument Members

        public Guid Id
        {
            get => _documentWrapper.Id;
            set
            {
                
            }
        }

        public string Name
        {
            get => _documentWrapper.Name;
            set => _documentWrapper.Name = value;
        }

        public ImageResource Icon
        {
            get => _documentWrapper.Icon;
            set => _documentWrapper.Icon = value;
        }

        public string Whisper
        {
            get => _documentWrapper.Whisper;
            set => _documentWrapper.Whisper = value;
        }

        public string Labels
        {
            get => _documentWrapper.Labels;
            set => _documentWrapper.Labels = value;
        }


        public string Storyboard
        {
            get => _documentWrapper.Storyboard?.Name;
            set => _documentWrapper.Storyboard = new Storyboard { Name = value };
        }

        public AbilityEntryPart Cost
        {
            get => _documentWrapper.Cost;
        }

        public AbilityEntryPart General
        {
            get => _documentWrapper.General;
        }

        public AbilityEntryPart Evolution
        {
            get => _documentWrapper.Evolution;
        }

        public AbilityEntryPart Unlocked
        {
            get => _documentWrapper.Unlocked;
        }

        public AbilityEntryPart Hidden
        {
            get => _documentWrapper.Hidden;
        }

        public AbilitySpritCore Sprit
        {
            get => _documentWrapper.Sprit;
        }


        /// <summary>
        /// 动作描述
        /// </summary>
        public string Motion
        {
            get => _documentWrapper.Motion;
            set => _documentWrapper.Motion = value;
        }

        /// <summary>
        /// 主观使用描述。
        /// </summary>
        public string Subjectivity
        {
            get => _documentWrapper.Subjectivity;
            set => _documentWrapper.Subjectivity = value;
        }
        
        #endregion
    }
}
