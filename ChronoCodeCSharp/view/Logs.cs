using FocusChanged.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FocusChanged.view
{
    partial class Logs : Form
    {


        public Logs(Model.Session session, Boolean isDetailed)
        {

            InitializeComponent();
            this.session = session;
            this.isDetailed = isDetailed;

            configDataGridView();

            buildTable();

            if (isDetailed)
            {
                populateTableDetailled();
            }
            else 
            {
                populatedTable();
            }
        }

        private void populatedTable()
        {
            int elapsedTimeSec = 0;
            int elapsedTimeMili = 0;            

            foreach (Task t in this.session.ListTasks)
            {
                foreach (Period p in t.periods)
                {
                    elapsedTimeSec += p.elapsedTimeSec;
                    elapsedTimeMili += p.elapsedTimeMili;
                }
                addRow(t.ProcessName, "", "", elapsedTimeSec.ToString() + ":" + elapsedTimeMili.ToString());
            }
        }

        private void populateTableDetailled()
        {
            addRow("name","start","end","time");

            foreach(Task t in this.session.ListTasks)
            {
                foreach(Period populateTable in t.periods)
                {
                    addRow(t.ProcessName, populateTable.startDate.ToString(), populateTable.endDate.ToString(), populateTable.elapsedTimeSec.ToString() + ":" + populateTable.elapsedTimeMili.ToString());
                }
            }

        }

        private void addRow(String TaskName,String start,String end,String elapsed)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(this.dataGridViewLogs);
            

            if (this.isDetailed)
            {
                row.Cells[0].Value = TaskName;
                row.Cells[1].Value = start;
                row.Cells[2].Value = end;
                row.Cells[3].Value = elapsed;
            }
            else
            {
                row.Cells[0].Value = TaskName;
                row.Cells[1].Value = elapsed;
            }

            
            this.dataGridViewLogs.Rows.Add(row);
        }

        private void configDataGridView()
        {
           //this.dataGridViewLogs.
        }

        public void buildTable()
        {
            TaskNameColumn = new DataGridViewTextBoxColumn();
            TaskNameColumn.DataPropertyName = "TaskName";
            TaskNameColumn.HeaderText = "Task Name";

            if (this.isDetailed)
            {
                StratTimestampColumn = new DataGridViewTextBoxColumn();
                StratTimestampColumn.DataPropertyName = "StratTimestamp";
                StratTimestampColumn.HeaderText = "Start Timestamp";

                EndTimestampColumn = new DataGridViewTextBoxColumn();
                EndTimestampColumn.DataPropertyName = "EndTimestamp";
                EndTimestampColumn.HeaderText = "End Timestamp";
            }

            ElapsedTimeColumn = new DataGridViewTextBoxColumn();
            ElapsedTimeColumn.DataPropertyName = "ElapsedTime";
            ElapsedTimeColumn.HeaderText = "Elapsed Time";

            this.dataGridViewLogs.Columns.Add(TaskNameColumn);
            if (this.isDetailed)
            {
                this.dataGridViewLogs.Columns.Add(StratTimestampColumn);
                this.dataGridViewLogs.Columns.Add(EndTimestampColumn);
            }
            this.dataGridViewLogs.Columns.Add(ElapsedTimeColumn);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private Model.Session session;
        private DataGridViewTextBoxColumn TaskNameColumn;
        private DataGridViewTextBoxColumn StratTimestampColumn;
        private DataGridViewTextBoxColumn EndTimestampColumn;
        private DataGridViewTextBoxColumn ElapsedTimeColumn;
        public bool isDetailed { get; set; }
    }
}
