using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MT_Creator_WPF
{
    /// <summary>
    /// Interaction logic for addSound.xaml
    /// </summary>
    public partial class addSound : Window
    {
        Window1 w_Cur;

        public addSound(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Audio files (.mp3)|*.mp3"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                SoundFile.Text = filename;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            bool []gesturesAllowed = new bool[3];
            w_Cur.RefreshScene(SoundFile.Text, 3, 0, 0, gesturesAllowed, null);
            this.Close();
        }
    }
}
