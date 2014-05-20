using MPSpellCorrector.Class;
using MPSpellCorrector.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MPSpellCorrector
{
    /// <summary>
    /// Interaction logic for CorrectionWizard.xaml
    /// </summary>
    public partial class CorrectionWizard : Window
    {
        private Container container;
        private Project project;

        private MainWindow mainWindow;
        private WizardViewModel wizardViewModel;

        public CorrectionWizard(MainWindow window)
        {
            InitializeComponent();

            mainWindow = window;
            container = window.Container;
            
            project = new Project();

            wizardViewModel = new WizardViewModel(container.GetDictionaryManager().GetAvailableDictionaries());
            this.Wizard.DataContext = wizardViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(container.Settings.DataFolder))
            {
                dialog.SelectedPath = container.Settings.DataFolder;
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {                
                project.SourcePath = dialog.SelectedPath;
                container.Settings.DataFolder = dialog.SelectedPath;

                wizardViewModel.SourceDirectory = project.SourcePath;
                this.SelectFilesButton.IsEnabled = false;
            }
        }

        private void Wizard_Finish(object sender, RoutedEventArgs e)
        {
            project.Dictionary = wizardViewModel.SelectedItem;
            container.Project = project;

            mainWindow.PrepareProject();
        }

        private void Destination_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                project.DestinationPath = dialog.SelectedPath;
                wizardViewModel.DestinationDirectory = project.DestinationPath;
            }
        }

        private void CustomDictionary_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = @"C:\";
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                project.CustomDictionary = fileDialog.FileName;
                wizardViewModel.CustomDictionary = project.CustomDictionary;
            }
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = @"C:\";
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.Multiselect = true;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && fileDialog.FileNames.Length > 0)
            {
                project.SourceFiles = fileDialog.FileNames;
                wizardViewModel.SourceFiles = project.SourceFiles;
                this.SelectSourceFilesStackPanel.IsEnabled = false;
                this.PreserveSubfolderCheckBox.IsChecked = false;
                this.PreserveSubfolderCheckBox.IsEnabled = false;
            }
        }

        private void PreserveSubfolderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (null != wizardViewModel)
            {
                wizardViewModel.PreserveSubfolders = true;
                project.PreserveSubfolders = true;
            }
        }

        private void PreserveSubfolderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            wizardViewModel.PreserveSubfolders = false;
            project.PreserveSubfolders = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != wizardViewModel.SelectedItem)
            {
                this.Page1.CanSelectNextPage = true;
            }
        }



    }
}
