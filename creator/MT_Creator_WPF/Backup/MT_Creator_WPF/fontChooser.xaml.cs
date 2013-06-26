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
    /// Interaction logic for fontChooser.xaml
    /// </summary>
    public partial class fontChooser : Window
    {
        public addText t_Cur;
        public updateText t_Up;
        public bool updating;

        public fontChooser(addText tAdd, bool isUpdate)
        {
            InitializeComponent();
            fontCombo.ItemsSource = Fonts.SystemFontFamilies;
            updating = isUpdate;

            t_Cur = tAdd;
            t_Cur.textBox1.FontFamily = new FontFamily();
            t_Cur.textBox1.FontSize = 12;
        }

        public fontChooser(updateText tUp, bool isUpdate)
        {
            InitializeComponent();
            fontCombo.ItemsSource = Fonts.SystemFontFamilies;
            updating = isUpdate;
            t_Up = tUp;

            t_Up.textBox1.FontFamily = new FontFamily();
            t_Up.textBox1.FontFamily = t_Up.w_Cur.ff;
            t_Up.textBox1.FontSize = t_Up.w_Cur.fontSize;
            t_Up.textBox1.Foreground = t_Up.w_Cur.fontColor;
        }

        private void fontCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBlock1.FontFamily = (FontFamily)fontCombo.SelectedItem;
            if (updating == false)
            {
                t_Cur.w_Cur.ff = textBlock1.FontFamily;
            }
            else
            {
                t_Up.w_Cur.ff = textBlock1.FontFamily;
            }
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlock1.FontSize = (int)((slider1.Value+1) * 10);
            this.label4.Content = textBlock1.FontSize;
            if (updating == false)
            {
                t_Cur.w_Cur.fontSize = (int)textBlock1.FontSize;
            }
            else
            {
                t_Up.w_Cur.fontSize = (int)textBlock1.FontSize;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (updating == false)
            {
                t_Cur.textBox1.FontFamily = textBlock1.FontFamily;
                t_Cur.textBox1.FontSize = textBlock1.FontSize;
                t_Cur.textBox1.Foreground = textBlock1.Foreground;
            }
            else
            {
                t_Up.textBox1.FontFamily = textBlock1.FontFamily;
                t_Up.textBox1.FontSize = textBlock1.FontSize;
                t_Up.textBox1.Foreground = textBlock1.Foreground;
            }
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (updating == false)
            {
                colorChooser cc = new colorChooser(t_Cur.w_Cur);
                cc.fc = this;
                cc.ShowDialog();
            }
            else
            {
                colorChooser cc = new colorChooser(t_Up.w_Cur);
                cc.fc = this;
                cc.ShowDialog();
            }
        }
    }
}
