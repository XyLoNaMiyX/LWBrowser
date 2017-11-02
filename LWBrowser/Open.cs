//Lonami (XyLoNaMiyX)
//LW Browser 2013
//Email: totufals@hotmail.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LWBrowser
{
    public partial class Open : Form
    {
        WebBrowser wb;

        public Open(WebBrowser wb)
        {
            this.wb = wb;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wb.Navigate(textBox1.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Archivos de texto(*.txt)|*.txt|Documentos HTML(*.html)|*.html|Todos los archivos|*.*";
            if (o.ShowDialog() == DialogResult.OK)
                textBox1.Text = o.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                wb.Navigate(textBox1.Text);
                this.Close();
            }

        }

    }
}
