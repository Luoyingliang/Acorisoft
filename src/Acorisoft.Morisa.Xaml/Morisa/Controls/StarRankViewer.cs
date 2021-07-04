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

    /// <summary>
    /// <see cref="StarRankViewer"/> 类型表示一个稀有度呈现器
    /// </summary>
    public class StarRankViewer : ContentControl
    {
        static StarRankViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StarRankViewer), new FrameworkPropertyMetadata(typeof(StarRankViewer)));
        }


        private static void OnRarityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Rarity rarity)
            {
                d.SetValue(RankInternProperty, Rank.One);
            }
            else
            {

                switch (rarity.Rank)
                {
                    case 2:
                        d.SetValue(RankInternProperty, Rank.Two); break;
                    case 3:
                        d.SetValue(RankInternProperty, Rank.Three); break;
                    case 4:
                        d.SetValue(RankInternProperty, Rank.Four); break;
                    case 5:
                        d.SetValue(RankInternProperty, Rank.Five); break;
                    case 6:
                        d.SetValue(RankInternProperty, Rank.Six); break;
                    case 7:
                        d.SetValue(RankInternProperty, Rank.Seven); break;
                    case 8:
                        d.SetValue(RankInternProperty, Rank.Eight); break;
                    case 9:
                        d.SetValue(RankInternProperty, Rank.Nine); break;
                    case 10:
                        d.SetValue(RankInternProperty, Rank.Ten); break;
                    default:
                        d.SetValue(RankInternProperty, Rank.One); break;
                }
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
            typeof(StarRankViewer),
            new PropertyMetadata(null, OnRarityChanged));


        public static readonly DependencyPropertyKey RankInternProperty = DependencyProperty.RegisterReadOnly(
            "RankIntern",
            typeof(Rank),
            typeof(StarRankViewer),
            new PropertyMetadata(Rank.One));
    }
}
