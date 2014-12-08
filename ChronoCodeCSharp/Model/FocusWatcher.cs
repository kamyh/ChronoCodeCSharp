using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using FocusChanged.WatchDog;

namespace FocusChanged.Model
{
    public partial class FocusWatcher : Form
    {
        public FocusWatcher()
        {
            InitializeComponent();

            this.FormClosed += ownCloseHandler;
        }

        private void ownCloseHandler(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("Closing App");
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private FocusWatcher fW;
    }
}
