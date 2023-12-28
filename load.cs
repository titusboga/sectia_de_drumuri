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
	public partial class load : Form
	{
		// Volatile is used as hint to the compiler that this data
		// member will be accessed by multiple threads.
		private volatile bool _shouldDo=false;
		public load()
		{
			InitializeComponent();
		}

		private void load_Load(object sender, EventArgs e)
		{
			CenterToScreen();
		}
		public void KILL()
		{
			Close();
		}
		public void killYourself()
		{
			_shouldDo = true;
		}
		public void DoWork()
		{
			while (!_shouldDo)
			{
				;//nu o face
			}
			KILL();
		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
