using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Sectia_de_drumuri
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 
		public static string file_name = null;															// @"C:\Users\root\Desktop\Drumuri0.1r\test.drumuinator";
		public const string exte = ".drumuinator";
		public static bool refa = true;
		public static MainForm fmc;
		public static Form1 main;
		public static load loader;

		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try
			{
				if (args != null && args.Length > 0)
				{
					file_name = args[0];
					if (File.Exists(file_name))
					{
						if (Path.GetExtension(file_name) != exte) file_name = null;
					}
				}
				main = new Form1();
				Application.Run(main);
				//Application.Run(new Form1());

			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.ToString(), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


	}
}
