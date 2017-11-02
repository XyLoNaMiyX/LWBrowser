//Lonami (XyLoNaMiyX)
//LW Browser 2013
//Email: totufals@hotmail.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;

namespace LWBrowser
{
    public partial class LWBrowser : Form
    {
        List<String> urls = new List<String>();
        String homePage = "about:blank";
        CultureInfo currentCulture;

        //.PointToClient(screenCord);

        public LWBrowser()
        {
            InitializeComponent();
            currentCulture = CultureInfo.CurrentCulture;
        }

        #region Form Load

        private void LWBrowser_Load(object sender, EventArgs e)
        {
            new test().Show();
            return;

            addNewTab();
            adrBarTextBox.TextBox.Select();

            RegistryKey srchEngineKey = Registry.CurrentUser.CreateSubKey("Software\\LonamiWebs\\Browser\\searchEngine");
            
            if (srchEngineKey.GetValue(null) == null)
                srchEngineKey.SetValue(null, "google");

            switch (srchEngineKey.GetValue(null).ToString())
            {
                case "google":
                    googleSearch.Checked = true;
                    liveSearch.Checked = false;
                    yahooSearch.Checked = false;
                    break;
                case "live":
                    googleSearch.Checked = false;
                    liveSearch.Checked = true;
                    yahooSearch.Checked = false;
                    break;
                case "yahoo":
                    googleSearch.Checked = false;
                    liveSearch.Checked = false;
                    yahooSearch.Checked = true;
                    break;
            }
            srchEngineKey.Close();
        }

        #endregion

        #region TABURI

        private void addNewTab()
        {
            TabPage tpage = new TabPage();
            tpage.BorderStyle = BorderStyle.Fixed3D;
            browserTc.TabPages.Insert(browserTc.TabCount - 1, tpage);
            WebBrowser browser = new WebBrowser();
            browser.Navigate(homePage);
            tpage.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browserTc.SelectTab(tpage);
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Form1_DocumentCompleted);
            browser.NewWindow += new CancelEventHandler(browser_NewWindow);
            WebBrowser currentBrowser = getCurrentBrowser();
            img.Image = null;
        }

        private void Form1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser currentBrowser = getCurrentBrowser();
            currentBrowser.ScriptErrorsSuppressed = true;
            String text = "Página en blanco";

            //currentBrowser.Document.MouseUp += new HtmlElementEventHandler(Document_MouseUp);

            currentBrowser.ContextMenuStrip = webBrowserCMS;
            currentBrowser.IsWebBrowserContextMenuEnabled = false;

            if (!currentBrowser.Url.ToString().Equals("about:blank"))
            {
                text = getCurrentBrowser().Document.GetElementsByTagName("title")[0].InnerText;
            }

            this.adrBarTextBox.Text = currentBrowser.Url.ToString();
            browserTc.SelectedTab.Text = text;

            img.Image = favicon(currentBrowser.Url.ToString(), "net.png");

