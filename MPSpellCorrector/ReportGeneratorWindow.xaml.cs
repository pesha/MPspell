using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using MPSpell.Extensions;
using Microsoft.Win32;

namespace MPSpellCorrector
{
    /// <summary>
    /// Interaction logic for ReportGeneratorWindow.xaml
    /// </summary>
    public partial class ReportGeneratorWindow : Window
    {

        enum Step
        {
            NotMistake,
            MissingCorrection,
            CorrectCorrection,
            WrongCorrection,
            WrongDueDictionary,
            WrongDueLanguage,
        }

        private readonly char[] separator = new char[] { ';' };
        private readonly char[] contextSeparator = new char[] { ',' };
        StreamReader reader;

        private int notMistake;
        private int missingCorrection;
        private int correctCorrection;
        private int wrongCorrection;
        private int wrongDueDictionary;
        private int wrongDueLanguage;

        private Step lastStep;
        private string[] line;

        public ReportGeneratorWindow()
        {
            InitializeComponent();
        }

        private void File_Button_Click(object sender, RoutedEventArgs e)
        {
            notMistake = 0;
            missingCorrection = 0;
            correctCorrection = 0;
            wrongCorrection = 0;

            if (reader != null)
            {
                reader.Dispose();
            }
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = @"C:\dev\git\Pspell\MPSpellCorrector\bin\Debug";
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    reader = EncodingDetector.GetStreamWithEncoding(fileDialog.FileName);

                    this.ShowNext();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void ShowNext(bool stepBack = false)
        {
            if (!reader.EndOfStream)
            {
                this.MistakeTextBox.Background = Brushes.Transparent;
                this.WrongCorrectionButton.IsEnabled = true;
                this.CorrectCorrectionButton.IsEnabled = true;
                this.NotMistakeButton.IsEnabled = true;
                this.MissingCorrectionButton.IsEnabled = true;
                this.WrongDueDictionaryButton.IsEnabled = true;
                this.WrongDueLanguageButton.IsEnabled = true;

                string[] parts;
                if (stepBack)
                {
                    parts = this.line;
                }
                else
                {
                    parts = reader.ReadLine().Split(separator);
                    this.line = parts;
                }

                this.PreviousWord.Text = parts[0];

                if (String.IsNullOrEmpty(parts[1]))
                {
                    this.MistakeTextBox.Background = Brushes.Pink;
                    this.WrongCorrectionButton.IsEnabled = false;
                    this.CorrectCorrectionButton.IsEnabled = false;
                    this.WrongDueDictionaryButton.IsEnabled = false;
                    this.WrongDueLanguageButton.IsEnabled = false;
                }
                else
                {
                    this.NotMistakeButton.IsEnabled = false;
                    this.MissingCorrectionButton.IsEnabled = false;
                }

                this.MistakeTextBox.Text = parts[1];

                List<string> leftContext = parts[3].Split(contextSeparator).ToList();
                List<string> rightContext = parts[4].Split(contextSeparator).ToList();

                leftContext.RemoveAt(leftContext.Count - 1);
                rightContext.RemoveAt(0);

                string lc = string.Empty;
                foreach (string item in leftContext)
                {
                    lc += " " + item;
                }

                string rc = string.Empty;
                foreach (string item in rightContext)
                {
                    rc += " " + item;
                }

                this.LeftContextTextBox.Text = lc;
                this.RightContextTextBox.Text = rc;                
            }
        }

        private void NotMistake_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.NotMistake;
            notMistake += 1;
            this.NotMistakeTextBlock.Text = notMistake.ToString();

            this.ShowNext();
        }

        private void CorrectCorrection_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.CorrectCorrection;
            correctCorrection += 1;
            this.CorrectCorrectionTextBlock.Text = correctCorrection.ToString();

            this.ShowNext();
        }

        private void WrongCorrection_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.WrongCorrection;
            wrongCorrection += 1;
            this.WrongCorrectionTextBlock.Text = wrongCorrection.ToString();

            this.ShowNext();
        }

        private void MissingCorrection_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.MissingCorrection;
            missingCorrection += 1;
            this.MissingCorrectionTextBlock.Text = missingCorrection.ToString();

            this.ShowNext();
        }

        private void WrongDueDictionary_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.WrongDueDictionary;
            wrongDueDictionary += 1;
            this.WrongDueDictionaryTextBlock.Text = wrongDueDictionary.ToString();

            this.ShowNext();
        }

        private void WrongDueLanguage_Button_Click(object sender, RoutedEventArgs e)
        {
            lastStep = Step.WrongDueLanguage;
            wrongDueLanguage += 1;
            this.WrongDueLanguageTextBlock.Text = wrongDueLanguage.ToString();

            this.ShowNext();
        }

        private void StepBack_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (this.lastStep)
            {
                case Step.CorrectCorrection:
                    correctCorrection -= 1;
                    this.CorrectCorrectionTextBlock.Text = correctCorrection.ToString();
                    break;
                case Step.MissingCorrection:
                    missingCorrection -= 1;
                    this.MissingCorrectionTextBlock.Text = missingCorrection.ToString();
                    break;
                case Step.NotMistake:
                    notMistake -= 1;
                    this.NotMistakeTextBlock.Text = notMistake.ToString();
                    break;
                case Step.WrongCorrection:
                    wrongCorrection -= 1;
                    this.WrongCorrectionTextBlock.Text = wrongCorrection.ToString();
                    break;
                case Step.WrongDueDictionary:
                    wrongDueDictionary -= 1;
                    this.WrongDueDictionaryTextBlock.Text = wrongDueDictionary.ToString();
                    break;
                case Step.WrongDueLanguage:
                    wrongDueLanguage -= 1;
                    this.WrongDueLanguageTextBlock.Text = wrongDueLanguage.ToString();
                    break;

            }

            this.ShowNext(true);
        }


    }
}
