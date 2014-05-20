using Microsoft.Win32;
using MPSpell.Extensions;
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

namespace MPSpellCorrector
{
    /// <summary>
    /// Interaction logic for DictionaryCreatorWindow.xaml
    /// </summary>
    public partial class DictionaryCreatorWindow : Window
    {

        private readonly char[] separator = new char[] { ';' };

        StreamReader reader;
        List<string> dictionary = new List<string>();        

        public DictionaryCreatorWindow()
        {
            InitializeComponent();
        }

        private void File_Button_Click(object sender, RoutedEventArgs e)
        {
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

                    this.SaveButton.IsEnabled = true;
                    this.SkipButton.IsEnabled = true;
                    this.AddButton.IsEnabled = true;
                    this.AddPreviousButton.IsEnabled = true;

                    this.ShowNext();
                    this.WordsCount.Text = this.dictionary.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void ShowNext()
        {
            if (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Contains(';'))
                {
                    string[] parts = line.Split(separator);

                    this.WrongWord.Text = parts[0];
                    this.Frequency.Text = parts[1];
                    this.Corrections.Text = parts[2].Replace(",", "  ");
                }
                else
                {
                    this.WrongWord.Text = line;
                }                
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.PrevWord.Text = this.WrongWord.Text;
            string word = this.WrongWord.Text;
            if (!String.IsNullOrEmpty(word))
            {
                this.dictionary.Add(word.ToLower());
            }

            this.WordsCount.Text = this.dictionary.Count.ToString();
            this.ShowNext();
        }

        private void AddPrev_Button_Click(object sender, RoutedEventArgs e)
        {
            string word = this.PrevWord.Text;
            if (!String.IsNullOrEmpty(word))
            {
                this.dictionary.Add(word.ToLower());
            }

            this.WordsCount.Text = this.dictionary.Count.ToString();
        }
        
        private void Skip_Button_Click(object sender, RoutedEventArgs e)
        {
            this.PrevWord.Text = this.WrongWord.Text;
            this.ShowNext();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "txt";

            if (dialog.ShowDialog() == true)
            {
                string file = dialog.FileName;

                this.SaveDictionary(file);
            }
        }


        private void SaveDictionary(string file)
        {
            FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (string word in dictionary)
                {
                    writer.WriteLine(word);
                }
            }

            MessageBox.Show("Dictionary saved", "Done", MessageBoxButton.OK, MessageBoxImage.Information);

            stream.Dispose();
        }


    }
}
