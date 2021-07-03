using Acorisoft.Morisa.Documents;
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

namespace Acorisoft.Morisa.Controls
{
    public enum Rank
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten
    }

    /// <summary>
    /// <see cref="RankViewer"/> 类型表示一个稀有度呈现器
    /// </summary>
    public class RankViewer : ContentControl
    {
        static RankViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RankViewer), new FrameworkPropertyMetadata(typeof(RankViewer)));
        }


        private static void OnRarityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Rarity rarity)
            {
                return;
            }

            switch (rarity.Rank)
            {
                case 2:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Two); break;
                case 3:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Three); break;
                case 4:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Four); break;
                case 5:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Five); break;
                case 6:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Six); break;
                case 7:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Seven); break;
                case 8:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Eight); break;
                case 9:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Nine); break;
                case 10:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.Ten); break;
                default:
                    d.SetValue(RankInternProperty, Acorisoft.Morisa.Controls.Rank.One); break;
            }
        }

        public Rank RankIntern
        {
            get { return (Rank)GetValue(RankInternProperty.DependencyProperty); }
            private set { SetValue(RankInternProperty, value); }
        }
        public Rarity Rarity
        {
            get { return (Rarity)GetValue(RarityProperty); }
            set { SetValue(RarityProperty, value); }
        }


        public static readonly DependencyProperty RarityProperty = DependencyProperty.Register(
            "Rarity",
            typeof(Rarity),
            typeof(RankViewer),
            new PropertyMetadata(null, OnRarityChanged));


        public static readonly DependencyPropertyKey RankInternProperty = DependencyProperty.RegisterReadOnly(
            "RankIntern",
            typeof(Rank),
            typeof(RankViewer),
            new PropertyMetadata(Morisa.Controls.Rank.One));
    }
}
