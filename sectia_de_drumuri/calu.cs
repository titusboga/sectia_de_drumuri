using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sectia_de_drumuri
{
	public partial class calu : Form
	{
		public calu()
		{
			InitializeComponent();
			CenterToScreen();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}

		private void calu_Load(object sender, EventArgs e)
		{

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Invoke(new Action(Close));
		}
	}
}
