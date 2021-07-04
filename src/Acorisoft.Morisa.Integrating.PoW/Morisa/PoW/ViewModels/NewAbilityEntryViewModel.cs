using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Morisa.PoW.ViewModels
{
    public class NewAbilityEntryViewModel : DialogViewModel
    {
        private string _name;
        private string _description;
        private readonly AbilityEntry _newEntry;

        public NewAbilityEntryViewModel()
        {
            _newEntry = new AbilityEntry();
        }

        protected sealed override bool CanFinish()
        {
            return !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_description);
        }

        protected override object GetResult()
        {
            return _newEntry;
        }

        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
                _newEntry.Name = value;
                RaiseUpdate();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                Set(ref _description, value);
                _newEntry.Description = value;
                RaiseUpdate();
            }
        }
    }
}