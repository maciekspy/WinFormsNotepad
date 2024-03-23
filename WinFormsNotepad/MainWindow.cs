using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsNotepad
{
    public partial class MainWindow : Form
    {
        private string _filename = null;
        private bool _modified = false;

        public MainWindow()
        {
            InitializeComponent();

            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (_filename != null)
            {
                Text = $"{(_modified ? "*" : "")}{Path.GetFileName(_filename)} - Notepad";
            }
            else
            {
                Text = "Untitled - Notepad";
            }
        }

        private void OpenFile(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                textArea.Text = sr.ReadToEnd();
            }
            _filename = filename;
            _modified = false;
            UpdateTitle();
        }

        private void SaveFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(textArea.Text);
            }
            _filename = filename;
            _modified = false;
            UpdateTitle();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textArea.Text = "";
        }

        private void UpdateTextCursorPosition()
        {
            // "Borrowed" from: https://stackoverflow.com/questions/66475/how-can-i-get-column-number-of-the-cursor-in-a-textbox-in-c
            int line = textArea.GetLineFromCharIndex(textArea.SelectionStart);
            int column = textArea.SelectionStart - textArea.GetFirstCharIndexFromLine(line);

            sbTextPosLabel.Text = $"Ln {line}, Col {column}";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter =
                    "Text files (*.txt)|*.txt|" +
                    "JSON files (*.json)|*.json|" +
                    "HTML files (*.html;*.htm)|*.html;*.htm|" +
                    "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 4;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    OpenFile(openFileDialog.FileName);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter =
                    "Text files (*.txt)|*.txt|" +
                    "JSON files (*.json)|*.json|" +
                    "HTML files (*.html;*.htm)|*.html;*.htm|" +
                    "All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 4;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(saveFileDialog.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filename == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }

            SaveFile(_filename);
        }

        private void textArea_TextChanged(object sender, EventArgs e)
        {
            if (_modified == false)
            {
                _modified = true;
                UpdateTitle();
            }

            UpdateTextCursorPosition();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();

            aboutForm.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: 
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();

            //TODO: pageSetupDialog.ShowDialog();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;

            textArea.WordWrap = wordWrapToolStripMenuItem.Checked;

        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;

            statusBar.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowEffects = false;

            fontDialog.Font = textArea.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textArea.Font = fontDialog.Font;
            }
        }

        private void textArea_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateTextCursorPosition();
        }

        private void textArea_KeyPress(object sender, KeyPressEventArgs e)
        {

            UpdateTextCursorPosition();
        }
    }
}
