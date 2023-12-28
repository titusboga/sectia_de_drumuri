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
	public partial class introducere_cheie : Form
	{

		public introducere_cheie()
		{
			InitializeComponent();
			this.CenterToScreen();
		}
		bool activat;
		SqlDataAdapter dadap;
		static string cons = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bazadedate.mdf;Integrated Security=True";
		SqlConnection con = new SqlConnection(cons);
		SqlCommand cmd;
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text.ToString() == logat.cheie_de_activare)
			{

				string comanda = "update conturi set cheiaa=(@name) WHERE email='" + logat.email + "'";
				dadap = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				cmd = new SqlCommand(comanda, con);
				con.Open();
				cmd.Parameters.AddWithValue("@name", textBox1.Text);

				cmd.ExecuteNonQuery();
				con.Close(); MessageBox.Show("Activare reusita!", "Va multumim.", MessageBoxButtons.OK, MessageBoxIcon.Information);
				clasa.disparitie = 2;
				activat = true;
				this.Close();
				Program.main.tooltip();
			}
			else
				MessageBox.Show("Cheia de activare este incorecta!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void introducere_cheie_Load(object sender, EventArgs e)
		{
			groupBox1.Select();
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
            ///ceva PUTE
			//activare a = new activare();
			//a.ShowDialog();
			activat = false;
            activare.omars = false;
			this.Close();
		}
		protected override void OnClosing(CancelEventArgs e)
		{
            if (activat)
            {
                activare.omars = true;
                //Program.main.Show();
                base.OnClosing(e);
            }

		}
	}
}


//protected override void OnClosing(CancelEventArgs e)
//{
//    if (MessageBox.Show("Sunteți sigur ca doriți sa părăsiți programul?", "Ieșire", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//    {
//        base.OnClosing(e);
//    }
//    e.Cancel = true;



//}
