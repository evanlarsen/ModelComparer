using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MowSessionValidator
{
    internal class FolderDialog
    {
        public string GetFolder()
        {
            var dlg = new FolderBrowserDialog();
            dlg.Description = "Choose the folder where m.alaskaair.com.dll exists";
            dlg.ShowNewFolderButton = false;

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            return null;
        }
    }
}
