using System;
using System.IO;
using System.Windows.Forms;

namespace HugeFiles
{
    public partial class MainForm : Form
    {
        private string _rootDir;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _rootDir = DirTextBox.Text;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_rootDir))
            {
                MessageBox.Show("Incorrect root path!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            int count = 0;
            foreach (var control in CountGB.Controls)
            {
                var radioButton = control as RadioButton;
                if (radioButton.Checked)
                {
                    count = int.Parse(radioButton.Text);
                }
            }

            SearchButton.Enabled = false;

            var result = FileExplorerHelper.StartSearchAsync(_rootDir, count);
            var awaiter = result.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                UpdateUI(awaiter.GetResult());
                progressBar.Visible = false;
                progressBar.Enabled = false;
                SearchButton.Enabled = true;
            });

            progressBar.Enabled = true;
            progressBar.Visible = true;
        }

        private void UpdateUI(FileInfo[] result)
        {
            richTextBox.Clear();
            foreach (var file in result)
            {
                richTextBox.AppendText(file.FullName + "  ...................  " + file.Length / 1_048_576 + " MB");
                richTextBox.AppendText(Environment.NewLine);
            }
        }

        private void DirTextBox_TextChanged(object sender, EventArgs e)
        {
            _rootDir = DirTextBox.Text;
        }

    }
}
