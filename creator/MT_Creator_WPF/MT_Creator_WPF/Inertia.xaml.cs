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
    /// Interaction logic for Inertia.xaml
    /// </summary>
    public partial class Inertia : Window
    {
        Window1 w_Cur;
        public Inertia(Window1 w)
        {
            InitializeComponent();

            w_Cur = w;
            this.textBox1.Text = w_Cur.initTranslateDeceleration.ToString();
            this.textBox2.Text = w_Cur.initRotateDeceleration.ToString();
            this.textBox3.Text = w_Cur.initExpandDeceleration.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            w_Cur.initTranslateDeceleration = Double.Parse(this.textBox1.Text);
            w_Cur.initRotateDeceleration = Double.Parse(this.textBox2.Text);
            w_Cur.initExpandDeceleration = Double.Parse(this.textBox3.Text);
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Presets ps = new Presets();
            ps.ShowDialog();
        }
    }
}
