namespace UpdateChecker
{
    partial class UpdateChecker
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateChecker));
            this.checkB = new System.Windows.Forms.Button();
            this.infoL = new System.Windows.Forms.Label();
            this.checkingPB = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // checkB
            // 
            resources.ApplyResources(this.checkB, "checkB");
            this.checkB.Name = "checkB";
            this.checkB.UseVisualStyleBackColor = true;
            this.checkB.Click += new System.EventHandler(this.checkB_Click);
            // 
            // infoL
            // 
            resources.ApplyResources(this.infoL, "infoL");
            this.infoL.Name = "infoL";
            // 
            // checkingPB
            // 
            resources.ApplyResources(this.checkingPB, "checkingPB");
            this.checkingPB.MarqueeAnimationSpeed = 0;
            this.checkingPB.Name = "checkingPB";
            this.checkingPB.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // UpdateChecker
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkingPB);
            this.Controls.Add(this.infoL);
            this.Controls.Add(this.checkB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateChecker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button checkB;
        private System.Windows.Forms.Label infoL;
        private System.Windows.Forms.ProgressBar checkingPB;
    }
}

