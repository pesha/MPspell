using MPSpell.Correction;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MPSpellCorrector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Container Container { get; private set; }
        private FolderCorrector corrector;

        public MainWindow()
        {
            InitializeComponent();
            Container = new Container();
        }

        private void New_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CorrectionWizard wizard = new CorrectionWizard(this);
            wizard.ShowDialog();
        }

        public void PrepareProject()
        {
            Project project = Container.Project;
            corrector = new FolderCorrector(project.Dictionary, project.FolderPath, project.DestinationPath);

            // todo udelat viewmodel
            FolderAnalyzeResult res = corrector.GetFolderAnalyzeResult();
            this.FileCount.Text = res.FileCount.ToString();
            this.FileSize.Text = res.GetSizeInMb().ToString();
        }
        
        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            long time = corrector.RunCorrection();
            this.RunningTime.Text = Math.Round((double)time / 1000, 1).ToString() + "sec";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
        }

    }
}
