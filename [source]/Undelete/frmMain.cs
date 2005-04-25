using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Undelete
{
  public class frmMain : System.Windows.Forms.Form
  {
    FATManager fileManager = null;
    
    #region Class Controls
    
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
    private System.ComponentModel.IContainer components;
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
    
    #endregion
    
    #region Class Construction
    
    public frmMain ()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
    }
    
    #region Windows Form Designer generated code
    
    /// <summary> Clean up any resources being used. </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }
    
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
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
    
    #endregion
    
    #region Entry Point
    
    [STAThread]
    public static void Main ()
    { Application.Run(new frmMain()); }
    
    #endregion
    

    private void frmMain_Load (object sender, System.EventArgs e)
    { RefreshDevices(); }
    
    private void btnExit_Click (object sender, System.EventArgs e)
    { this.Close(); }
    
    private void btnRefresh_Click (object sender, System.EventArgs e)
    { RefreshDevices(); }
    
    private void tvwDevices_DoubleClick (object sender, System.EventArgs e)
    { btnView_Click(btnView, EventArgs.Empty); }
    private void tvwDevices_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    { if (e.KeyChar == 13) { btnView_Click(btnView, EventArgs.Empty); e.Handled = true; } }
    private void btnView_Click (object sender, System.EventArgs e)
    {
      if (tvwDevices.SelectedNode == null) return;
      if (tvwDevices.SelectedNode.Tag.GetType() != typeof(LogicalDevice)) return;
      LogicalDevice volume = (LogicalDevice)tvwDevices.SelectedNode.Tag;
      if (!volume.Partition.IsFAT)
      { MessageBox.Show(this, "'"+volume.Partition.PartitionLabel+"' file system not supported.", "Undelete"); return; }
      ViewFileSystem(volume);
    }
    private void btnCloseFileSystem_Click(object sender, System.EventArgs e)
    { CloseFileSystem(); }
    
    private void lstFiles_DoubleClick (object sender, System.EventArgs e)
    {
      if (lstFiles.SelectedItems.Count == 0) return;
      UpdateFiles((FileEntry)lstFiles.SelectedItems[0].Tag);
    }
    private void lstFiles_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      if (e.KeyChar == 13)
      {
        if (lstFiles.SelectedItems.Count == 0) return;
        UpdateFiles((FileEntry)lstFiles.SelectedItems[0].Tag);
        e.Handled = true;
      }
    }
    
    private void btnRestore_Click (object sender, System.EventArgs e)
    {
      foreach (ListViewItem item in lstDeleted.SelectedItems)
        if (RestoreFile((FileEntry)item.Tag) == false) break;
    }
    private void lstDeleted_DoubleClick(object sender, System.EventArgs e)
    { btnRestore_Click(btnRestore, EventArgs.Empty); }
    private void menuDeletedRestore_Click (object sender, System.EventArgs e)
    { btnRestore_Click(btnRestore, EventArgs.Empty); }
    
    private void btnExamine_Click (object sender, System.EventArgs e)
    {
      foreach (ListViewItem item in lstDeleted.SelectedItems)
        ExamineFile((FileEntry)item.Tag);
    }
    private void menuDeletedExamine_Click(object sender, System.EventArgs e)
    { btnExamine_Click(btnExamine, EventArgs.Empty); }
    
    private void btnAbout_Click (object sender, System.EventArgs e)
    { MessageBox.Show(this, "Undelete Program\nBy Joe Esposito\n\nNOTE: It is strongly recommended that you restore files to a medium -other- than\nthe original device to ensure there is no conflict.", "About Undelete", MessageBoxButtons.OK, MessageBoxIcon.Information); }
    
    
    private void RefreshDevices ()
    {
      CloseDevices();
      for (int i = 0;; i++)
      {
        PhysicalDevice device = PhysicalDevice.FromPhysicalDrive(i);
        if (device == null) break;
        if (device.DeviceStream == null) continue;  // Skip empty streams
        TreeNode physicalDeviceNode = new TreeNode("Disk " + i.ToString());
        physicalDeviceNode.Tag = device;
        RefreshVolumes(physicalDeviceNode);
        tvwDevices.Nodes.Add(physicalDeviceNode);
        tvwDevices.ExpandAll();
      }
    }
    
    private void RefreshVolumes (TreeNode physicalDeviceNode)
    {
      physicalDeviceNode.Nodes.Clear();
      PhysicalDevice device = (PhysicalDevice)physicalDeviceNode.Tag;
      foreach (LogicalDevice volume in device.LogicalDevices)
      {
        TreeNode volumeNode = new TreeNode( ( volume.BootSector != null )?( volume.BootSector.VolumeLabel.Replace('\0', ' ').Trim() ):( "" ) );
        if (volumeNode.Text == "") volumeNode.Text += "Partition " + physicalDeviceNode.Nodes.Count;
        volumeNode.Text += " (" + volume.Partition.PartitionLabel + ")";
        volumeNode.ForeColor = (( volume.Partition.IsFAT )?( Color.MediumSeaGreen ):( Color.Gray ));
        volumeNode.Tag = volume;
        physicalDeviceNode.Nodes.Add(volumeNode);
      }
    }
    
    private void CloseDevices ()
    {
      tvwDevices.Nodes.Clear();
      foreach (TreeNode node in tvwDevices.Nodes)
      {
        ((PhysicalDevice)node.Tag).DeviceStream.Close();
        foreach (TreeNode child in node.Nodes)
          ((LogicalDevice)child.Tag).DeviceStream.Close();
      }
    }
    
    private void ViewFileSystem (LogicalDevice volume)
    {
      CloseFileSystem();
      fileManager = new FATManager(volume);
      RefreshFiles();
      lstFiles.Select();
    }
    private void CloseFileSystem ()
    {
      fileManager = null;
      RefreshFiles();
    }
    
    private void UpdateFiles (FileEntry file)
    {
      if (fileManager == null) return;
      if (!file.IsDirectory)
      {
        frmViewer viewer = new frmViewer(file.FormattedName, fileManager.ReadFileAsString(file));
        viewer.Owner = this;
        viewer.Show();
        return;
      }
      try
      { fileManager.ChangeDirectory(file.FormattedName); }
      catch (DirectoryNotFoundException)
      { MessageBox.Show(this, "Directory does not exist.", "Undelete"); }
      RefreshFiles();
    }
    
    private void RefreshFiles ()
    {
      lstFiles.Items.Clear();
      lstDeleted.Items.Clear();
      if (fileManager == null) return;
      
      // Populate files and directories
      foreach (FileEntry file in fileManager.EnumerateFileEntries())
      {
        if (file.FormattedName == ".") continue;
        ListViewItem item = new ListViewItem(file.FormattedName);
        item.ImageIndex = (( file.IsDirectory )?( 0 ):( 1 ));
        item.Tag = file;
        lstFiles.Items.Add(item);
      }
      if (lstFiles.Items.Count > 0) lstFiles.Items[0].Selected = true;
      
      // Aquire nearby files
      FileEntry [] nearbyFiles = fileManager.EnumerateFileEntries();
      FileEntry [] nearbyDeletedFiles = fileManager.EnumerateDeletedFileEntries();
      
      // Populate 'deleted' files
      foreach (FileEntry file in fileManager.EnumerateDeletedFiles())
      {
        bool overwritten = false, conflicted = false;
        
        // Check nearby files for overwritten cluster
        foreach (FileEntry testFile in nearbyFiles)
        { if (testFile.FirstCluster == file.FirstCluster) { overwritten = true; break; } }
        // Check nearby deleted files for overwritten cluster
        foreach (FileEntry testFile in nearbyDeletedFiles)
        {
          if ((testFile.Name == file.Name) && (testFile.FileSize == file.FileSize) && (testFile.CreateTime == file.CreateTime)
            && (testFile.CreateDate == file.CreateDate) && (testFile.CreateTimeMili == file.CreateTimeMili)) continue;
          if (testFile.FirstCluster == file.FirstCluster) { conflicted = true; break; }
        }
        // Append item to 'deleted' list
        ListViewItem item = new ListViewItem(new String [] { file.FormattedName, ((FileAttributes)file.Attributes).ToString(), file.FormattedSize, file.FirstCluster.ToString(), (( overwritten )?( "Overwritten" ):( (( conflicted )?( "Conflicted" ):( "" )) )) } );
        item.ImageIndex = (( file.IsDirectory )?( 0 ):( 1 ));
        if ((overwritten) || (conflicted)) item.ForeColor = Color.Firebrick;
        item.Tag = file;
        lstDeleted.Items.Add(item);
      }
      if (lstDeleted.Items.Count > 0) lstDeleted.Items[0].Selected = true;
    }
    
    private bool RestoreFile (FileEntry file)
    {
      if (fileManager == null) return false;
      if (!file.IsFile) return true;
      string fileName = file.FormattedName;
      if (fileName[0] == '?') fileName = '!' + (( fileName.Length > 1 )?( fileName.Substring(1) ):( "" ));
      dlgSaveFile.FileName = fileName;
      if (dlgSaveFile.ShowDialog(this) == DialogResult.Cancel) return false;
      FileStream fs = File.Create(dlgSaveFile.FileName);
      fileManager.SaveFile(file, fs);
      fs.Close();
      return true;
    }
    
    private void ExamineFile (FileEntry file)
    {
      if (fileManager == null) return;
      if (!file.IsFile) return;
      frmViewer viewer = new frmViewer(file.FormattedName, fileManager.ReadFileAsString(file));
      viewer.Owner = this;
      viewer.Show();
    }
  }
}
