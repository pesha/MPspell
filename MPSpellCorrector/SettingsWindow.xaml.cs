using MPSpellCorrector.Class;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        private Container container;
        private SettingsViewModel viewModel;

        public SettingsWindow(Container container)
        {
            InitializeComponent();
            this.container = container;

            viewModel = new SettingsViewModel(container.Settings);
            this.DataContext = viewModel;
        }

        private void ReportChange_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                viewModel.ReportsPath = dialog.SelectedPath;
            }
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
