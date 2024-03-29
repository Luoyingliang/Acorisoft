﻿using Acorisoft.Morisa.PoW.ViewModels;
using Acorisoft.Platform.Windows.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
using ReactiveUI;
using Acorisoft.Platform.Windows.Controls;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Morisa.Documents;
using Acorisoft.Platform.Windows;
using System.Windows.Forms;
using System.IO;

namespace Acorisoft.Morisa.PoW.Views
{
    /// <summary>
    /// Interaction logic for AbilityEditView.xaml
    /// </summary>
    [ViewModelParing(typeof(AbilityEditViewModel), typeof(AbilityEditView))]
    public partial class AbilityEditView : PageView<AbilityEditViewModel>
    {
        public AbilityEditView()
        {
            InitializeComponent();
        }

        protected override void OnBindingDataContext(IObservable<AbilityEditViewModel> sequence)
        {
            if(ViewModel is null)
            {
                return;
            }

            ViewModel.Update.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x =>
            {
                var index = 0;
                foreach(DataOption item in Category.Items)
                {
                    if(item?.Data is Category category && category == ViewModel.Document.Category)
                    {
                        Category.SelectedIndex = index;
                    }

                    index++;
                }

                index = 0;

                foreach (DataOption item in Rarity.Items)
                {
                    if (item?.Data is Rarity rarity && rarity.Rank == ViewModel.Document.Rarity.Rank)
                    {
                        Rarity.SelectedIndex = index;
                    }

                    index++;
                }

                index = 0;

                foreach (DataOption item in Type.Items)
                {
                    if (item?.Data is AbilityType type && type == ViewModel.Document.Type)
                    {
                        Type.SelectedIndex = index;
                    }

                    index++;
                }
            });
        }

        private void MakeSnapshot(object sender, RoutedEventArgs e)
        {
            var savedlg = new SaveFileDialog 
            {
                Filter = "PNG 文件|*.png"
            }; 

            if(savedlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var bitmap = Interop.Snapshot(Viewer);
            var encoder = new PngBitmapEncoder();


            using (var fs = new FileStream(savedlg.FileName, FileMode.Create))
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fs);

            }           
        }
    }
}
