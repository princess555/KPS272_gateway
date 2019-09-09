using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayKPS272_GetWay
{
    public partial class Form1 : Form
    {
        public delegate void NewMessageDelegate(string NewMessage);
 
        private PipeServer pipeServer;

        public Form1()
        {
            InitializeComponent();
            pipeServer = new PipeServer();
            pipeServer.PipeMessage += new DelegateMessage(PipesMessageHandler);

            //dataGridView1.ColumnCount = 4;
            //dataGridView1.Columns[0].Name = "1";
            //dataGridView1.Columns[1].Name = "2";
            //dataGridView1.Columns[2].Name = "3";
            //dataGridView1.Columns[3].Name = "4";

            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ButtonShow_Click(object sender, EventArgs e)
        {
            try
            {
                pipeServer.Listen("Connection");
                buttonShow.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error - " + ex.Message);
            }
        }

        private void PipesMessageHandler (string message)
        {
            string[] words = message.Split(' ');
            try
            {
                if (this.InvokeRequired)
                {

                     this.Invoke(new NewMessageDelegate(PipesMessageHandler), words);
                }
                else
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        dataGridView1.Rows.Add(words[0]);
                        dataGridView1.Rows.Add(words[1]);
                        dataGridView1.Rows.Add(words[2]);
                        dataGridView1.Rows.Add(words[3]);
                    }
                    
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //pipeServer.PipeMessage -= new DelegateMessage(PipesMessageHandler);
            pipeServer = null;

        }
    }
}
