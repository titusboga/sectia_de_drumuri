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
    public partial class VAT : Form
    {
        public VAT()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ec.europa.eu/taxation_customs/vies/vieshome.do?selectedLanguage=EN");   
        }

        private void VAT_Load(object sender, EventArgs e)
        {

        }
    }
}
