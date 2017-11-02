//Lonami (XyLoNaMiyX)
//LW Browser 2013
//Email: totufals@hotmail.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LWBrowser
{
    partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        //Versión de ensamblado
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        //Copyright de ensamblado
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        //Compañia de ensamblado
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        //Al cargar la form
        private void About_Load(object sender, EventArgs e)
        {
            this.labelVersion.Text = String.Format("Versión {0} {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
        }

        //Botón aceptar
        void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
