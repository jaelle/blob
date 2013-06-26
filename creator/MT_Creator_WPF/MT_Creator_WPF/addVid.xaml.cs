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
    /// Interaction logic for addVid.xaml
    /// </summary>
    public partial class addVid : Window
    {
        private Window1 w_Cur;
        public addVid(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Windows Media Video(.wmv)|*.wmv|AVI (.avi)|*.avi|Movie(.mov)|*.mov"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                VidFile.Text = filename;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked;
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;

            w_Cur.RefreshScene(VidFile.Text, 2, -1, -1, gesturesAllowed, null, 0);
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //do nothing.
        }

    }
}
