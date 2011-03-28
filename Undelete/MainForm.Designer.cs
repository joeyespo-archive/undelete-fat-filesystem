namespace Undelete
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.tvwDevices = new System.Windows.Forms.TreeView();
            this.lblDevices = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListView();
            this.imlFiles = new System.Windows.Forms.ImageList(this.components);
            this.imlFilesSmall = new System.Windows.Forms.ImageList(this.components);
            this.btnRestore = new System.Windows.Forms.Button();
            this.lstDeleted = new System.Windows.Forms.ListView();
            this.colDeletedName = new System.Windows.Forms.ColumnHeader();
            this.colDeletedAttributes = new System.Windows.Forms.ColumnHeader();
            this.colDeletedSize = new System.Windows.Forms.ColumnHeader();
            this.panFiles = new System.Windows.Forms.Panel();
            this.panNormalFiles = new System.Windows.Forms.Panel();
            this.lblFiles = new System.Windows.Forms.Label();
            this.splFiles = new System.Windows.Forms.Splitter();
            this.btnCloseFileSystem = new System.Windows.Forms.Button();
            this.colDeletedStatus = new System.Windows.Forms.ColumnHeader();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.btnExamine = new System.Windows.Forms.Button();
            this.menuDeleted = new System.Windows.Forms.ContextMenu();
            this.menuDeletedRestore = new System.Windows.Forms.MenuItem();
            this.menuDeletedExamine = new System.Windows.Forms.MenuItem();
            this.btnAbout = new System.Windows.Forms.Button();
            this.colDeletedCluster = new System.Windows.Forms.ColumnHeader();
            this.panFiles.SuspendLayout();
            this.panNormalFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwDevices
            // 
            this.tvwDevices.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
              | System.Windows.Forms.AnchorStyles.Left);
            this.tvwDevices.HideSelection = false;
            this.tvwDevices.ImageIndex = -1;
            this.tvwDevices.Location = new System.Drawing.Point(8, 24);
            this.tvwDevices.Name = "tvwDevices";
            this.tvwDevices.SelectedImageIndex = -1;
            this.tvwDevices.Size = new System.Drawing.Size(208, 396);
            this.tvwDevices.TabIndex = 1;
            this.tvwDevices.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvwDevices_KeyPress);
            this.tvwDevices.DoubleClick += new System.EventHandler(this.tvwDevices_DoubleClick);
            // 
            // lblDevices
            // 
            this.lblDevices.Location = new System.Drawing.Point(8, 8);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(208, 16);
            this.lblDevices.TabIndex = 0;
            this.lblDevices.Text = "Devices:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.btnRefresh.Location = new System.Drawing.Point(8, 428);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 36);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.btnView.Location = new System.Drawing.Point(116, 428);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 36);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "&View >>";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(680, 428);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstFiles.HideSelection = false;
            this.lstFiles.LargeImageList = this.imlFiles;
            this.lstFiles.Location = new System.Drawing.Point(0, 16);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(552, 192);
            this.lstFiles.SmallImageList = this.imlFilesSmall;
            this.lstFiles.TabIndex = 1;
            this.lstFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstFiles_KeyPress);
            this.lstFiles.DoubleClick += new System.EventHandler(this.lstFiles_DoubleClick);
            // 
            // imlFiles
            // 
            this.imlFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imlFiles.ImageSize = new System.Drawing.Size(32, 32);
            this.imlFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlFiles.ImageStream")));
            this.imlFiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imlFilesSmall
            // 
            this.imlFilesSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imlFilesSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imlFilesSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlFilesSmall.ImageStream")));
            this.imlFilesSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnRestore
            // 
            this.btnRestore.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.btnRestore.Location = new System.Drawing.Point(224, 428);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(96, 36);
            this.btnRestore.TabIndex = 5;
            this.btnRestore.Text = "&Restore Files...";
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // lstDeleted
            // 
            this.lstDeleted.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                 this.colDeletedName,
                                                                                 this.colDeletedAttributes,
                                                                                 this.colDeletedSize,
                                                                                 this.colDeletedCluster,
                                                                                 this.colDeletedStatus});
            this.lstDeleted.ContextMenu = this.menuDeleted;
            this.lstDeleted.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstDeleted.FullRowSelect = true;
            this.lstDeleted.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDeleted.HideSelection = false;
            this.lstDeleted.LargeImageList = this.imlFiles;
            this.lstDeleted.Location = new System.Drawing.Point(0, 216);
            this.lstDeleted.Name = "lstDeleted";
            this.lstDeleted.Size = new System.Drawing.Size(552, 196);
            this.lstDeleted.SmallImageList = this.imlFilesSmall;
            this.lstDeleted.TabIndex = 4;
            this.lstDeleted.View = System.Windows.Forms.View.Details;
            this.lstDeleted.DoubleClick += new System.EventHandler(this.lstDeleted_DoubleClick);
            // 
            // colDeletedName
            // 
            this.colDeletedName.Text = "Deleted File";
            this.colDeletedName.Width = 120;
            // 
            // colDeletedAttributes
            // 
            this.colDeletedAttributes.Text = "Attributes";
            this.colDeletedAttributes.Width = 100;
            // 
            // colDeletedSize
            // 
            this.colDeletedSize.Text = "Size";
            this.colDeletedSize.Width = 100;
            // 
            // panFiles
            // 
            this.panFiles.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
              | System.Windows.Forms.AnchorStyles.Left)
              | System.Windows.Forms.AnchorStyles.Right);
            this.panFiles.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                           this.panNormalFiles,
                                                                           this.splFiles,
                                                                           this.lstDeleted});
            this.panFiles.Location = new System.Drawing.Point(224, 8);
            this.panFiles.Name = "panFiles";
            this.panFiles.Size = new System.Drawing.Size(552, 412);
            this.panFiles.TabIndex = 4;
            // 
            // panNormalFiles
            // 
            this.panNormalFiles.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                 this.lstFiles,
                                                                                 this.lblFiles});
            this.panNormalFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panNormalFiles.Name = "panNormalFiles";
            this.panNormalFiles.Size = new System.Drawing.Size(552, 208);
            this.panNormalFiles.TabIndex = 7;
            // 
            // lblFiles
            // 
            this.lblFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(552, 16);
            this.lblFiles.TabIndex = 2;
            this.lblFiles.Text = "Files and Directories:";
            // 
            // splFiles
            // 
            this.splFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splFiles.Location = new System.Drawing.Point(0, 208);
            this.splFiles.Name = "splFiles";
            this.splFiles.Size = new System.Drawing.Size(552, 8);
            this.splFiles.TabIndex = 2;
            this.splFiles.TabStop = false;
            // 
            // btnCloseFileSystem
            // 
            this.btnCloseFileSystem.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.btnCloseFileSystem.Location = new System.Drawing.Point(432, 428);
            this.btnCloseFileSystem.Name = "btnCloseFileSystem";
            this.btnCloseFileSystem.Size = new System.Drawing.Size(96, 36);
            this.btnCloseFileSystem.TabIndex = 5;
            this.btnCloseFileSystem.Text = "&Close";
            this.btnCloseFileSystem.Click += new System.EventHandler(this.btnCloseFileSystem_Click);
            // 
            // colDeletedStatus
            // 
            this.colDeletedStatus.Text = "Status";
            this.colDeletedStatus.Width = 120;
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.FileName = "RestoredFile";
            this.dlgSaveFile.Filter = "All files (*.*)|*.*";
            this.dlgSaveFile.Title = "Restored File To";
            // 
            // btnExamine
            // 
            this.btnExamine.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.btnExamine.Location = new System.Drawing.Point(328, 428);
            this.btnExamine.Name = "btnExamine";
            this.btnExamine.Size = new System.Drawing.Size(96, 36);
            this.btnExamine.TabIndex = 5;
            this.btnExamine.Text = "&Examine...";
            this.btnExamine.Click += new System.EventHandler(this.btnExamine_Click);
            // 
            // menuDeleted
            // 
            this.menuDeleted.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.menuDeletedRestore,
                                                                                this.menuDeletedExamine});
            // 
            // menuDeletedRestore
            // 
            this.menuDeletedRestore.DefaultItem = true;
            this.menuDeletedRestore.Index = 0;
            this.menuDeletedRestore.Text = "&Restore Files...";
            this.menuDeletedRestore.Click += new System.EventHandler(this.menuDeletedRestore_Click);
            // 
            // menuDeletedExamine
            // 
            this.menuDeletedExamine.Index = 1;
            this.menuDeletedExamine.Text = "&Examine...";
            this.menuDeletedExamine.Click += new System.EventHandler(this.menuDeletedExamine_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.btnAbout.Location = new System.Drawing.Point(576, 428);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(96, 36);
            this.btnAbout.TabIndex = 7;
            this.btnAbout.Text = "&About...";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // colDeletedCluster
            // 
            this.colDeletedCluster.Text = "Cluster";
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(780, 469);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.btnAbout,
                                                                  this.panFiles,
                                                                  this.btnRestore,
                                                                  this.btnExit,
                                                                  this.btnView,
                                                                  this.btnRefresh,
                                                                  this.lblDevices,
                                                                  this.tvwDevices,
                                                                  this.btnCloseFileSystem,
                                                                  this.btnExamine});
            this.MinimumSize = new System.Drawing.Size(788, 240);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Undelete";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panFiles.ResumeLayout(false);
            this.panNormalFiles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.ImageList imlFiles;
        private System.Windows.Forms.ImageList imlFilesSmall;
        private System.Windows.Forms.Panel panNormalFiles;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.ColumnHeader colDeletedStatus;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
        private System.Windows.Forms.Button btnExamine;
        private System.Windows.Forms.ContextMenu menuDeleted;
        private System.Windows.Forms.MenuItem menuDeletedRestore;
        private System.Windows.Forms.MenuItem menuDeletedExamine;
        private System.Windows.Forms.TreeView tvwDevices;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ListView lstDeleted;
        private System.Windows.Forms.ColumnHeader colDeletedName;
        private System.Windows.Forms.ColumnHeader colDeletedAttributes;
        private System.Windows.Forms.ColumnHeader colDeletedSize;
        private System.Windows.Forms.Panel panFiles;
        private System.Windows.Forms.Splitter splFiles;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.ColumnHeader colDeletedCluster;
        private System.Windows.Forms.Button btnCloseFileSystem;
    }
}
