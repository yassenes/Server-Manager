using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Manager
{
    public struct ProcessInfo
    {
        public string FileName;
        public Process FileProcess;
    }

    public partial class MgrForm : System.Windows.Forms.Form
    {
        public static Dictionary<string, ProcessInfo> dProcessList;

        public MgrForm()
        {
            InitializeComponent();
            dProcessList = new Dictionary<string, ProcessInfo>();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (appList.CheckedItems.Count > 0)
            {
                foreach (var i in appList.CheckedItems)
                {
                    appList.Items.Remove(i);
                }
            }
            else
            {
                MessageBox.Show("There are no executables in the list.");
            }
        }

        private void MgrForm_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Ace");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "exe files|*.exe",
                Multiselect = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var f in ofd.FileNames)
                {
                    ProcessInfo processInfo = new ProcessInfo()
                    {
                        FileName = f,
                        FileProcess = null
                    };
                    dProcessList.Add(Path.GetFileName(f), processInfo);
                    appList.Items.Add(Path.GetFileName(f));
                }
            }
        }

        private void appList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void appCount_Click(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (appList.CheckedItems.Count > 0)
            {
                foreach (var i in appList.CheckedItems)
                {
                    ProcessInfo processInfo = dProcessList[i.ToString()];

                    if (processInfo.FileProcess != null)
                    {
                        if (!processInfo.FileProcess.HasExited)
                            return;
                    }

                    ProcessStartInfo start = new ProcessStartInfo()
                    {
                        FileName = processInfo.FileName,
                    };

                    processInfo.FileProcess = new Process();
                    processInfo.FileProcess = Process.Start(start);

                    dProcessList[i.ToString()] = processInfo;
                }
            }
            else
            {
                MessageBox.Show("There are no executables in the list.");
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (appList.CheckedItems.Count > 0)
            {
                foreach (var i in appList.CheckedItems)
                {
                    try
                    {
                        ProcessInfo processInfo = dProcessList[i.ToString()];

                        if (processInfo.FileProcess != null)
                        {
                            processInfo.FileProcess.Kill();
                            processInfo.FileProcess = null;

                            dProcessList[i.ToString()] = processInfo;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no executables in the list.");
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            if (appList.CheckedItems.Count > 0)
            {
                foreach (var i in appList.CheckedItems)
                {
                    try
                    {
                        ProcessInfo processInfo = dProcessList[i.ToString()];

                        if (processInfo.FileProcess != null)
                        {
                            processInfo.FileProcess.Kill();

                            Cursor.Current = Cursors.WaitCursor;

                            Thread.Sleep(3000);

                            ProcessStartInfo start = new ProcessStartInfo()
                            {
                                FileName = processInfo.FileName,
                            };

                            processInfo.FileProcess = new Process();
                            processInfo.FileProcess = Process.Start(start);

                            Cursor.Current = Cursors.Default;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no executables in the list.");
            }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really wish to exit this program?", "Exit confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void appCountText_Click(object sender, EventArgs e)
        {

        }
    }
}
