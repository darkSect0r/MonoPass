using System;
using System.Timers;
using System.Windows.Forms;

namespace MonoPass
{
    public partial class Form2 : Form
    {

        private static System.Timers.Timer aTimer;
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button3.Text = "Close";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cpyTxt = maskedTextBox1.Text;
            Clipboard.SetText(cpyTxt);
            SetTimer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cpyTxt2 = maskedTextBox2.Text;
            Clipboard.SetText(cpyTxt2);
            SetTimer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetTimer()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            aTimer = new System.Timers.Timer(1); 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            LblS(); 
        }
        
        private void LblS()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(LblS));
            }    
            else  
            {
                if (label4.Visible)
                {
                    label4.Visible = false;
                    aTimer.Enabled = false;
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
                else
                {
                    label4.Visible = true;
                    aTimer.Interval = 3000;
                }
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            button3.Text = "Save and Close";
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            button3.Text = "Save and Close";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
