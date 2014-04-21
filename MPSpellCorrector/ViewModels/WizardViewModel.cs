using MPSpell.Dictionaries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MPSpellCorrector.ViewModels
{
    public class WizardViewModel
    {

        private readonly CollectionView collection;
        private Dictionary selectedDictionary = null;

        public WizardViewModel(List<Dictionary> dictionaries)
        {
            collection = new CollectionView(dictionaries);

        }

        public CollectionView Dictionaries
        {
            get
            {
                return collection;
            }
        }

        public Dictionary SelectedItem
        {
            get
            {
                return selectedDictionary;
            }

            set
            {
                if (selectedDictionary == value)
                    return;

                selectedDictionary = value;
                
                OnPropertyChanged("Name");
            }
        }

        public string Name
        {
            get
            {
                if (null == this.selectedDictionary)
                {
                    return string.Empty;
                }

                return this.selectedDictionary.Name;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
