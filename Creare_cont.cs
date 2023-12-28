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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sectia_de_drumuri 
{
    public partial class Creare_cont : Form
    {
        static public bool mamaie;
        public Creare_cont()
        {
            InitializeComponent();
            this.CenterToScreen();
            Location = new Point(Location.X + 350,Location.Y); 
            
        }
       
        static string cons = @"Data Source=(localdb)\mssqllocaldb;AttachDbFilename=|DataDirectory|\bazadedate.mdf;Integrated Security=True";
        SqlConnection con = new SqlConnection(cons);
        SqlCommand cmd;

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			//de prin form
			Program.main.Show();
		}
		private void Creare_cont_Load(object sender, EventArgs e)
        {
			groupBox1.Select();
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 3000;
            toolTip1.InitialDelay = 700;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
			splitContainer1.Panel2Collapsed = true;
			Width = splitContainer1.SplitterDistance;
        }
        int a = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string nume = textBox1.Text.ToString();
            string parola = textBox2.Text.ToString();
			string email = textBox4.Text.ToString();
           
            if (email.Contains("@yahoo.com")||email.Contains("@gmail.com")
				|| email.Contains("@hotmail.com")|| email.Contains("@outlook.com") 
				|| email.Contains("@microsoft.com") )
            {
                cmd = new SqlCommand("insert into conturi(username,password,email) " +
					"values(@name,@password,@email)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", textBox2.Text);
                cmd.Parameters.AddWithValue("@email", textBox4.Text);
                cmd.ExecuteNonQuery();
                con.Close();
				MessageBox.Show("Cont creat cu succes!", "Sectia de drumuri",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
            }
            else
            {
				MessageBox.Show("Contul exista deja!", "Sectia de drumuri",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
               
            }
        }
  // cmd =new SqlCommand("insert into conturi(username, password, email) values("+textBox1.Text+", "+textBox2.Text+", "+textBox4.Text+"");
            
          //  con.Open();
            
          //  // cmd = new SqlCommand("select * from conturi");
          ////  con.Open();
          //  DataTable dt = new DataTable();
            
          //  dadap = new SqlDataAdapter("select * from conturi",con);
          //  dadap.Fill(dt);
          //  dataGridView1.DataSource = dt;
          //  con.Close();
      
        string creatura;
        private void button3_Click(object sender, EventArgs e)
        {
			if(textBox5.Text.ToString().Length!=12)
                MessageBox.Show("Numarul introdus nu este valid.",
					"Eroare",MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
			{
                string telefon = textBox5.Text.ToString();
				for(int i=1;i<telefon.Length;i++)
				{
					if(telefon[i]<'0'||telefon[i]>'9')
					{
						MessageBox.Show("Numarul introdus nu este valid.",
							"Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				try
				{
					Random rnd = new Random();
					int ni = rnd.Next(1000, 9999);
					string nistring = ni.ToString();
					creatura = nistring;
					var accountSid = "ACf95911f1b5a95c7328c673f91e4c9cbb";
					var authToken = "be51502feefac36b10f178716afaae73";

					TwilioClient.Init(accountSid, authToken);

					var message = MessageResource.Create(
						to: new PhoneNumber(telefon),
						from: new PhoneNumber("+17342924788"),
						body: "Buna ziua. Codul tau de activare este: " + nistring);
					splitContainer2.Panel2Collapsed = false;splitContainer2.Panel1Collapsed = true;
				}
				catch(Exception ee)
				{
					if(MessageBox.Show("A aparut o eroare. Doriti sa vedeti detalii?",
						"Eroare", MessageBoxButtons.YesNo, MessageBoxIcon.Error)==DialogResult.Yes)
						MessageBox.Show(ee.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == creatura)
                button1.Enabled = true;
			else MessageBox.Show("Codul de verificare introdus nu este valid", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);


		}

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        bool verificare(string nume,string email)
        {
            cmd = new SqlCommand("select username,email from conturi", con) ;
            SqlDataAdapter dadap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dadap.Fill(dt);
            
            for(int i=0;i<dt.Rows.Count;i++)
                if (dt.Rows[i][0].ToString() == nume || dt.Rows[i][1].ToString() == email)
                    return false;
            return true;
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nume = textBox1.Text;
            string parola = textBox2.Text;
            string email = textBox4.Text;

          

                if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Parola nu este confirmata!");
                return;
            }
           
            if(textBox1.Text.ToString().Length == 0)
            {
                MessageBox.Show("Trebuie ales un username obligatoriu!");
                return;
            }

           if (textBox4.Text.ToString() == "example@yahoo.com")
            {
                MessageBox.Show("Va rugam introduceti o adresa de email valida!");
                return;
            }

            if (email.Contains("@yahoo.com") || email.Contains("@gmail.com") || email.Contains("@hotmail.com") || email.Contains("@outlook.com") || email.Contains("@microsoft.com"))
            {


                if (verificare(nume, email)==true)//urmatoare pag
                {
					splitContainer2.Panel2Collapsed = true;
					splitContainer1.Panel2Collapsed = false;
					splitContainer1.Panel1Collapsed = true;
					
             
                }
                else
					MessageBox.Show("Contul exista deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);


			}
			else
            {
                MessageBox.Show("Adresa de email invalida!");
            }
          

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            string nume = textBox1.Text.ToString();
            string parola = textBox2.Text.ToString();
            string email = textBox4.Text.ToString();

            if (verificare(nume, email) == true)
            {
                if (email.Contains("@yahoo.com") || email.Contains("@gmail.com") || email.Contains("@hotmail.com") || email.Contains("@outlook.com") || email.Contains("@microsoft.com"))
                {
                    cmd = new SqlCommand("insert into conturi(username,password,email) values(@name,@password,@email)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@password", textBox2.Text);
                    cmd.Parameters.AddWithValue("@email", textBox4.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Cont creat cu succes!","Sectia de drumuri",MessageBoxButtons.OK,MessageBoxIcon.Information);
					Close();
                }
                else
                {
                    MessageBox.Show("Ceva eroare");
                    return;

                }
            }
            else
				MessageBox.Show("Cont creat cu succes!", "Sectia de drumuri", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{

		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			splitContainer1.Panel1Collapsed = false;
			splitContainer2.Panel2Collapsed = true;
			textBox5.Text = "+40";

		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			splitContainer2.Panel1Collapsed = false;
			splitContainer2.Panel2Collapsed = true;
			textBox6.Text = "";
			button1.Enabled = false;
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void groupBox5_Enter(object sender, EventArgs e)
		{

		}
	}
}
