using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FocusChanged.Model;
using FocusChanged.Tools;
using System.Collections.Generic;
using System.Threading;
using FocusChanged.view;
using System.IO;
using System.Xml.Serialization;

/**
 * FocusWatcher Handle the focus applications monitoring
 * 
 **/

namespace FocusChanged.WatchDog
{
    public class FocusWatcher : Form
    {
        public FocusWatcher()
        {
            Debug.WriteLine("FW");
            this.isRunning = false;
            this.timerTick = false;
            this.session = new Session(this);
            new DataStream("");
            this.FormClosed += ownCloseHandler;
            InitializeComponent();
            init();
            populateSelectionProcessComboBox();
        }

        /**
         * Fill the combobox that list all running process
         * 
         **/
        private void populateSelectionProcessComboBox()
        {
            this.selectionProcessComboBox.Items.Clear();
            var list = this.dicoSelectionCBox.Values.ToList();
            list.Sort();

            //For user friendly purpose
            this.selectionProcessComboBox.Items.Add("Select a process");
            this.selectionProcessComboBox.Items.Add("----------------");

            this.selectionProcessComboBox.SelectedIndex = 0;

            foreach (String valueFromDico in list)
            {
                this.selectionProcessComboBox.Items.Add(valueFromDico);
            }
        }

        /**
         * When application close a log file is created
         * 
         **/
        private void ownCloseHandler(object sender, FormClosedEventArgs e)
        {
            DataStream dS = new DataStream("logs.txt");
            dS.toLogs(this.session.ListTasks);
        }

        /**
         * Initialisation
         * 
         **/
        private void init()
        {
            initPart_1();

            //Only to display in tha main windows the total elapsed time of all utilisations
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += delegate
            {
                if (this.isRunning)
                {
                    if (this.timerTick)
                    {
                        this.labelTotElapsedTime.Text = sToTime(this.session.getTotElapsedTime() + this.counterUp);
                    }
                    else
                    {
                        this.labelTotElapsedTime.Text = sToTime(this.session.getTotElapsedTime());
                    }
                    this.counterUp++;
                }
            };
            timer.Interval = 1000;
            timer.Start();
        }

        private void initPart_1()
        {
            //list all processes
            this.dicoSelectionCBox = new Dictionary<String, String>();
            //list all ban processes store in the ban.txt file
            this.dSAddBan = new DataStream("ban.txt");
            //list of all watched processes
            this.itemsFromListBoxWatchedProcess = new List<String>();

            populateDictionnary();

            //Declaration of the focus change listener
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            this.counterUp = 0;
            this.labelTotElapsedTime.Text = sToTime(this.session.getTotElapsedTime());
        }

        /**
         * convertion seconds to time string
         * 
         **/
        private string sToTime(int seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);

            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            return answer;
        }

        /**
         * Update combobox that show all avalaible processes
         * 
         **/
        private void majComboBoxBan()
        {
            this.selectionProcessComboBox.Items.Clear();
            this.dicoSelectionCBox.Clear();

            populateDictionnary();
            populateSelectionProcessComboBox();
        }

        /**
         * get all avalaible processes for selection
         * take all processes running and remove ban processes
         * 
         **/
        private void populateDictionnary()
        {
            ProcessesAnalyser pA = new ProcessesAnalyser();
            String key = "", value = "";

            this.dicoSelectionCBox.Clear();

            foreach (Process p in pA.processList)
            {
                value = p.ProcessName;
                key = p.ProcessName;

                if (!this.dicoSelectionCBox.ContainsKey(key))
                {
                    this.dicoSelectionCBox.Add(key, value);
                }
            }
        }

