﻿using System;
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
    /// Interaction logic for updateShapexaml.xaml
    /// </summary>
    public partial class updateShapexaml : Window
    {
        Window1 w_Cur;
        public SolidColorBrush shapeColor;
        public updateShapexaml(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
            if (w_Cur.selectedElement.GetType() == typeof(Ellipse))
            {
                shapeColor = (SolidColorBrush)w_Cur.DraggableEllipse.Fill;
            }
            if (w_Cur.selectedElement.GetType() == typeof(Rectangle))
            {
                shapeColor = (SolidColorBrush)w_Cur.DraggableRectangle.Fill;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XAML file(.xaml)|*.xaml";

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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            colorChooser cc = new colorChooser(w_Cur);
            cc.selectedColor = shapeColor;
            cc.usx = this;
            cc.ShowDialog();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string linksTo;
            int height = Int32.Parse(HeightT.Text);
            int width = Int32.Parse(WidthT.Text);
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked;
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;
            if (!File.Exists(textBox1.Text))
            {
                linksTo = null;
            }
            else
            {
                linksTo = textBox1.Text;
            }

            if (HeightT.Text.Length > 0)
            {
                height = Int32.Parse(HeightT.Text);
                if (height < 0)
                {
                    height *= -1;
                }
            }

            if (WidthT.Text.Length > 0)
            {
                width = Int32.Parse(WidthT.Text);
                if (width < 0)
                {
                    width *= -1;
                }
            }
            w_Cur.UpdateScene(null, w_Cur.curType, height, width, gesturesAllowed, linksTo);
            this.Close();
        }
    }
}
