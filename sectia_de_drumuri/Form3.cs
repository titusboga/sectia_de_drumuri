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
	public partial class Form3 : Form
	{
		public Form3()
		{
			InitializeComponent();
		}
		int poz;

		public Form3(int poz)
		{
			InitializeComponent();
			this.poz = poz;
			Text = "Setari " + MainForm.cars[poz].Name;
		}
		private void Form3_Load(object sender, EventArgs e)
		{
			CenterToScreen();
		}

		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			MainForm.cars[poz].consum = numericUpDown2.Value;
			Close();
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{

		}
	}
}
