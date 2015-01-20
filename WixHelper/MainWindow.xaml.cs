using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WixHelper.Annotations;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace WixHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _folderPath = @"C:\git\VidyoIntegration\src\VidyoIntegration\Addin\VidyoAddin\bin\Release";
        private string _output;
        private string _pathPrefix = @"..\VidyoAddin\bin\Release\";


        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value; 
                OnPropertyChanged();
            }
        }

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value; 
                OnPropertyChanged();
            }
        }

        public string PathPrefix
        {
            get { return _pathPrefix; }
            set
            {
                _pathPrefix = value; 
                OnPropertyChanged();
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Process_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    MessageBox.Show("Folder does not exist: " + FolderPath);
                    return;
                }

                // Clear previous output
                Output = "";

                // Get list of files
                var files = Directory.GetFiles(FolderPath, "*.*", SearchOption.AllDirectories);

                // Iterate through files
                foreach (var file in files)
                {
                    // Files are full paths, so clear the part of the path we know about
                    var partialFilePath = file.Substring(FolderPath.Length).Trim(new[] {'\\'});

                    // Get just the actual file name
                    var fileName = partialFilePath.Contains("\\")
                        ? partialFilePath.Substring(partialFilePath.LastIndexOf("\\")).Trim(new[] { '\\' })
                        : partialFilePath;

                    // Write the File element to the output
                    Output +=
                        string.Format(
                            "<File Id=\"{0}\" Name=\"{1}\" Source=\"{2}\" Vital=\"yes\" KeyPath=\"no\" DiskId=\"1\"/>",
                            fileName.Replace('-', '_'), fileName,
                            Path.Combine(PathPrefix, partialFilePath)) + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var fbd = new FolderBrowserDialog();
                if (!string.IsNullOrEmpty(FolderPath))
                    fbd.SelectedPath = FolderPath;
                if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                FolderPath = fbd.SelectedPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
