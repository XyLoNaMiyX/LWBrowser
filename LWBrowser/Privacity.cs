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
using System.Diagnostics;

namespace LWBrowser
{
    public partial class Privacity : Form
    {
        Process hP = new Process();
        
        public Privacity()
        {
            InitializeComponent();
        }

        private void Privacity_Load(object sender, EventArgs e)
        {
            hP.StartInfo.FileName = "rundll32.exe";
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            bool ca = checkBox6.Checked;
            checkBox1.Checked = ca;
            checkBox2.Checked = ca;
            checkBox3.Checked = ca;
            checkBox4.Checked = ca;
            checkBox5.Checked = ca;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int toRemove = 0;
            if (checkBox1.Checked == true)
            {
                toRemove = toRemove + 1;
            }
            if (checkBox2.Checked == true)
            {
                toRemove = toRemove + 2;
            }
            if (checkBox3.Checked == true)
            {
                toRemove = toRemove + 8;
            }
            if (checkBox4.Checked == true)
            {
                toRemove = toRemove + 16;
            }
            if (checkBox5.Checked == true)
            {
                toRemove = toRemove + 32;
            }

            if (toRemove != 0)
            {
                hP.StartInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess " + toRemove;
                Process.Start(hP.StartInfo);
            }

            /*¿Por qué estos números?
             *
             * InetCpl.cpl,ClearMyTracksByProcess <número>
             * 1    = Historial.
             * 2    = Cookies.
             * 8    = Archivos temporales de internet.
             * 16   = Data de la form.
             * 32   = Contraseñas.
             * 255  = Eliminar todo.
             * 4351 = Eliminar todo, incluso archivos y ajustes de extensiones.
             */

            this.Close();
        }
    }
}
