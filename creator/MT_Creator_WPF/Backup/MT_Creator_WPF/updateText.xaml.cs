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
using System.IO;

namespace MT_Creator_WPF
{
    /// <summary>
    /// Interaction logic for updateText.xaml
    /// </summary>
    public partial class updateText : Window
    {
        public Window1 w_Cur;
        public updateText(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
            textBox1.FontFamily = w_Cur.ff;
            textBox1.FontSize = w_Cur.fontSize;
            textBox1.Foreground = w_Cur.fontColor;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            fontChooser fc = new fontChooser(this, true);
            fc.textBlock1.FontSize = w_Cur.fontSize;
            fc.textBlock1.FontFamily = w_Cur.FontFamily;
            fc.textBlock1.Foreground = w_Cur.fontColor;
            Console.WriteLine(w_Cur.FontFamily.ToString());
            fc.fontCombo.SelectionMode = SelectionMode.Single;
            //fc.fontCombo.SetValue(w_Cur.FontFamily.ToString());
            fc.ShowDialog();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XAML file(.xaml)|*.xaml";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                textBox2.Text = filename;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string linksTo;
            int height = Int32.Parse(HeightT.Text);
            int width = Int32.Parse(WidthT.Text);
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked;
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;
            if (!File.Exists(textBox2.Text))
            {
                linksTo = null;
            }
            else
            {
                linksTo = textBox2.Text;
            }

            if (HeightT.Text.Length > 0)
            {
                height = Int32.Parse(HeightT.Text);
                if (height < 0)
                {
                    height *= -1;
                }
            }
            /*if ((bool)checkBox4.IsChecked)
            {
                height = -1;
            }*/

            if (WidthT.Text.Length > 0)
            {
                width = Int32.Parse(WidthT.Text);
                if (width < 0)
                {
                    width *= -1;
                }
            }
            w_Cur.UpdateScene(textBox1.Text, 0, height, width, gesturesAllowed, linksTo);
            w_Cur.DraggableText.FontFamily = textBox1.FontFamily;
            w_Cur.DraggableText.Foreground = textBox1.Foreground;
            w_Cur.DraggableText.FontSize = textBox1.FontSize;
            this.Close();
        }
    }
}
