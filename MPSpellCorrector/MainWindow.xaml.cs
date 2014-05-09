using MPSpell.Correction;
using MPSpellCorrector.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        public MPSpellCorrector.Class.Container Container { get; private set; }
        private FolderCorrector corrector;

        public MainWindow()
        {
            InitializeComponent();
            Container = new MPSpellCorrector.Class.Container();
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

            this.RunButton.IsEnabled = true;
        }
        
        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {            
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;

            this.RunButton.IsEnabled = false;

            worker.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            long time = corrector.CorrectionTime;
            this.RunningTime.Text = Math.Round((double)time / 1000, 1).ToString() + "sec";
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            corrector.RunCorrection((BackgroundWorker) sender);            
        }

        private void Settings_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
        }

        private void Report_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReportGeneratorWindow window = new ReportGeneratorWindow();
            window.Show();
        }

    }
}
