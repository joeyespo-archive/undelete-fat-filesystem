namespace Undelete
{
    partial class ViewerForm
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtContents = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtContents
            // 
            this.txtContents.BackColor = System.Drawing.SystemColors.Window;
            this.txtContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContents.Multiline = true;
            this.txtContents.Name = "txtContents";
            this.txtContents.ReadOnly = true;
            this.txtContents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContents.Size = new System.Drawing.Size(464, 373);
            this.txtContents.TabIndex = 0;
            this.txtContents.Text = "";
            this.txtContents.WordWrap = false;
            // 
            // frmViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(464, 373);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.txtContents});
            this.Name = "frmViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Contents Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.TextBox txtContents;
    }
}
