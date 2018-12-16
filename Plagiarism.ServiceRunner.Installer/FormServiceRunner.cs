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
using System.Diagnostics;

namespace Plagiarism.ServiceRunner.Installer
{
    public partial class FormServiceRunner : Form
    {
        public FormServiceRunner()
        {
            InitializeComponent();
        }
        private string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plagiarism.ServiceRunner.exe");

        private void ButtonInstall_Click(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(filename);
                    processStartInfo.Arguments = "install --autostart start";
                    Process.Start(processStartInfo).WaitForExit(1000);
                    MessageBox.Show("Service installed successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Service failed to install.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonUninstall_Click(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(filename);
                    processStartInfo.Arguments = " uninstall";
                    Process.Start(processStartInfo).WaitForExit(1000);
                    MessageBox.Show("Service uninstalled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Service failed to uninstall.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
