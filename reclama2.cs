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
	public partial class reclama2 : Form
	{
		public reclama2()
		{
			InitializeComponent();
			Random rnd = new Random();
			Location = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - Width), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - Height));
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			
			ProcessStartInfo startinfo = new ProcessStartInfo("https://ro.metin2.gameforge.com");
			Process.Start(startinfo);
		}
	}
}
