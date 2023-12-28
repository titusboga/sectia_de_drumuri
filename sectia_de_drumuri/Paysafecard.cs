using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sectia_de_drumuri
{
    public partial class Paysafecard : Form
    {
        public Paysafecard()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox12.TextLength == 12 && checkBox6.Checked == true)
            {
                logat.vandut = true;
                
                this.Close();
            }
            else
            {
                if(checkBox6.Checked==false)
                {
                    MessageBox.Show("Pentru a plati cu paysafecard este necesar sa acceptati Termenii", "Termeni", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Codul introdus nu este un cod valid paysafecard", "Cod invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Paysafecard_Load(object sender, EventArgs e)
        {
			pictureBox2.Select();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.paysafecard.com/ro-ro/ccg/");
        }

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			logat.vandut = false;
			Close();
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
		}
	}
}
