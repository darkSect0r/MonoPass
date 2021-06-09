using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using mpdata;

namespace MonoPass
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.MultiSelect = false;
        }

        private ListViewItem _itemDnD = null;
        mpdatacl mpdata = new mpdatacl();

        private void saveF()
        {
            RichTextBox rtbs = new RichTextBox();
            if (!File.Exists("Data\\DoNotDelete")) {rtbs.SaveFile("Data\\DoNotDelete", RichTextBoxStreamType.PlainText); }
            int len = textBox4.Text.Length;
            len = 32 - len;
            StringBuilder sb = new StringBuilder(textBox4.Text);
            sb.Append('A', len);
            string pass = sb.ToString();
            foreach (ListViewItem lvi in listView1.Items)
            {
                rtbs.AppendText("---\n" + mpdatacl.EncryptString(pass, lvi.Text) + "\n" + mpdatacl.EncryptString(pass, lvi.SubItems[1].Text) + "\n" + mpdatacl.EncryptString(pass, lvi.SubItems[2].Text) + "\n");
            }
            rtbs.SaveFile("Data\\DoNotDelete", RichTextBoxStreamType.PlainText);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RichTextBox rtb = new RichTextBox();
            if (!File.Exists("Data\\DoNotDelete")) { rtb.SaveFile("Data\\DoNotDelete", RichTextBoxStreamType.PlainText); }
            Form3 form3 = new Form3();
            form3.ShowDialog();
            textBox4.Text = form3.textBox1.Text;
            int len = textBox4.Text.Length;
            len = 32 - len;
            StringBuilder sb = new StringBuilder(textBox4.Text);
            sb.Append('A', len);
            string pass = sb.ToString();
            int line = 0;
            try
            {
                rtb.LoadFile("Data\\DoNotDelete", RichTextBoxStreamType.PlainText);
                foreach (string l in rtb.Lines)
                {
                    if (l is "---")
                    {
                        ListViewItem lvi = new ListViewItem(mpdatacl.DecryptString(pass, rtb.Lines[line + 1]));
                        lvi.SubItems.Add(mpdatacl.DecryptString(pass, rtb.Lines[line + 2]));
                        lvi.SubItems.Add(mpdatacl.DecryptString(pass, rtb.Lines[line + 3]));
                        listView1.Items.Add(lvi);
                    }
                    line += 1;
                }
            }
            catch
            {
                MessageBox.Show("Error loading list, you may have entered the wrong Master Password! Please restart program to fix.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = false; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                form2.groupBox1.Text = listView1.Items[listView1.FocusedItem.Index].Text;
                form2.maskedTextBox1.Text = listView1.Items[listView1.FocusedItem.Index].SubItems[1].Text;
                form2.maskedTextBox2.Text = listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text;
                form2.ShowDialog();
                listView1.Items[listView1.FocusedItem.Index].SubItems[1].Text = form2.maskedTextBox1.Text;
                listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text = form2.maskedTextBox2.Text;
            }
            catch
            {
                MessageBox.Show("You must first pick an item to display details for.", "Opps", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) | string.IsNullOrEmpty(textBox2.Text) | string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Missing one or more entries! Please fill out \"Name\", \"ID\", and \"Password\" fields properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                listView1.Items.Add(textBox1.Text).SubItems.AddRange(new string[] { textBox2.Text, textBox3.Text });
                textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox1.Focus();
                try
                {
                    saveF();
                    MessageBox.Show("Login details added and saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Error saving login details, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                saveF();
                MessageBox.Show("Password List saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Error saving Password List, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (button1.Text != "Nothing Selected")
            {
                DialogResult msgResult = MessageBox.Show("Are you sure you want to delete " + button1.Text + "? This cannot be undone!", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msgResult == DialogResult.Yes)
                {
                    try
                    {
                        listView1.SelectedItems[0].Remove();
                    }
                    catch
                    {
                        MessageBox.Show("Error deleting list item! Restart MonoPass and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("You havn't selected anything! Make a selection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            switch (textBox3.PasswordChar)
            {
                case '\0':
                    textBox3.PasswordChar = '¤';
                    break;
                case '¤':
                    textBox3.PasswordChar = '\0';
                    break;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            switch (textBox4.PasswordChar)
            {
                case '\0':
                    textBox4.PasswordChar = '¤';
                    break;
                case '¤':
                    textBox4.PasswordChar = '\0';
                    break;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.darksector.info/");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int len = textBox4.Text.Length;
            label9.Text = len.ToString();
            textBox4.MaxLength = 32;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button3.PerformClick();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button5.PerformClick();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                saveF();
                MessageBox.Show("Master Password saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Error saving Master Password! Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1.PerformClick();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string label2txt = listView1.Items[listView1.FocusedItem.Index].Text;
                button1.Text = label2txt + " Details";
            }
            catch
            {
                button1.Text = "Nothing Selected";
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            _itemDnD = listView1.GetItemAt(e.X, e.Y);
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null)
            { return; }
            Cursor = Cursors.Hand;
            int lastItemBottom = Math.Min(e.Y, listView1.Items[listView1.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
            ListViewItem itemOver = listView1.GetItemAt(0, lastItemBottom);
            if (itemOver == null)
            { return; }
            Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
            if (e.Y < rc.Top + (rc.Height / 2))
            {
                listView1.LineBefore = itemOver.Index;
                listView1.LineAfter = -1;
            }
            else
            {
                listView1.LineBefore = -1;
                listView1.LineAfter = itemOver.Index;
            }
            listView1.Invalidate();
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null)
            { return; }
            try
            {
                int lastItemBottom = Math.Min(e.Y, listView1.Items[listView1.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
                ListViewItem itemOver = listView1.GetItemAt(0, lastItemBottom);
                if (itemOver == null)
                { return; }
                Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
                bool insertBefore;
                if (e.Y < rc.Top + (rc.Height / 2))
                {
                    insertBefore = true;
                }
                else
                {
                    insertBefore = false;
                }
                if (_itemDnD != itemOver)
                {
                    if (insertBefore)
                    {
                        listView1.Items.Remove(_itemDnD);
                        listView1.Items.Insert(itemOver.Index, _itemDnD);
                    }
                    else
                    {
                        listView1.Items.Remove(_itemDnD);
                        listView1.Items.Insert(itemOver.Index + 1, _itemDnD);
                    }
                }
                listView1.LineAfter =
                listView1.LineBefore = -1;
                listView1.Invalidate();
            }
            finally
            {
                _itemDnD = null;
                Cursor = Cursors.Default;
            }
        }
    }
}