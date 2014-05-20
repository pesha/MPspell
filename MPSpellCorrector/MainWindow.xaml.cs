using MPSpell.Correction;
using MPSpell.Tools;
using MPSpellCorrector.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        private BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            Container = new MPSpellCorrector.Class.Container();
        }

        private void New_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Container.Project = null;
            CorrectionWizard wizard = new CorrectionWizard(this);
            wizard.ShowDialog();
        }

        public void PrepareProject()
        {
            this.ReportGrid.Visibility = System.Windows.Visibility.Hidden;

            Project project = Container.Project;
            if (!String.IsNullOrEmpty(project.CustomDictionary) && File.Exists(project.CustomDictionary))
            {
                List<string> extensionDict = SimpleFileLoader.Load(project.CustomDictionary);
                if (extensionDict.Count > 0)
                {
                    project.Dictionary.AddRange(extensionDict);
                }
            }

            project.ReportPath = Container.Settings.ReportFolder;
            
            if (null != project.SourceFiles)
            {
                corrector = new FolderCorrector(project.Dictionary, project.SourceFiles, project.DestinationPath, Container.Settings.ReportFolder);
            }
            else
            {
                corrector = new FolderCorrector(project.Dictionary, project.SourcePath, project.DestinationPath, Container.Settings.ReportFolder, project.PreserveSubfolders);
            }

            corrector.ExportContext = Container.Settings.ExportContext;

            // todo udelat viewmodel
            FolderAnalyzeResult res = corrector.GetFolderAnalyzeResult();
            this.FileCount.Text = res.FileCount.ToString();
            this.FileSize.Text = res.GetSizeInMb();
            this.AvgFileSize.Text = res.GetAvgSize();
            this.ThreadCount.Text = corrector.ThreadsUsed.ToString() + " (limit " + corrector.ThreadsAvailable + ")";
            this.ProgressStatus.Text = this.ReportToString(Report.WaitingToStart);

            this.RunButton.IsEnabled = true;
        }
        
        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {                        
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;            

            this.RunButton.IsEnabled = false;
            this.StopButton.IsEnabled = true;

            worker.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            this.ProgressBar.Value = e.ProgressPercentage;

            ProgressReport report = (ProgressReport)e.UserState;
            if (null != report)
            {
                this.ProgressStatus.Text = this.ReportToString(report.Report);

                if (report.Report == Report.PreparingStatistics)
                {
                    this.ResultDataButton.IsEnabled = true;
                }
            }
        }

        private string ReportToString(Report report)
        {
            switch (report)
            {
                case Report.Canceled: return "Process canceled";
                case Report.Done: return "Done";
                case Report.PreloadingDictionary: return "Loading dictionary";
                case Report.PreparingStatistics: return "Calculating statistics";
                case Report.WaitingToStart: return "Waiting";
                case Report.Working: return "Working"; 
            }

            return "";
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            long time = corrector.CorrectionTime;
            this.RunningTime.Text = Math.Round((double)time / 1000, 1).ToString() + " sec";
            this.DetectedTextBlock.Text = corrector.Detected.ToString();
            this.CorrectedTextBlock.Text = corrector.Corrected.ToString();

            this.ReportGrid.Visibility = System.Windows.Visibility.Visible;
            this.ResultDataButton.IsEnabled = true;
            this.StatisticsButton.IsEnabled = true;
            this.StopButton.IsEnabled = false;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            corrector.RunCorrection((BackgroundWorker) sender);            
        }

        private void Settings_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow(this.Container);
            window.Show();
        }

        private void Report_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReportGeneratorWindow window = new ReportGeneratorWindow();
            window.Show();
        }

        private void DictionaryGen_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DictionaryCreatorWindow window = new DictionaryCreatorWindow();
            window.Show();
        }

        private void Close_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();

            this.RunButton.IsEnabled = true;
            this.StopButton.IsEnabled = false;
        }

        private void ResultDataButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + corrector.ResultDirectory);
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + corrector.ReportDirectory);
        }

    }
}
