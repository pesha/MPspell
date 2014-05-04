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
                project.FolderPath = dialog.SelectedPath;
                container.Settings.DataFolder = dialog.SelectedPath;
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
            }
        }

    }
}
