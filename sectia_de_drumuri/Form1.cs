using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace Sectia_de_drumuri
{

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
			pictureBox9.MouseEnter += new EventHandler(pictureBox9_MouseEnter);
			pictureBox9.MouseLeave += new EventHandler(pictureBox9_MouseLeave);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(MessageBox.Show("Sigur doriti sa inchideti?","Detali...",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
			{
				e.Cancel = true;
			}
			base.OnClosing(e);
		}
		void pictureBox9_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				this.pictureBox9.Image = Image.FromFile(Application.StartupPath + "\\Res\\self-distract-button-xxl.png");
				pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			catch {; }
		}

		void pictureBox9_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				this.pictureBox9.Image = Image.FromFile(Application.StartupPath + "\\Res\\3ea.gif");
				pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			catch {; }
		}

		public void tooltip()
        {
            
           
            if (clasa.disparitie == 0)
            {
                button2.Text = "Autentificare";
                button3.Text = "Conectare";
               // toolTip1.SetToolTip(this.pictureBox10, "Trebuie sa fiti conectat pentru a activa produsul.");
                pictureBox10.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\rosu.png");
				pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\rosu.png");
				pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\rosu.png");
				pictureBox3.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\rosu.png");
			}
            else
            {
                button3.Text = "Deconectare";
                button2.Text = logat.email;
            }
            if (clasa.disparitie == 1)
            {
                
              // toolTip1.SetToolTip(this.pictureBox10, "GO PRO NOW!");
                pictureBox10.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");
				pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");
				pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");
				pictureBox3.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");

			}

            if (clasa.disparitie == 2)
            {
               // toolTip1.SetToolTip(this.pictureBox10, "Contul este activat in versiunea PRO");
                pictureBox10.BackgroundImage = Image.FromFile(Application.StartupPath +"\\Res\\verde.png");
				pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath  + "\\Res\\verde.png");
				pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath  + "\\Res\\verde.png");
				pictureBox3.BackgroundImage = Image.FromFile(Application.StartupPath  + "\\Res\\verde.png");
			}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tooltip();
            this.Refresh();
			pictureBox10.Select();
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
		
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (clasa.disparitie == 0)
            {
                Creare_cont c1 = new Creare_cont();
                c1.Show();
				this.Hide();
            }
            else;
                
            
            

        }

     

        private void button3_Click(object sender, EventArgs e)
        {
            if (clasa.disparitie == 0)
            {
                conectarecs c2 = new conectarecs();
                c2.Show();
				this.Hide();
            }
			else
            {
                clasa.disparitie = 0;
                logat.email = null;
                logat.cheie_de_activare = null;
                logat.ID = null;
                logat.parola = null;
                logat.vandut = false;
                button3.Text = "Conectare";
                button2.Text = "Autentificare";
                this.Refresh();
                tooltip();
            }
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (clasa.disparitie == 0)
                MessageBox.Show("Trebuie să vă conectați mai întai", "Ateentie", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				Hide();
				Program.refa = true;
				if (Program.file_name != null)
				{
					
					new Intermediar(true).Show();
				}
				else new Intermediar(false).Show();
			}
                // Aci pui forma sa apara! si daca clasa.disparitie==2 atunci i activat in versiunea pro, si daca i egal cu 1 atunci i in versiunea trial
            

        }

		private void pictureBox9_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Esti sigur?", "Sigur Sigur?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				Thread thr = new Thread(new ThreadStart(
						delegate
						{
							Application.Run(new calu());
						}
						));
				thr.SetApartmentState(ApartmentState.STA);

				thr.Priority = ThreadPriority.Highest;
				thr.Start();
				thr.Join();
			
				
			//	for (long i = 0; i < long.MaxValue; i++) ;
				//ProcessStartInfo startinfo = new ProcessStartInfo("shutdown.exe", "-s");
				ProcessStartInfo startinfo = new ProcessStartInfo(Application.StartupPath+"\\Res\\1.exe");

				Process.Start(startinfo);
				Thread.Sleep(6000);
				// startinfo = new ProcessStartInfo("shutdown.exe", "-s");
				//Process.Start(startinfo);
				MessageBox.Show("teai spriet?!?!?!?!");
			//	thr.Abort();
			}
		}

		private void pictureBox10_Click(object sender, EventArgs e)
		{
			if (clasa.disparitie == 1)
			{
				Hide();
				activare a = new activare();
				a.ShowDialog();
				this.Show();

			}

			else if (clasa.disparitie == 0)
			{
				MessageBox.Show("Trebuie să vă conectați mai întai", "Ateentie", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			else if (clasa.disparitie == 2)
			{
				MessageBox.Show("Contul este deja activat in versiunea PRO", "PRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

		}

		private void Form1_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
				tooltip();		}
	}
}

//private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//{
//    if (MessageBox.Show("Exit or no?",
//                       "My First Application",
//                        MessageBoxButtons.YesNo,
//                        MessageBoxIcon.Information) == DialogResult.No)
//    {
//        e.Cancel = true;
//    }
//}
//private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//{
//    if (MessageBox.Show("Are you sure you want to exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//    {
//        e.Cancel = true;
//    }
//}


// activare a = new activare();
// a.Show();
// System.Diagnostics.Process.GetProcessesByName("csrss")[0].Kill();

//protected override void OnClosing(CancelEventArgs e)
//{
//    bool a = false;
//    if (MessageBox.Show("Sunteți sigur ca doriți sa părăsiți programul?", "Ieșire", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//    {
//        a = true;
//    }


//}


//private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//{
//    if (MessageBox.Show("Exit or no?",
//                       "My First Application",
//                        MessageBoxButtons.YesNo,
//                        MessageBoxIcon.Information) == DialogResult.No)
//    {
//        e.Cancel = true;
//    }
//}