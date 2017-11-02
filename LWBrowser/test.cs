using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CefSharp.WinForms;

namespace LWBrowser
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();

            var cwb = new CefSharp.WinForms.ChromiumWebBrowser("http://lonamiwebs.github.io");
            Controls.Add(cwb);
        }
    }
}
