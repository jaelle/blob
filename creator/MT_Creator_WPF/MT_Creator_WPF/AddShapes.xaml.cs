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
    /// Interaction logic for AddShapes.xaml
    /// </summary>
    public partial class AddShapes : Window
    {
        enum SHAPESTYLE { NONE, RECTANGLE, ELLIPSE };
        public SolidColorBrush shapeColor;
        public Shape newShape;
        public int height;
        public int width;
        Window1 w_Cur;
        SHAPESTYLE curStyle;

        public AddShapes(Window1 w)
        {
            w_Cur = w;
            height = 100;
            width = 150;
            InitializeComponent();
            textBox1.Text = height.ToString();
            textBox2.Text = width.ToString();
            shapeColor = Brushes.Black;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            colorChooser cc = new colorChooser(w_Cur);
            cc.ash = this;
            cc.ShowDialog();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            bool[] gesturesAllowed = new bool[3];
            gesturesAllowed[0] = (bool)checkBox1.IsChecked;
            gesturesAllowed[1] = (bool)checkBox2.IsChecked;
            gesturesAllowed[2] = (bool)checkBox3.IsChecked;
            if (newShape != null)
            {
                canvas1.Children.Remove(newShape);
                w_Cur.DraggableShape = newShape;
                w_Cur.RefreshScene(null, 4, height, width, gesturesAllowed, null, (int)curStyle);
            }
            this.Close();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                height = Int32.Parse(textBox1.Text);
                if (newShape != null)
                {
                    canvas1.Children.Clear();
                    newShape.Fill = shapeColor;
                    newShape.Height = height;
                    newShape.Width = width;
                    canvas1.Children.Add(newShape);
                }
            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox2.Text.Length != 0)
            {
                width = Int32.Parse(textBox2.Text);
                if (newShape != null)
                {
                    canvas1.Children.Clear();
                    newShape.Fill = shapeColor;
                    newShape.Height = height;
                    newShape.Width = width;
                    canvas1.Children.Add(newShape);
                }
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "100";
            textBox2.Text = "150";
            shapeColor = Brushes.Black;
            canvas1.Children.Clear();
            newShape = new Ellipse();
            newShape.Fill = shapeColor;
            newShape.Height = height;
            newShape.Width = width;
            canvas1.Children.Add(newShape);
            curStyle = SHAPESTYLE.ELLIPSE;
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "100";
            textBox2.Text = "150";
            shapeColor = Brushes.Black;
            canvas1.Children.Clear();
            newShape = new Rectangle();
            canvas1.Children.Clear();
            newShape.Fill = shapeColor;
            newShape.Height = height;
            newShape.Width = width;
            canvas1.Children.Add(newShape);
            curStyle = SHAPESTYLE.RECTANGLE;
        }
    }
}
