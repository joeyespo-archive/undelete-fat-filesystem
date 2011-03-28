using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Undelete
{
    /// <summary>
    /// Represents the viewer dialog.
    /// </summary>
    public sealed partial class ViewerForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewerForm"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file to view.</param>
        /// <param name="contents">The contents to show.</param>
        public ViewerForm(string fileName, string contents)
        {
            InitializeComponent();

            Text += " - " + fileName;
            txtContents.Text = contents;
            txtContents.Select(0, 0);
        }
    }
}
