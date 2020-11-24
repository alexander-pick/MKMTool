using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace MKMTool
{
    /// <summary>
    /// A small dialog window for inputing name of the xml file to which to store MKMBotSettings Preset.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class SettingPresetStore : Form
    {
        private MKMBotSettings toStore;
        private string chosenName;

        public SettingPresetStore(MKMBotSettings toStore)
        {
            InitializeComponent();
            this.toStore = toStore;
        }

        /// <summary>
        /// Gets the name finally chosen by the user for the preset.
        /// </summary>
        /// <returns>The name chosen for the preset (without the ".xml" extenstion).</returns>
        public string GetChosenName()
        {
            return chosenName;
        }

        private void buttonStore_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo f = new FileInfo(@".//Presets//" + textBoxFileName.Text + ".xml");
                if (f.Exists)
                {
                    if (MessageBox.Show("Preset with this name already exists.\nOverwrite?", "Preset already exists",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }
                toStore.description = textBoxDescription.Text;
                // first try to write it into a memory stream to see if Save() finishes correctly
                // only if it does, delete the previous file (if there was one) and write into it
                MemoryStream ms = new MemoryStream();
                toStore.Serialize().Save(ms); 
                if (f.Exists)
                    f.Delete();
                FileStream fs = f.OpenWrite();
                ms.Position = 0;
                ms.CopyTo(fs);
                ms.Close();
                fs.Close();
                chosenName = textBoxFileName.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Saving preset to a file failed (make sure the name is a valid name for a file):"
                    + Environment.NewLine + Environment.NewLine + exc.Message + Environment.NewLine,
                    "Storing preset failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
