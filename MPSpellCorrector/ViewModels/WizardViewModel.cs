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
    public class WizardViewModel : ViewModelBase
    {

        private readonly CollectionView collection;
        private Dictionary selectedDictionary = null;
        private string sourceDirectory;
        private string destinationDirectory;
        private bool preserveSubfolders;
        private string[] sourceFiles = null;
        private string customDictionary;

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

        public bool PreserveSubfolders
        {
            get
            {
                return preserveSubfolders;
            }

            set
            {
                preserveSubfolders = value;
            }
        }

        public string SourceFilesCount
        {
            get
            {
                if (null != sourceFiles)
                {
                    string res = sourceFiles.Length.ToString();
                    res += sourceFiles.Length == 1 ? " file" : " files";

                    return res;
                }

                return "";
            }
        }

        public bool CanFinish
        {
            get
            {
                if (!String.IsNullOrEmpty(destinationDirectory) &&
                    (sourceFiles != null || !String.IsNullOrEmpty(sourceDirectory)))
                {

                    return true;
                }

                return false;
            }


        }

        public string CustomDictionary
        {
            get
            {
                if (String.IsNullOrEmpty(customDictionary))
                {
                    return "-";
                }

                return customDictionary;
            }

            set
            {
                customDictionary = value;
                OnPropertyChanged("CustomDictionary");
            }
        }

        public string Source
        {
            get
            {
                if (!String.IsNullOrEmpty(SourceDirectory))
                {
                    return SourceDirectory;
                }

                return SourceFilesCount;
            }
        }

        public string[] SourceFiles
        {
            get
            {
                return sourceFiles;
            }

            set
            {
                sourceFiles = value;

                OnPropertyChanged("SourceFiles");
                OnPropertyChanged("SourceFilesCount");
                OnPropertyChanged("Source");
                OnPropertyChanged("CanFinish");
            }
        }

        public string SourceDirectory
        {
            get
            {
                return sourceDirectory;
            }

            set
            {
                sourceDirectory = value;
                OnPropertyChanged("SourceDirectory");
                OnPropertyChanged("Source");
                OnPropertyChanged("CanFinish");
            }
        }

        public string DestinationDirectory
        {
            get
            {
                return destinationDirectory;
            }

            set
            {
                destinationDirectory = value;
                OnPropertyChanged("DestinationDirectory");
                OnPropertyChanged("CanFinish");
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

    }
}