        /**
         * Program core
         * Listener on focus changement
         * 
         **/
        private void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            //if the user didn't start the recording we do nothing
            if (this.isRunning)
            {
                this.timerTick = false;
                AutomationElement element = src as AutomationElement;
                if (element != null)
                {
                    //get focus application process information
                    string name = element.Current.Name;
                    string id = element.Current.AutomationId;
                    int processId = element.Current.ProcessId;

                    //get the process object
                    using (Process process = Process.GetProcessById(processId))
                    {
                        //if it's in the list of watched process
                        if (isTaskToWatch(process.ProcessName))
                        {
                            this.counterUp = 0;
                            this.timerTick = true;
                            //update ellement in session
                            //if first use we have to create a Task to the Session
                            //if not first use we add a Period to the existing Task
                            this.session.update(process.ProcessName, name, id, process.MachineName);
                        }
                        else
                        {
                            //if it's a not watched process we set the prevTask attribute from Session object to null
                            this.session.setPreviousTaskNull();
                        }
                    }
                }
            }
        }

        /**
         * Check if the task is in the list of watched process
         * 
         **/
        private bool isTaskToWatch(string pN)
        {
            foreach (String key in this.itemsFromListBoxWatchedProcess)
            {
                if (key.Equals(pN))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Draw and init UI components
         * 
         **/
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocusWatcher));
            this.selectionProcessComboBox = new System.Windows.Forms.ComboBox();
            this.btnBan = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetBanFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBanFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxWatchedProcess = new FocusChanged.WatchDog.ImprovedListBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.labelTotElapsedTime = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectionProcessComboBox
            // 
            this.selectionProcessComboBox.FormattingEnabled = true;
            this.selectionProcessComboBox.Location = new System.Drawing.Point(13, 28);
            this.selectionProcessComboBox.Name = "selectionProcessComboBox";
            this.selectionProcessComboBox.Size = new System.Drawing.Size(287, 21);
            this.selectionProcessComboBox.TabIndex = 0;
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point(306, 28);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(75, 23);
            this.btnBan.TabIndex = 1;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler(this.btnBan_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(387, 28);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.informationsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(474, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayLogsToolStripMenuItem,
            this.editEntriesToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // displayLogsToolStripMenuItem
            // 
            this.displayLogsToolStripMenuItem.Name = "displayLogsToolStripMenuItem";
            this.displayLogsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.displayLogsToolStripMenuItem.Text = "Reset";
            // 
            // editEntriesToolStripMenuItem
            // 
            this.editEntriesToolStripMenuItem.Name = "editEntriesToolStripMenuItem";
            this.editEntriesToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.editEntriesToolStripMenuItem.Text = "Edit Entries";
            // 
            // informationsToolStripMenuItem
            // 
            this.informationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logsToolStripMenuItem,
            this.shortLogsToolStripMenuItem,
            this.exportCSVToolStripMenuItem});
            this.informationsToolStripMenuItem.Name = "informationsToolStripMenuItem";
            this.informationsToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.informationsToolStripMenuItem.Text = "Informations";
            // 
            // logsToolStripMenuItem
            // 
            this.logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            this.logsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.logsToolStripMenuItem.Text = "Detailled Logs";
            this.logsToolStripMenuItem.Click += new System.EventHandler(this.logsToolStripMenuItem_Click);
            // 
            // shortLogsToolStripMenuItem
            // 
            this.shortLogsToolStripMenuItem.Name = "shortLogsToolStripMenuItem";
            this.shortLogsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.shortLogsToolStripMenuItem.Text = "Short Logs";
            this.shortLogsToolStripMenuItem.Click += new System.EventHandler(this.shortLogsToolStripMenuItem_Click);
            // 
            // exportCSVToolStripMenuItem
            // 
            this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
            this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exportCSVToolStripMenuItem.Text = "Export CSV";
            this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.exportCSVToolStripMenuItem_click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetBanFileToolStripMenuItem,
            this.editBanFileToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // resetBanFileToolStripMenuItem
            // 
            this.resetBanFileToolStripMenuItem.Name = "resetBanFileToolStripMenuItem";
            this.resetBanFileToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.resetBanFileToolStripMenuItem.Text = "Reset Ban File";
            this.resetBanFileToolStripMenuItem.Click += new System.EventHandler(this.resetBanFileToolStripMenuItem_Click);
            // 
            // editBanFileToolStripMenuItem
            // 
            this.editBanFileToolStripMenuItem.Name = "editBanFileToolStripMenuItem";
            this.editBanFileToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.editBanFileToolStripMenuItem.Text = "Edit Ban File";
            this.editBanFileToolStripMenuItem.Click += new System.EventHandler(this.editBanFileToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // listBoxWatchedProcess
            // 
            this.listBoxWatchedProcess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxWatchedProcess.FormattingEnabled = true;
            this.listBoxWatchedProcess.Location = new System.Drawing.Point(13, 55);
            this.listBoxWatchedProcess.Name = "listBoxWatchedProcess";
            this.listBoxWatchedProcess.Size = new System.Drawing.Size(449, 199);
            this.listBoxWatchedProcess.TabIndex = 4;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(387, 260);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Start";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.buttonValidate_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(306, 260);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // labelTotElapsedTime
            // 
            this.labelTotElapsedTime.AutoSize = true;
            this.labelTotElapsedTime.Location = new System.Drawing.Point(13, 265);
            this.labelTotElapsedTime.Name = "labelTotElapsedTime";
            this.labelTotElapsedTime.Size = new System.Drawing.Size(13, 13);
            this.labelTotElapsedTime.TabIndex = 7;
            this.labelTotElapsedTime.Text = "0";
            // 
            // FocusWatcher
            // 
            this.ClientSize = new System.Drawing.Size(474, 290);
            this.Controls.Add(this.labelTotElapsedTime);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.listBoxWatchedProcess);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnBan);
            this.Controls.Add(this.selectionProcessComboBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FocusWatcher";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /**
         * Export to CSV
         * 
         **/
        private void exportCSVToolStripMenuItem_click(object sender, EventArgs e)
        {
            DataStream dS = new DataStream();
            //we have to stop recording because we have to close the last recording period
            if (this.isRunning)
                startPause();
            dS.exportCsv(this.session);
        }

        /**
         * Logs Display
         * 
         **/
        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logs logs = new Logs(this.session, true);
            logs.Show();
        }

        /**
         * Reset Ban list
         * 
         **/
        private void resetBanFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dSAddBan.resetFile();
            majComboBoxBan();
        }

        /**
         * Open a new windows to edit ban processes list
         * 
         **/
        private void editBanFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditBanFile fABF = new FormEditBanFile();

            fABF.Show();
        }

        private String getkeyFromValue(String value)
        {
            foreach (KeyValuePair<String, String> pair in this.dicoSelectionCBox)
            {
                if (value.Equals(pair.Value)) return pair.Key;
            }

            return null;
        }

        /**
         * Ban a process
         * 
         **/
        private void btnBan_Click(object sender, EventArgs e)
        {
            String selectedValue = (String)this.selectionProcessComboBox.SelectedItem;

            String selectedKey = getkeyFromValue(selectedValue);

            this.dSAddBan.addBan(selectedKey);

            majComboBoxBan();
        }

        /**
         * Add process button handler
         * 
         **/
        private void btnAdd_Click(object sender, EventArgs e)
        {
            String selectedValue = (String)this.selectionProcessComboBox.SelectedItem;

            //Add process to list
            addProcessToListBox(selectedValue);
        }

        /**
         * Add a new process to watch
         * 
         **/
        private void addProcessToListBox(String selectedValue)
        {
            //avoid those two value
            if (selectedValue != "Select a process" && selectedValue != "----------------")
            {
                String selectedKey = getkeyFromValue(selectedValue);
                if (selectedKey == null)
                {
                    selectedKey = selectedValue;
                }
                this.selectionProcessComboBox.Items.Remove(selectedValue);
                if (this.dicoSelectionCBox.ContainsKey(selectedKey))
                {
                    this.dicoSelectionCBox.Remove(selectedKey);
                }

                this.itemsFromListBoxWatchedProcess.Add(selectedValue);

                //refresh listbox containing list of watched processes
                updateListBox();

                this.selectionProcessComboBox.SelectedIndex = 0;

            }
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            startPause();
        }

        /**
         * Start/Stop recording watched processes
         * 
         **/
        private void startPause()
        {
            this.session.saveWatchedTasks(parseWatchedProcess());

            this.isRunning = !this.isRunning;

            if (this.isRunning)
            {
                this.btnValidate.Text = "Pause";
            }
            else
            {
                this.btnValidate.Text = "Start";
            }
        }

        /**
         * listbox to ArrayList<>
         * 
         **/
        private ArrayList parseWatchedProcess()
        {
            ArrayList result = new ArrayList();

            foreach (String s in this.listBoxWatchedProcess.Items)
            {
                result.Add(s);
            }

            return result;
        }

        /**
         * Remove a watched process from the list
         * 
         **/
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            String selectedProcessInListBox = (string)this.listBoxWatchedProcess.SelectedItem;

            this.itemsFromListBoxWatchedProcess.Remove(selectedProcessInListBox);
            majComboBoxBan();

            updateListBox();
            populateSelectionProcessComboBox();
        }

        /**
         * Refresh ListBox content
         * 
         **/
        private void updateListBox()
        {
            this.listBoxWatchedProcess.Clear();
            Color color;

            foreach (String s in this.itemsFromListBoxWatchedProcess)
            {
                this.listBoxWatchedProcess.Items.Add(s);
                color = Color.Black;
                //Test if process is running on the machine
                //with save/load systeme, saved processes list can contain inactive process
                //if it's append we tell that to the user by coloring in grey the process name
                if (!isActiveProcess(s))
                {
                    color = Color.Gray;
                }
                this.listBoxWatchedProcess.SetItemColor(this.listBoxWatchedProcess.Items.Count - 1, color);
            }
        }

        /**
         * Determin if a Process exist on the machine
         * 
         **/
        private bool isActiveProcess(String key)
        {
            populateDictionnary();
            return this.dicoSelectionCBox.ContainsKey(key);
        }

        /**
         * Open short logs windows
         * 
         **/
        private void shortLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if second parameters to true it give detailed logs
            Logs logs = new Logs(this.session, false);
            logs.Show();
        }

        /**
         * Save Session in a XML file
         * 
         **/
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.session.saveWatchedTasks(parseWatchedProcess());

            SaveFileDialog file = new SaveFileDialog();

            file.InitialDirectory = "./";
            file.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            file.RestoreDirectory = true;

            string path;

            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
                // serialize session object
                Session.serializeToXML(this.session, path);
                Debug.WriteLine("SAVE");
            }
        }

        /**
         * Load Session from a XML file
         * 
         **/
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //have to init application components
            initPart_1();
            //init process list
            populateSelectionProcessComboBox();
            String path = "./save.xml";
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "./";
            file.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            file.RestoreDirectory = true;

            //load session
            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer _xSer = new XmlSerializer(typeof(Model.Session));

                    Session myObject = (Session)_xSer.Deserialize(fs);

                    this.session = myObject;
                }

                //populate watched processes ListBox
                foreach (String s in this.session.arrayListWatchedProcess)
                {
                    addProcessToListBox(s);
                }

                this.updateListBox();

                //refresh time display label
                this.labelTotElapsedTime.Text = sToTime(this.session.getTotElapsedTime());
            }
        }


        /** INPUTS **/

        public Session session { get; set; }
        private ComboBox selectionProcessComboBox;
        private Button btnBan;
        private Button btnAdd;
        private Dictionary<String, String> dicoSelectionCBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem filesToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem resetBanFileToolStripMenuItem;
        private ImprovedListBox listBoxWatchedProcess;
        private Button btnValidate;
        private Button btnRemove;
        private DataStream dSAddBan;
        public Label labelTotElapsedTime;
        private ToolStripMenuItem actionsToolStripMenuItem;
        private ToolStripMenuItem displayLogsToolStripMenuItem;
        private ToolStripMenuItem editEntriesToolStripMenuItem;
        private ToolStripMenuItem informationsToolStripMenuItem;
        private ToolStripMenuItem logsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem editBanFileToolStripMenuItem;
        private List<String> itemsFromListBoxWatchedProcess;
        private ToolStripMenuItem shortLogsToolStripMenuItem;
        private Boolean isRunning;
        private int counterUp;
        private ToolStripMenuItem exportCSVToolStripMenuItem;
        private Boolean timerTick;
    }
}
