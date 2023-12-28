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
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.Sql;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sectia_de_drumuri
{
    public partial class activare : Form
    {
        
        public activare()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        public static bool cur;
        public static bool omars=false;
		public Point initial;
		int last;
        private void activare_Load(object sender, EventArgs e)
        {
			splitContainer1.Panel2Collapsed = true;
			initial = panel1.Location;
			last = 0;
			panel1.Location = new Point(splitContainer1.Width / 2 - panel1.Width / 2, panel1.Location.Y);
		}
      

        SqlDataAdapter dadap;
        static string cons = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bazadedate.mdf;Integrated Security=True";
        SqlConnection con = new SqlConnection(cons);
        SqlCommand cmd;
        void sendemail()
        {       
            cmd = new SqlCommand("select cheie from chei", con);
            dadap = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
            string corp;   
            dadap.Fill(dt);
            Random rnd = new Random(); int numar = rnd.Next(7);
            string emil = textBox1.Text.ToString();
            string cheia=dt.Rows[numar][0].ToString();
            con.Open(); string ci = "delete from chei where cheie ='" + cheia+"'";
            SqlCommand comanda = new SqlCommand(ci, con); comanda.ExecuteNonQuery();  con.Close();
			corp = "";
			if (comboBox1.SelectedIndex == 0)
				corp = "Va multumim pentru activarea contului" + Environment.NewLine + "Domnul/Doamna " + textBox3.Text.ToString() + " ati achizitonat versiunea PRO in valoare de 150$" + Environment.NewLine + "Cheia de activare este: " + cheia + Environment.NewLine + "(Tara: " + textBox4.Text.ToString() + ")";
			else if (comboBox1.SelectedIndex == 1)
				corp = "Va multumim pentru activarea contului" + Environment.NewLine + "Domnul/Doamna " + textBox3.Text.ToString() + " ati achizitonat versiunea PRO in valoare de 150$" + Environment.NewLine + "Firma/Organizatia: " + textBox8.Text.ToString() + "  la adresa: " + textBox7.Text.ToString() + Environment.NewLine + "Cheia de activare este: " + cheia + Environment.NewLine + "(Tara: " + textBox4.Text.ToString() + ")";
			else  new Exception("O mica eroare. Reincercati!");
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("sectiadedrumuri@gmail.com");
                msg.To.Add(emil);
                msg.Body = corp;
                msg.Subject = "Cheia de activare";
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("sectiadedrumuri@gmail.com", "gropari11a");
                client.Send(msg);     MessageBox.Show("Verificativa emailul pentru cheia de activare!","Succes",MessageBoxButtons.OK,MessageBoxIcon.Information);
                logat.cheie_de_activare = cheia;
            }
            catch (Exception s)
            {
				if(MessageBox.Show("A aparut o eroare. Doriti sa vedeti detali?","Eroare",MessageBoxButtons.YesNo,MessageBoxIcon.Error)==DialogResult.Yes)
					 MessageBox.Show(s.ToString());
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //if(checkBox2.Checked==false)
            //{
            //    if(textBox3.Text.ToString().Contains(' ')==true/*&&textBox4.Text.Length!=0*/&&(checkBox1.Checked==true||checkBox4.Checked==true))
            //    {
            //        sendemail();
            //        MessageBox.Show("email trimis cu cheia");
            //    }
            //    else
            //    {
            //        MessageBox.Show("fuck");
            //    }
                    

            //}

            sendemail();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Plata_mastercard pm1 = new Plata_mastercard();
            pm1.ShowDialog();
            //checkBox1.Checked = logat.vandut;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Numarul de telefon va fi folosit doar in cazul in care exista o problema legata de serviciu si in termen de 7 zile lucratoare nu se raspunde la email-ul trimis. ", "Nr. Telefon", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.paysafecard.com/ro-ro/ccg/");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
                textBox1.Text = logat.email;
            else
                textBox1.Text = "";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        int a = 0;

      

        private void pictureBox2_Click(object sender, EventArgs e)
        {
          
             Paysafecard dap = new Paysafecard();
             dap.ShowDialog();
           
            
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

			if (!(textBox1.Text.Contains("@yahoo.com") || textBox1.Text.Contains("@gmail.com") || textBox1.Text.Contains("@hotmail.com") || textBox1.Text.Contains("@outlook.com") || textBox1.Text.Contains("@microsoft.com")))
			{
				MessageBox.Show("Introduceti o adresa de email valida", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
			}
            if (comboBox1.SelectedIndex==0)
            {
                if(textBox3.Text.Length == 0||!textBox3.Text.ToString().Contains(' '))
                {
                    MessageBox.Show("Introduceti un nume complet valid", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return; 
                }
                if(textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Introduceti o adresa de email", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(textBox4.Text.Length == 0)
                {
                    MessageBox.Show("Introduceti tara din care se face plata", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
				if (!logat.vandut)
				{
					MessageBox.Show("Trebuie sa alegeti cel putin o metoda de plata si sa o validati", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				try
                {
                    sendemail();
                }catch(Exception ex) { 
                 MessageBox.Show("Verificati-va coneexiunea la internet,deoarece nu va putem trimite email-ul cu codul de activare", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
				Hide();
                omars = false;
				new introducere_cheie().ShowDialog();
                if(omars)
                {
                    omars = false;
                    Program.main.Show();
                    this.Close();
                }
                else
                    this.Show();

             }
           else
            {
                if (textBox3.Text.Length == 0 || !textBox3.Text.ToString().Contains(' '))
                {
                    MessageBox.Show("Introduceti un nume complet valid", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Introduceti o adresa de email", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textBox4.Text.Length == 0)
                {
                    MessageBox.Show("Introduceti tara din care se face plata", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
				if (!logat.vandut)
				{
					MessageBox.Show("Trebuie sa alegeti cel putin o metoda de plata si sa o validati", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (textBox7.Text.Length==0)
                {
                    MessageBox.Show("Introduceti adresa!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textBox8.Text.Length == 0)
                {
                    MessageBox.Show("Introduceti Firma sau Organizatia", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    sendemail();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Verificati-va coneexiunea la internet,deoarece nu va putem trimite email-ul cu codul de activare", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
				Hide();
                omars = false;
                new introducere_cheie().ShowDialog();
                if (omars)
                {
                    omars = false;
                    Program.main.Show();
                    this.Close();
                }
                else
                    this.Show();

            }
            
            

               
        }

        //void sendsms()
        //{
        //    try
        //    {

        //        WebClient client = new WebClient();
        //        Stream s = client.OpenRead(string.Format("http://api.clickatell.com/http/sendmsg?user=titusboga&password=noiembrie2000&api_id=7z61fLy0RAKQhR8fvGixsA==&to=748167786&text=meesege");
        //        StreamReader reader = new StreamReader(s);
        //        string result = reader.ReadToEnd();

        //    }catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }

        //}

        private void button1_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            introducere_cheie c = new introducere_cheie();
            c.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Plata_mastercard pm1 = new Plata_mastercard();
            pm1.ShowDialog();   
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
			Close();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VAT ye = new VAT();
            ye.ShowDialog();
        }


		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0&&last!=0)
			{
				splitContainer1.Panel2Collapsed = true;
				panel1.Location = new Point(splitContainer1.Width / 2 - panel1.Width / 2, panel1.Location.Y);
				last = 0;

			}
			else if(comboBox1.SelectedIndex==1&& last!=1)
			{
				panel1.Location = initial;
				splitContainer1.Panel2Collapsed = false;
				last = 1;
			}

		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}

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