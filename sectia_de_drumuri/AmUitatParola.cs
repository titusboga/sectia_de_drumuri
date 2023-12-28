using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
namespace Sectia_de_drumuri
{
    public partial class AmUitatParola : Form
    {
        public AmUitatParola()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(conectarecs.cons);
        SqlCommand cmd;

        private void AmUitatParola_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        string emil = null;
        bool conectare = false;
        string corp = null;
        Random rnd = new Random();

        int get_random(int a)
        {
            switch (a)
            {
                case 1: return rnd.Next(48, 58);
                case 2: return rnd.Next(65, 91);
            }
            return 0;
        }
        string cod()
        {
            string ya = null;
            for (int i = 0; i < 6; ++i)
            {
                int aux = rnd.Next(1, 3);
                ya += Char.ConvertFromUtf32(get_random(aux));
            }
            return ya;
        }
        void sendemail()
        {

            emil = textBox1.Text;
            corp = cod();
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("sectiadedrumuri@gmail.com");
                msg.To.Add(emil);
                msg.Body = "Cheia de resetare a parolei pentru contul dumneavoastra este:"+Environment.NewLine+corp + Environment.NewLine+Environment.NewLine
                    +"Daca nu ati incercat sa va recuperati parola, s-ar putea ca altcineva sa incerce sa acceseze contul dumneavoastra.";
                msg.Subject = "Cheia de resetare";
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("sectiadedrumuri@gmail.com", "gropari11a");
                client.Send(msg); MessageBox.Show("Verificativa emailul pentru cod!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conectare = true;

                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel1Collapsed = true;
                splitContainer2.Panel1Collapsed = false;
                splitContainer2.Panel2Collapsed = true;

            }
            catch (Exception s)
            {
                if (MessageBox.Show("A aparut o eroare. Doriti sa vedeti detali?", "Eroare", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MessageBox.Show(s.Message);
            }
        }

        bool gasire()
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM conturi WHERE email=@email", con);
                cmd.Parameters.AddWithValue("@email", textBox1.Text);
                con.Open();
                var true_false = cmd.ExecuteScalar();
                con.Close();
                if (true_false != null)
                    return true;
                return false;
            }
            catch (Exception ee)
            {
                if (MessageBox.Show("A aparut o eroare. Doriti sa vedeti detali?", "Eroare", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MessageBox.Show(ee.Message);
                con.Close();
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = false;    
            splitContainer1.Panel1Collapsed = true;
            splitContainer2.Panel1Collapsed = false;
            splitContainer2.Panel2Collapsed = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text == textBox3.Text && textBox3.Text != null)
                {

                    cmd = new SqlCommand("UPDATE conturi SET password=@pass WHERE email=@email", con);
                    cmd.Parameters.AddWithValue("@pass", textBox3.Text);
                    cmd.Parameters.AddWithValue("@email", emil);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Parola o fost schimbata cu succes!","Totul este bine acum! (cred)",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.Close();
                }
                else
                  if (textBox1.Text != null)
                    MessageBox.Show("Parolele nu potrivesc!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ee)
            {
                if (MessageBox.Show("A aparut o eroare. Doriti sa vedeti detali?", "Eroare", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MessageBox.Show(ee.Message);
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == corp)
            {
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel1Collapsed = true;
                splitContainer2.Panel2Collapsed = false;
                splitContainer2.Panel1Collapsed = true;
                textBox2.Text = "";
            }
            else MessageBox.Show("Cod incorect!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {  
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = true;   
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (gasire())
                sendemail();
            else
                MessageBox.Show("Nu exista un cont cu acest email!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class CustomGrpBox : GroupBox
    {
        private string _Text = "";
        public CustomGrpBox()
        {
            //set the base text to empty 
            //base class will draw empty string
            //in such way we see only text what we draw
            base.Text = "";
        }
        //create a new property a
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("GroupBoxText")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Text
        {
            get
            {

                return _Text;
            }
            set
            {

                _Text = value;
                this.Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            //first let the base class to draw the control 
            base.OnPaint(e);
            //create a brush with fore color
            SolidBrush colorBrush = new SolidBrush(this.ForeColor);
            //create a brush with back color
            var backColor = new SolidBrush(this.BackColor);
            //measure the text size
            var size = TextRenderer.MeasureText(this.Text, this.Font);
            // evaluate the postiong of text from left;
            int left = (this.Width - size.Width) / 2;
            //draw a fill rectangle in order to remove the border
            e.Graphics.FillRectangle(backColor, new Rectangle(left, 0, size.Width, size.Height));
            //draw the text Now
            e.Graphics.DrawString(this.Text, this.Font, colorBrush, new PointF(left, 0));

        }
    }
}
