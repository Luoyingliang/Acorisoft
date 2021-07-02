using Acorisoft.ComponentModel;
using Acorisoft.Morisa.Resources;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Documents
{
    public class AbilityWrapper : Bindable
    {
        private readonly ObservableCollectionExtended<AbilityEntry> _regular;
        private readonly ObservableCollectionExtended<AbilityEntry> _unlock;
        private readonly ObservableCollectionExtended<AbilityEntry> _hidden;
        private readonly ObservableCollectionExtended<AbilityEntry> _evolution;
        private readonly ObservableCollectionExtended<AbilityEntry> _zone;
        private readonly ObservableCollectionExtended<AbilityEntry> _cost;

        public AbilityWrapper(Ability ability)
        {
            Source = ability ?? throw new ArgumentNullException(nameof(ability));
            _cost = new ObservableCollectionExtended<AbilityEntry>(Source.Cost);
            _regular = new ObservableCollectionExtended<AbilityEntry>(Source.Regular);
            _unlock = new ObservableCollectionExtended<AbilityEntry>(Source.Unlock);
            _hidden = new ObservableCollectionExtended<AbilityEntry>(Source.Hidden);
            _evolution = new ObservableCollectionExtended<AbilityEntry>(Source.Evolution);
            _zone = new ObservableCollectionExtended<AbilityEntry>(Source.Zone);
        }


        protected internal Ability Source { get; }

        public Guid Id { get => Source.Id; }

        /// <summary>
        /// 获取或设置能力的名称。
        /// </summary>
        public string Name 
        { 
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置情绪。
        /// </summary>
        public string Emotion
        {
            get => Source.Emotion;
            set
            {
                Source.Emotion = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置能力的图标。
        /// </summary>
        public DocumentImageResource Icon
        {
            get => Source.Icon;
            set
            {
                Source.Icon = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置能力的分类。
        /// </summary>
        public AbilityCategory Category
        {
            get => Source.Category;
            set
            {
                Source.Category = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置能力的标签。
        /// </summary>
        /// <remarks>
        /// 例如：
        /// <para>战斗系、飞行、</para>
        /// </remarks>
        public AbilityLabel Labels
        {
            get => Source.Labels;
            set
            {
                Source.Labels = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置能力的稀有度。
        /// </summary>
        public AbilityRarity Rarity
        {
            get => Source.Rarity;
            set
            {
                Source.Rarity = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbilityEntry> Cost {
            get => _cost;
            set {
                if(value is not null)
                {
                    _cost.AddRange(value);
                }
            } 
        }

        /// <summary>
        /// 常规部分
        /// </summary>
        public IEnumerable<AbilityEntry> Regular
        {
            get => _regular;
            set
            {
                if (value is not null)
                {
                    _regular.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 解锁部分
        /// </summary>
        public IEnumerable<AbilityEntry> Unlock
        {
            get => _unlock;
            set
            {
                if (value is not null)
                {
                    _unlock.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 进化部分
        /// </summary>
        public IEnumerable<AbilityEntry> Evolution
        {
            get => _evolution;
            set
            {
                if (value is not null)
                {
                    _evolution.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 隐藏部分
        /// </summary>
        public IEnumerable<AbilityEntry> Hidden
        {
            get => _hidden;
            set
            {
                if (value is not null)
                {
                    _hidden.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbilityEntry> Zone
        {
            get => _zone;
            set
            {
                if (value is not null)
                {
                    _zone.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 故事集
        /// </summary>
        public AbilityStorySet StorySet
        {
            get => Source.StorySet;
            set
            {
                Source.StorySet = value;
                RaiseUpdated();
            }
        }
    }
}
