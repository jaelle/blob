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
    /// Interaction logic for addText.xaml
    /// </summary>
    public partial class addText : Window
    {
        public Window1 w_Cur;
        public addText(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string linksTo;
            int height = 200;
            int width = 300;
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked;
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;
            if (textBox2.Text == "Blank if no link...")
            {
                linksTo = null;
            }
            else
            {
                linksTo = textBox2.Text;
            }

            w_Cur.RefreshScene(textBox1.Text, 0, height, width, gesturesAllowed, linksTo);
            w_Cur.DraggableText.FontFamily = textBox1.FontFamily;
            w_Cur.DraggableText.FontSize = textBox1.FontSize;
            w_Cur.DraggableText.Foreground = textBox1.Foreground;
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            fontChooser fc = new fontChooser(this, false);
            fc.ShowDialog();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
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
                textBox2.Text = filename;
            }
        }
    }
}
