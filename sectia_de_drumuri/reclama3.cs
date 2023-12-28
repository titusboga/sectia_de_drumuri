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
	public partial class reclama3 : Form
	{
		public reclama3()
		{
			InitializeComponent();

			Random rnd = new Random();
			Location = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - Width), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - Height));
			//	Location = new Point(Screen.PrimaryScreen.Bounds.Size.Width - Width, Screen.PrimaryScreen.Bounds.Size.Height - Height);
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startinfo = new ProcessStartInfo("https://signup.eune.leagueoflegends.com/ro/signup/index");
			Process.Start(startinfo);
		}
	}
}
