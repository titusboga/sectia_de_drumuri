using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sectia_de_drumuri
{
    public partial class Plata_mastercard : Form
    {
        public Plata_mastercard()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void Plata_mastercard_Load(object sender, EventArgs e)
        {
			pictureBox1.Select();
			comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!(comboBox1.SelectedIndex > -1))
            {
                MessageBox.Show("Selectati moneda in care este facuta plata!", "Eroare",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(textBox2.Text.ToString().Length!=12)
            {
                MessageBox.Show("Numarul cardului este invalid!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(textBox5.Text.Length>4|| textBox5.Text.Length<2)
            {
                MessageBox.Show("Codul CVV este invalid!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           if(!textBox6.Text.Contains(" "))
            {
                MessageBox.Show("Introduceti un nume valid!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

		   if(DateTime.Now.Date>=dateTimePicker1.Value.Date)
			{
				MessageBox.Show("Introduceti un o data valida!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
            this.Close();
            logat.vandut = true;

        }

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			logat.vandut = false;
			Close();
		}
	}
}
