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
	public partial class Reclame : Form
	{
		public Reclame()
		{
			InitializeComponent();
		}

		private void Reclame_Load(object sender, EventArgs e)
		{
			Random rnd = new Random();
			Location = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - Width ), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - Height));
			pictureBox2.Parent = pictureBox1;
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startinfo = new ProcessStartInfo("https://en.grepolis.com");
			Process.Start(startinfo);
		}
	}
}
