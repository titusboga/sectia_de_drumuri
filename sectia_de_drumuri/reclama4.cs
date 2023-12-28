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
	public partial class reclama4 : Form
	{
		public reclama4()
		{
			InitializeComponent();
			Random rnd = new Random();
		//	MessageBox.Show(Screen.PrimaryScreen.Bounds.Width + " " + Screen.PrimaryScreen.Bounds.Height);
			Location = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - Width), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - Height));
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			
			ProcessStartInfo startinfo = new ProcessStartInfo("http://www.codeblocks.org");
			Process.Start(startinfo);
		}

		private void reclama4_Load(object sender, EventArgs e)
		{

		}
	}
}
