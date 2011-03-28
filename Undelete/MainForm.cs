using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Undelete
{
    /// <summary>
    /// Represents the main form.
    /// </summary>
    public sealed partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #region Event Handler Methods

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            RefreshDevices();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            RefreshDevices();
        }

        private void tvwDevices_DoubleClick(object sender, System.EventArgs e)
        {
            btnView_Click(btnView, EventArgs.Empty);
        }

        private void tvwDevices_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView_Click(btnView, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            if(tvwDevices.SelectedNode == null)
                return;
            if(tvwDevices.SelectedNode.Tag.GetType() != typeof(LogicalDevice))
                return;
            LogicalDevice volume = (LogicalDevice)tvwDevices.SelectedNode.Tag;
            if(!volume.Partition.IsFAT)
            {
                MessageBox.Show(this, "'" + volume.Partition.PartitionLabel + "' file system not supported.", "Undelete");
                return;
            }
            ViewFileSystem(volume);
        }

        private void btnCloseFileSystem_Click(object sender, System.EventArgs e)
        {
            CloseFileSystem();
        }

        private void lstFiles_DoubleClick(object sender, System.EventArgs e)
        {
            if(lstFiles.SelectedItems.Count == 0)
                return;
            UpdateFiles((FileEntry)lstFiles.SelectedItems[0].Tag);
        }

        private void lstFiles_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(lstFiles.SelectedItems.Count == 0)
                    return;
                UpdateFiles((FileEntry)lstFiles.SelectedItems[0].Tag);
                e.Handled = true;
            }
        }

        private void btnRestore_Click(object sender, System.EventArgs e)
        {
            foreach(ListViewItem item in lstDeleted.SelectedItems)
                if(RestoreFile((FileEntry)item.Tag) == false)
                    break;
        }
        private void lstDeleted_DoubleClick(object sender, System.EventArgs e)
        {
            btnRestore_Click(btnRestore, EventArgs.Empty);
        }

        private void menuDeletedRestore_Click(object sender, System.EventArgs e)
        {
            btnRestore_Click(btnRestore, EventArgs.Empty);
        }

        private void btnExamine_Click(object sender, System.EventArgs e)
        {
            foreach(ListViewItem item in lstDeleted.SelectedItems)
                ExamineFile((FileEntry)item.Tag);
        }

        private void menuDeletedExamine_Click(object sender, System.EventArgs e)
        {
            btnExamine_Click(btnExamine, EventArgs.Empty);
        }

        private void btnAbout_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show(this, "Undelete Program\nBy Joe Esposito\n\nNOTE: It is strongly recommended that you restore files to a medium -other- than\nthe original device to ensure there is no conflict.", "About Undelete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void RefreshDevices()
        {
            CloseDevices();
            for(int i = 0; ; i++)
            {
                PhysicalDevice device = PhysicalDevice.FromPhysicalDrive(i);
                if(device == null)
                    break;
                if(device.DeviceStream == null)
                    continue;  // Skip empty streams
                TreeNode physicalDeviceNode = new TreeNode("Disk " + i.ToString());
                physicalDeviceNode.Tag = device;
                RefreshVolumes(physicalDeviceNode);
                tvwDevices.Nodes.Add(physicalDeviceNode);
                tvwDevices.ExpandAll();
            }
        }

        private void RefreshVolumes(TreeNode physicalDeviceNode)
        {
            physicalDeviceNode.Nodes.Clear();
            PhysicalDevice device = (PhysicalDevice)physicalDeviceNode.Tag;
            foreach(LogicalDevice volume in device.LogicalDevices)
            {
                TreeNode volumeNode = new TreeNode((volume.BootSector != null) ? (volume.BootSector.VolumeLabel.Replace('\0', ' ').Trim()) : (""));
                if(volumeNode.Text == "")
                    volumeNode.Text += "Partition " + physicalDeviceNode.Nodes.Count;
                volumeNode.Text += " (" + volume.Partition.PartitionLabel + ")";
                volumeNode.ForeColor = ((volume.Partition.IsFAT) ? (Color.MediumSeaGreen) : (Color.Gray));
                volumeNode.Tag = volume;
                physicalDeviceNode.Nodes.Add(volumeNode);
            }
        }

        private void CloseDevices()
        {
            tvwDevices.Nodes.Clear();
            foreach(TreeNode node in tvwDevices.Nodes)
            {
                ((PhysicalDevice)node.Tag).DeviceStream.Close();
                foreach(TreeNode child in node.Nodes)
                    ((LogicalDevice)child.Tag).DeviceStream.Close();
            }
        }

        private void ViewFileSystem(LogicalDevice volume)
        {
            CloseFileSystem();
            fileManager = new FATManager(volume);
            RefreshFiles();
            lstFiles.Select();
        }

        private void CloseFileSystem()
        {
            fileManager = null;
            RefreshFiles();
        }

        private void UpdateFiles(FileEntry file)
        {
            if(fileManager == null)
                return;
            if(!file.IsDirectory)
            {
                ViewerForm viewer = new ViewerForm(file.FormattedName, fileManager.ReadFileAsString(file));
                viewer.Owner = this;
                viewer.Show();
                return;
            }
            try
            {
                fileManager.ChangeDirectory(file.FormattedName);
            }
            catch(DirectoryNotFoundException)
            {
                MessageBox.Show(this, "Directory does not exist.", "Undelete");
            }
            RefreshFiles();
        }

        private void RefreshFiles()
        {
            lstFiles.Items.Clear();
            lstDeleted.Items.Clear();
            if(fileManager == null)
                return;

            // Populate files and directories
            foreach(FileEntry file in fileManager.EnumerateFileEntries())
            {
                if(file.FormattedName == ".")
                    continue;
                ListViewItem item = new ListViewItem(file.FormattedName);
                item.ImageIndex = ((file.IsDirectory) ? (0) : (1));
                item.Tag = file;
                lstFiles.Items.Add(item);
            }
            if(lstFiles.Items.Count > 0)
                lstFiles.Items[0].Selected = true;

            // Aquire nearby files
            FileEntry[] nearbyFiles = fileManager.EnumerateFileEntries();
            FileEntry[] nearbyDeletedFiles = fileManager.EnumerateDeletedFileEntries();

            // Populate 'deleted' files
            foreach(FileEntry file in fileManager.EnumerateDeletedFiles())
            {
                bool overwritten = false, conflicted = false;

                // Check nearby files for overwritten cluster
                foreach(FileEntry testFile in nearbyFiles)
                {
                    if(testFile.FirstCluster == file.FirstCluster)
                    {
                        overwritten = true;
                        break;
                    }
                }
                // Check nearby deleted files for overwritten cluster
                foreach(FileEntry testFile in nearbyDeletedFiles)
                {
                    if((testFile.Name == file.Name) && (testFile.FileSize == file.FileSize) && (testFile.CreateTime == file.CreateTime)
                      && (testFile.CreateDate == file.CreateDate) && (testFile.CreateTimeMili == file.CreateTimeMili))
                        continue;
                    if(testFile.FirstCluster == file.FirstCluster)
                    {
                        conflicted = true;
                        break;
                    }
                }
                // Append item to 'deleted' list
                ListViewItem item = new ListViewItem(new String[] { file.FormattedName, ((FileAttributes)file.Attributes).ToString(), file.FormattedSize, file.FirstCluster.ToString(), ((overwritten) ? ("Overwritten") : (((conflicted) ? ("Conflicted") : ("")))) });
                item.ImageIndex = ((file.IsDirectory) ? (0) : (1));
                if((overwritten) || (conflicted))
                    item.ForeColor = Color.Firebrick;
                item.Tag = file;
                lstDeleted.Items.Add(item);
            }
            if(lstDeleted.Items.Count > 0)
                lstDeleted.Items[0].Selected = true;
        }

        private bool RestoreFile(FileEntry file)
        {
            if(fileManager == null)
                return false;
            if(!file.IsFile)
                return true;
            string fileName = file.FormattedName;
            if(fileName[0] == '?')
                fileName = '!' + ((fileName.Length > 1) ? (fileName.Substring(1)) : (""));
            dlgSaveFile.FileName = fileName;
            if(dlgSaveFile.ShowDialog(this) == DialogResult.Cancel)
                return false;
            FileStream fs = File.Create(dlgSaveFile.FileName);
            fileManager.SaveFile(file, fs);
            fs.Close();
            return true;
        }

        private void ExamineFile(FileEntry file)
        {
            if(fileManager == null)
                return;
            if(!file.IsFile)
                return;
            ViewerForm viewer = new ViewerForm(file.FormattedName, fileManager.ReadFileAsString(file));
            viewer.Owner = this;
            viewer.Show();
        }

        private FATManager fileManager = null;
    }
}