            if (!urls.Contains(currentBrowser.Url.ToString()))
                urls.Add(currentBrowser.Url.Host.ToString());
        }

        private void browser_NewWindow(object sender, CancelEventArgs e)
        {
            addNewTab();
            getCurrentBrowser().Navigate(getCurrentBrowser().StatusText);
            e.Cancel = true;
        }     

        #region WEBBROWSERContextMenuStrip

        static HtmlElement lastClickedElem;

        private void clickedElement()
        {
            WebBrowser currentBrowser = getCurrentBrowser();

            //Context menu, cogemos coordenadas y las pasamos al navegador
            Point screenCord = new Point(MousePosition.X, MousePosition.Y);
            Point browserCord = currentBrowser.PointToClient(screenCord);

            //Así al hacer click en uno, lo cogemos de las coordenadas especificadas en la variable
            HtmlElement clickedElem = currentBrowser.Document.GetElementFromPoint(browserCord);

            lastClickedElem = clickedElem;
        }

        private void webBrowserCMS_Opening(object sender, CancelEventArgs e)
        {
            clickedElement();

            WebBrowser currentBrowser = getCurrentBrowser();

            //Context menu, cogemos coordenadas y las pasamos al navegador
            Point screenCord = new Point(MousePosition.X, MousePosition.Y);
            Point browserCord = currentBrowser.PointToClient(screenCord);

            //Así al hacer click en uno, lo cogemos de las coordenadas especificadas en la variable
            HtmlElement clickedElem = currentBrowser.Document.GetElementFromPoint(browserCord);

            //Eliminamos el ContextMenu de IE (todas hasta que no haya)
            for (int i = 0; i < webBrowserCMS.Items.Count; i++)
            {
                webBrowserCMS.Items[i].Visible = false;
            }

            //Después, comprobamos cada etiqueta y activamos según sea
            switch (clickedElem.TagName)
            {
                case "A":
                    TsmiOpen.Visible = true;
                    TsmiOpenInNewTab.Visible = true;
                    TsmiCopyLink.Visible = true;
                    break;
                case "IMG":
                    TsmiSaveImage.Visible = true;
                    TsmiCopyImage.Visible = true;
                    TsmiCopyImgUrl.Visible = true;
                    TsmiOpenImgInNewTab.Visible = true;
                    break;
                default:
                    TsmiRefresh.Visible = true;
                    TsmiViewSource.Visible = true;
                    break;
            }
        }

        private void TsmiOpen_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(lastClickedElem.GetAttribute("href"));
        }

        private void TsmiOpenInNewTab_Click(object sender, EventArgs e)
        {
            addNewTab();
            getCurrentBrowser().Navigate(lastClickedElem.GetAttribute("href"));
        }

        private void TsmiCopyLink_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lastClickedElem.GetAttribute("href"));
        }

        //¡Trabajando con Streams! Es realemnte fácil.
        //Esto consigue una imagen de un Stream (vista genérica de una secuencia de bits).
        public static Image getImgFromUrl(string url)
        {
            HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(url);

            using (HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse())
            {
                using (Stream currentStream = webRes.GetResponseStream())
                {
                    return Image.FromStream(currentStream);
                }
            }
        }

        private void TsmiCopyImage_Click(object sender, EventArgs e)
        {
            string imgUrl = lastClickedElem.GetAttribute("src");
            Clipboard.SetImage(getImgFromUrl(imgUrl));
        }

        private void TsmiSaveImage_Click(object sender, EventArgs e)
        {
            string imgSrc = lastClickedElem.GetAttribute("src");
            Image imgToSave = getImgFromUrl(imgSrc);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Gráfico portable para la red PNG|*.png|Grupo de expertos fotográficos unidos JPEG|*.jpg|Formato de intercambio gráfico GIF|*.gif|Mapa de bits BMP|*.bpm|Formato de Archivo de Imagen Etiquetada TIFF|*.tif";
            saveDialog.Title = "Guardar imagen como...";
            saveDialog.ShowDialog();

            if (saveDialog.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveDialog.OpenFile();
                switch (saveDialog.FilterIndex)
                {
                    case 1:
                        imgToSave.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        imgToSave.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 3:
                        imgToSave.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        imgToSave.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 5:
                        imgToSave.Save(fs, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }
            }
        }

        private void TsmiCopyImgUrl_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lastClickedElem.GetAttribute("src"));
        }

        private void TsmiOpenImgInNewTab_Click(object sender, EventArgs e)
        {
            addNewTab();
            getCurrentBrowser().Navigate(lastClickedElem.GetAttribute("src"));
        }

        private void TsmiRefresh_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }

        private void viewSource()
        {
            String source = ("source.txt");
            StreamWriter writer = File.CreateText(source);
            writer.Write(getCurrentBrowser().DocumentText);
            writer.Close();
            Process.Start("notepad.exe", source);
        }

        private void TsmiViewSource_Click(object sender, EventArgs e)
        {
            viewSource();
        }

        #endregion

        private void closeTab()
        {
            if (browserTc.TabCount != 2)
            {
                browserTc.TabPages.RemoveAt(browserTc.SelectedIndex);
            }
            else
            {
                DialogResult exitQuery = MessageBox.Show("¿Desea cerrar el navegador?", "Cerrar navegador", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (exitQuery.Equals(DialogResult.OK))
                    this.Close();
            }
        }

        static int lastClickedTab;

        private void browserTc_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < closeTabContext.Items.Count; i++)
                {
                    closeTabContext.Items[i].Visible = false;
                }

                closeTabCTsmi.Visible = true;
                duplicateTabCTsmi.Visible = true;

                for (int i = 0; i < browserTc.TabCount; i++)
                {
                    Rectangle r = browserTc.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        lastClickedTab = i;
                        closeSelTabTsmi.Visible = true;
                    }
                }
            }
        }
        //coolami
        private void closeSelTabTsmi_Click(object sender, EventArgs e)
        {
            if (lastClickedTab != browserTc.TabPages.Count - 1)
                browserTc.TabPages.RemoveAt(lastClickedTab);
        }

        private void browserTc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (browserTc.SelectedIndex == browserTc.TabPages.Count - 1) addNewTab();
            else
            {
                //Código extra para cambiar el favicon... Y para cambiar el título de la pestaña.
                WebBrowser currentBrowser = getCurrentBrowser();
                string text;
                img.Image = null;

                if (getCurrentBrowser().Url != null)
                {
                    adrBarTextBox.Text = getCurrentBrowser().Url.ToString();
                    img.Image = favicon(currentBrowser.Url.ToString(), "net.png");

                    text = getCurrentBrowser().Document.GetElementsByTagName("title")[0].InnerText;
                    browserTc.SelectedTab.Text = text;
                }
                else
                {
                    adrBarTextBox.Text = "about:blank";
                    browserTc.SelectedTab.Text = "Página en blanco";
                }
            }
        }

        private void closeTabCTsmi_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void duplicateTabCTsmi_Click(object sender, EventArgs e)
        {
            if (getCurrentBrowser().Url != null)
            {
                Uri dup_url = getCurrentBrowser().Url;
                addNewTab();
                getCurrentBrowser().Url = dup_url;
            }
            else addNewTab();
        }

        #endregion

        #region FAVICON

        public static Image favicon(String u, string file)
        {
            Uri url = new Uri(u);

            String iconurl = "http://" + url.Host + "/favicon.ico";

            if (iconurl != "http:///favicon.ico")
            {
                WebRequest request = WebRequest.Create(iconurl);
                try
                {
                    WebResponse response = request.GetResponse();

                    Stream s = response.GetResponseStream();
                    return Image.FromStream(s);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return null;
                }
            }

            else
                return null;
        }
        #endregion
        /*
         //LANONEEDED LWBrowser.Designer.cs
        #region TOOL STRIP MENU

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(adress);
        }

        private void openInNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewTab();
            getCurrentBrowser().Navigate(adress);
        }

        private void openInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LWBrowser new_form = new LWBrowser();
            new_form.Show();
            new_form.getCurrentBrowser().Navigate(adress);
        }
        
        #endregion
        */
        #region WEB

        private void goto_click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(sender.ToString());
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoBack();
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoForward();
        }

        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(homePage);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }

        private void verCódigoFuenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewSource();
        }

        private void fullScreenTsmi_Click(object sender, EventArgs e)
        {
            if (!(this.FormBorderStyle == FormBorderStyle.None && this.WindowState == FormWindowState.Maximized))
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.TopMost = true;
                menuBar.Visible = false;
                adrBar.Visible = false;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
                menuBar.Visible = true;
                adrBar.Visible = true;
            }
        }

        #endregion

        #region FILE

        private void newTabTsmi_Click(object sender, EventArgs e)
        {
            addNewTab();
        }

        private void duplicateTabTsmi_Click(object sender, EventArgs e)
        {
            if (getCurrentBrowser().Url != null)
            {
                Uri dup_url = getCurrentBrowser().Url;
                addNewTab();
                getCurrentBrowser().Url = dup_url;
            }
            else addNewTab();
        }

        private void closeTabTsmi_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void newWindowTsmi_Click(object sender, EventArgs e)
        {
            (new LWBrowser()).Show();
        }

        private void openTsmi_Click(object sender, EventArgs e)
        {
            (new Open(getCurrentBrowser())).Show();
        }

        private void saveAsTsmi_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowSaveAsDialog();
        }

        private void pageSetupTsmi_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPageSetupDialog();
        }

        private void printTsmi_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintDialog();
        }

        private void printPreviewTsmi_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintPreviewDialog();
        }

        private void pageByEmailTsmi_Click(object sender, EventArgs e)
        {
            Process.Start("msimn.exe");
        }

        private void linkByEmailTsmi_Click(object sender, EventArgs e)
        {
            Process.Start("msimn.exe");
        }

        private void propertiesTsmi_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPropertiesDialog();
        }

        #endregion

        #region EDIT

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Cut", false, null);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Copy", false, null);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Paste", false, null);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("SelectAll", true, null);
        }

        #endregion

        #region TOOLBAR

        private void calculatorTmsi_Click(object sender, EventArgs e)
        {
            Process.Start("calc.exe");
        }

        private void calendarTsmi_Click(object sender, EventArgs e)
        {
            (new Calendar()).Show();
        }

        #endregion

        #region HELP

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate("http://lonamiwebs.github.io/contacto.htm");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new About()).Show();
        }

        #endregion

        #region ADRESS BAR

        private WebBrowser getCurrentBrowser()
        {
            return (WebBrowser)browserTc.SelectedTab.Controls[0];
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoForward();
        }

        private void doSearch(string goTo)
        {
            if (googleSearch.Checked == true)
                getCurrentBrowser().Navigate("http://google.com/search?q=" + goTo);
            else
                if (liveSearch.Checked == true)
                    getCurrentBrowser().Navigate("http://search.live.com/results.aspx?q=" + goTo);
                else
                    getCurrentBrowser().Navigate("http://es.search.yahoo.com/search?p=" + goTo);
        }

        private bool checkUrl(string urlToCheck)
        {
            if (urlToCheck.Contains("http://"))
                return true;
            else if (urlToCheck.Contains("https://"))
                return true;
            else if (urlToCheck.Contains("www."))
                return true;
            else if (urlToCheck.Contains(".com"))
                return true;
            else if (urlToCheck.Contains(".net"))
                return true;
            else if (urlToCheck.Contains(" "))
                return false;
            else
                return false;
        }

        private void adrBarTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string goTo = adrBarTextBox.Text;
                if (checkUrl(goTo) == true)
                    getCurrentBrowser().Navigate(goTo);
                else
                    doSearch(goTo);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string goTo = adrBarTextBox.Text;
            if (checkUrl(goTo) == true)
                getCurrentBrowser().Navigate(goTo);
            else
                doSearch(goTo);
        }

        private void adrBarTextBox_Click(object sender, EventArgs e)
        {
            adrBarTextBox.SelectAll();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }

        private void googleSearch_Click(object sender, EventArgs e)
        {
            RegistryKey srchEngineKey = Registry.CurrentUser.CreateSubKey("Software\\LonamiWebs\\Browser\\searchEngine");
            srchEngineKey.SetValue(null, "google");
            srchEngineKey.Close();
            googleSearch.Checked = true;
            liveSearch.Checked = false;
            yahooSearch.Checked = false;
        }

        private void liveSearch_Click(object sender, EventArgs e)
        {
            RegistryKey srchEngineKey = Registry.CurrentUser.CreateSubKey("Software\\LonamiWebs\\Browser\\searchEngine");
            srchEngineKey.SetValue(null, "live");
            srchEngineKey.Close();
            googleSearch.Checked = false;
            liveSearch.Checked = true;
            yahooSearch.Checked = false;
        }

        private void yahooSearch_Click(object sender, EventArgs e)
        {
            RegistryKey srchEngineKey = Registry.CurrentUser.CreateSubKey("Software\\LonamiWebs\\Browser\\searchEngine");
            srchEngineKey.SetValue(null, "yahoo");
            srchEngineKey.Close();
            googleSearch.Checked = false;
            liveSearch.Checked = false;
            yahooSearch.Checked = true;
        }

        #endregion

        #region LINKS BAR

        private void items_Click(object sender, EventArgs e)
        {
            ToolStripButton b = (ToolStripButton)sender;
            getCurrentBrowser().Navigate(b.ToolTipText);
        }

        private void img_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate("about:blank");
        }

        #endregion

        private void LWBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {

                        string[] historyPaths = Directory.GetFiles("C:\\Users\\" + System.Environment.UserName + "\\AppData\\Local\\Microsoft\\Windows\\History");
                foreach (string filePath in historyPaths)
                    File.Delete(filePath);
        }

        private void privacityTsmi_Click(object sender, EventArgs e)
        {
            (new Privacity()).Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            string[] historyPaths = Directory.GetFiles("C:\\Users\\" + System.Environment.UserName + "\\AppData\\Local\\Microsoft\\Windows\\History");
            foreach (string filePath in historyPaths)
                File.Delete(filePath);

            this.Close();
        }

        private void checkUpdatesB_Click(object sender, EventArgs e)
        {
            new UpdateChecker.UpdateChecker(System.Reflection.Assembly.GetExecutingAssembly().Location, "lwbrowser").Show();
        }
    }
}
