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
using System.Data.Sql;

namespace Sectia_de_drumuri
{


    public partial class conectarecs : Form
    {
       
        
        
        public conectarecs()
        {
            InitializeComponent();
            this.CenterToScreen();

        }

        public static string cons = @"Data Source=(localdb)\mssqllocaldb;AttachDbFilename=|DataDirectory|\bazadedate.mdf;Integrated Security=True";
        SqlConnection con = new SqlConnection(cons);
        SqlCommand cmd;
        SqlDataAdapter dadap;

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			Program.main.Show();
		}
		private void conectarecs_Load(object sender, EventArgs e)
        {
            customGrpBox1.Select();
        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string password = textBox2.Text;
            con.Open();
            cmd = new SqlCommand("select username,email,password,cheiaa from conturi",con);
            dadap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dadap.Fill(dt);         
            for(int i=0;i<dt.Rows.Count;i++)
                if (user == dt.Rows[i][0].ToString() || user == dt.Rows[i][1].ToString())
                {
                    if (password == dt.Rows[i][2].ToString() && dt.Rows[i][3].ToString() != "")
                    {
                        logat.ID = dt.Rows[i][0].ToString();
                        logat.email = dt.Rows[i][1].ToString();
                        logat.parola = dt.Rows[i][2].ToString();
                        clasa.disparitie = 2;
                        break;
                    }
                    else if (password == dt.Rows[i][2].ToString())
                       {
                              clasa.disparitie = 1;
                              logat.ID = dt.Rows[i][0].ToString();
                              logat.email = dt.Rows[i][1].ToString();
                              logat.parola = dt.Rows[i][2].ToString();
                             break;
                        }
                }
            con.Close();         
            if (clasa.disparitie == 0)
				MessageBox.Show("Informatile introduse nu corespund!","Sectia de drumuri",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            else
            {
                MessageBox.Show("Bine ai venit!","Sectia de drumuri",MessageBoxButtons.OK,MessageBoxIcon.Information);
				Close();
            }                  
        }

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            new AmUitatParola().ShowDialog();
            Show();
        }
    }
}
