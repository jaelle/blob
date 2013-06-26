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
    /// Interaction logic for addImage.xaml
    /// </summary>
    public partial class addImage : Window
    {
        private Window1 w_Cur;

        public addImage(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "JPG Images(.jpg)|*.jpg|PNG Images(.png|*.png|JPEG Images(.jpeg)|*.jpeg|GIF Images(.gif)|*.gif|BMP Images(.bmp)|*.bmp"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                ImageFile.Text = filename;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string linksTo;
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked; 
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;
            if (textBox1.Text == "Blank if no link...")
            {
                linksTo = null;
            }
            else
            {
                linksTo = textBox1.Text;
            }

            w_Cur.RefreshScene(ImageFile.Text, 1, -1, -1, gesturesAllowed, linksTo, 0);
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XAML file(.xaml)|*.xaml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                textBox1.Text = filename;
            }
        }
        void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
