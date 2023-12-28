using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Sectia_de_drumuri
{
	public partial class Intermediar : Form
	{
		public Intermediar()
		{
			InitializeComponent();

			

		}
		bool iicuarg;
		bool apasat;
		public  Intermediar(bool cuarg)
		{
			InitializeComponent();
			splitContainer1.Panel2Collapsed = true;
			Height = splitContainer1.SplitterDistance;
			iicuarg = cuarg;
			apasat = false;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			Program.file_name = null;

		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void Intermediar_Load(object sender, EventArgs e)
		{
			CenterToScreen();

			if(iicuarg)
			{

				mora();
			}
		}
		void mora()
		{
			try
			{
				Hide();
				while(Program.refa)
				{
					if (apasat) Program.fmc = new MainForm(new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value));
					else Program.fmc = new MainForm();
					Program.fmc.ShowDialog();
					Program.fmc.Dispose();
					Program.fmc= null;
					GC.Collect();
					if(Program.refa&&Program.file_name==null)
					{
						splitContainer1.Panel1Collapsed = false;
						splitContainer1.Panel2Collapsed = true;
						Show();
						return;

					}
					

				}
			}
			catch (Exception ee)
			{
				if(MessageBox.Show("A aparut o eroare. Doriti sa vedeti detali?","Eroare",MessageBoxButtons.YesNo,MessageBoxIcon.Error)==DialogResult.Yes)
					MessageBox.Show(ee.ToString(), "Eroare", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
				Close();
				
			}
			Close();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			apasat = false;
			splitContainer1.Panel1Collapsed = false;
			splitContainer1.Panel2Collapsed = true;
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
			
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			apasat = true;
			Program.file_name = null;
			splitContainer1.Panel2Collapsed = false;
			splitContainer1.Panel1Collapsed = true;


		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			Program.main.Show();
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Drumuinator File(*.drumuinator)| *.drumuinator";
			ofd.RestoreDirectory = true;
			ofd.Title = "Incarcare sesiune :";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				if (Path.GetExtension(ofd.FileName) == Program.exte)
				{
					//	MessageBox.Show(ofd.FileName);
					Program.file_name = ofd.FileName;
					Program.refa = true;
					mora();
					//   Application.Run(new Form1());
					// Environment.Exit(0);
				}

			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			mora();
		}

		private void groupBox1_Enter(object sender, EventArgs e)
		{

		}

		private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
		{

		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
