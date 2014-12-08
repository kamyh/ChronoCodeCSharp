using FocusChanged.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocusChanged.view
{
    public partial class FormEditBanFile : Form
    {
        public FormEditBanFile()
        {
            InitializeComponent();

            initListBanProcess();
        }

        private void initListBanProcess()
        {
            dS = new DataStream("./ban.txt");

            populateListBanProcesses();
        }

        private void populateListBanProcesses()
        {
            this.listBanProcess.Items.Clear();

            allBanProcesses = dS.getBannedProcess();

            allBanProcesses.Sort();

            foreach (String process in allBanProcesses)
            {
                this.listBanProcess.Items.Add(process);
            }
        }

        private void btnRemoveBan_Click(object sender, EventArgs e)
        {
            if(this.listBanProcess.SelectedItem != null)
            {
                this.allBanProcesses.Remove(this.listBanProcess.SelectedItem);
                dS.updateBanList(this.allBanProcesses);
                populateListBanProcesses();
            }
        }

        private DataStream dS;
        private ArrayList allBanProcesses;
    }
}
