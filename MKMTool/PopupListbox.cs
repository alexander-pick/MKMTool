using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MKMTool
{
    /// <summary>
    /// A simple window with title, confirm button and a listbox allowing multiple choice from the specified list.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class PopupListbox : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupListbox"/> class.
        /// </summary>
        /// <param name="title">Title will be displayed both above the list box as well as the window title.</param>
        public PopupListbox(string title)
        {
            InitializeComponent();
            Text = title;
            labelTitle.Text = title;
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Sets the collection to use to create entries in the list.
        /// <seealso cref="UpdateDataSource"/> - does not reset current selection.
        /// </summary>
        /// <param name="dataSource">The data source to use to populate the listbox.</param>
        public void SetDataSource(List<string> dataSource)
        {
            listBoxContent.DataSource = dataSource;
        }

        /// <summary>
        /// If some of the string in the specified dataSource are not in the listbox yet, they will be added.
        /// </summary>
        /// <param name="dataSource">The data source to populate the listbox.</param>
        public void UpdateDataSource(List<string> dataSource)
        {
            foreach (string s in dataSource)
            {
                if (!listBoxContent.Items.Contains(s))
                    listBoxContent.Items.Add(s);
            }
        }

        /// <summary>
        /// Gathers all the selected items and returns them.
        /// </summary>
        /// <returns>Collection of names of the selected items.</returns>
        public List<string> GetSelected()
        {
            List<string> ret = new List<string>();
            foreach (var selected in listBoxContent.SelectedItems)
                ret.Add(selected.ToString());
            return ret;
        }

        /// <summary>
        /// Sets the specified items as selected. Make sure to call SetDataSource before this.
        /// </summary>
        /// <param name="selected">The list of item names to select. Items not in the list will be deselected.
        /// Items in this list not existing in the data source of this PopupListbox will be ignored.</param>
        public void SetSelected(List<string> selected)
        {
            listBoxContent.ClearSelected();
            // for each selected string, find if it is in the listbox's list and if so, set to true
            // this is more efficient assuming selected.Count << listBoxContent.Items.Count
            foreach (string s in selected)
            {
                int index = listBoxContent.Items.IndexOf(s);
                if (index != -1)
                    listBoxContent.SetSelected(index, true);
            }            
        }

        private void listBoxContent_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonConfirm.Text = "OK (" + listBoxContent.SelectedItems.Count + " selected)";
        }
    }
}
