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
using System.Reflection;

namespace MT_Creator_WPF
{
    /// <summary>
    /// Interaction logic for colorChooser.xaml
    /// </summary>
    public partial class colorChooser : Window
    {
        public Window1 w_Cur;
        public SolidColorBrush selectedColor;
        public fontChooser fc;
        public bool bg;
        
        public colorChooser(Window1 w)
        {
            w_Cur = w;
            InitializeComponent();
            fc = null;
            bg = false;
            setUpColors();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void setUpColors()
        {
            selectedColor = null;
            Type ColorsType = typeof(Brushes);
            PropertyInfo[] ColorsProps = ColorsType.GetProperties();

            foreach(PropertyInfo p in ColorsProps)
            {
                Button b = new Button();
                b.Name = p.Name;
                b.Background = (SolidColorBrush)p.GetValue(null, null);
                b.Click += new RoutedEventHandler(b_Click);
                this.uniformGrid1.Children.Add(b);
            }
        }
        private void b_Click(object sender, RoutedEventArgs e)
        {
            Button b = new Button();
            b = (Button)sender;
            this.rectangle1.Fill = b.Background;
            this.textBlock1.Text = b.Name;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            selectedColor = (SolidColorBrush)rectangle1.Fill;
            if (fc != null)
            {
                fc.textBlock1.Foreground = selectedColor;
                w_Cur.fontColor = selectedColor;
            }
            if (bg == true)
            {
                w_Cur.Background = selectedColor;
                w_Cur.bgColor = selectedColor;
            }

            this.Close();
        }
    }
}
