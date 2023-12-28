using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Collections;
using System.Threading;

namespace Sectia_de_drumuri
{
	public partial class MainForm : Form
	{
        //algoritmul care se foloseste pentru gasirea celui mai scurt drum
        public static Cautare algoritm = Cautare.Dijkastra;

		#region error list
        #endregion

        /// <summary>
        /// Method to rotate an Image object. The result can be one of three cases:
        /// - upsizeOk = true: output image will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false & clipOk = true: output same size as input, clipping occurs
        /// - upsizeOk = false & clipOk = false: output same size as input, image reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated image. If color = transparent the output image will be 32-bit, 
        /// otherwise the output image will be 24-bit.
        /// 
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputImage">input Image object, is not modified</param>
        /// <param name="angleDegrees">angle of rotation, in degrees</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input image</returns>
        public static Bitmap RotateImage(Image inputImage, float angleDegrees, bool upsizeOk,
										 bool clipOk, Color backgroundColor)
		{
			// Test for zero rotation and return a clone of the input image
			if (angleDegrees == 0f)
				return (Bitmap)inputImage.Clone();

			// Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
			int oldWidth = inputImage.Width;
			int oldHeight = inputImage.Height;
			int newWidth = oldWidth;
			int newHeight = oldHeight;
			float scaleFactor = 1f;

			// If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
			if (upsizeOk || !clipOk)
			{
				double angleRadians = angleDegrees * Math.PI / 180d;

				double cos = Math.Abs(Math.Cos(angleRadians));
				double sin = Math.Abs(Math.Sin(angleRadians));
				newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
				newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
			}

			// If upsizing not wanted and clipping not OK need a scaling factor
			if (!upsizeOk && !clipOk)
			{
				scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
				newWidth = oldWidth;
				newHeight = oldHeight;
			}

			// Create the new bitmap object. If background color is transparent it must be 32-bit, 
			//  otherwise 24-bit is good enough.
			Bitmap newBitmap = new Bitmap(newWidth, newHeight, backgroundColor == Color.Transparent ?
											 PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
			newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

			// Create the Graphics object that does the work
			using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
			{
				graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
				graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

				// Fill in the specified background color if necessary
				if (backgroundColor != Color.Transparent)
					graphicsObject.Clear(backgroundColor);

				// Set up the built-in transformation matrix to do the rotation and maybe scaling
				graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

				if (scaleFactor != 1f)
					graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

				graphicsObject.RotateTransform(angleDegrees);
				graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

				// Draw the result 
				graphicsObject.DrawImage(inputImage, 0, 0);
			}

			return newBitmap;
		}

		public static Color InvertColour(Color ColourToInvert)
		{
			return Color.FromArgb((byte)~ColourToInvert.R, (byte)~ColourToInvert.G, (byte)~ColourToInvert.B);
		}
		const int PEN_SIZE = 5;

		public static Font nota_fnt = new Font("Lucida console", 16);

		#region culorinormale
		public static Color culoare_sageata = Color.Black;
		public static Color culoare_drum_simplu = Color.AliceBlue;
		public static Color culoare_drum_dublu = Color.FromArgb(0, 37, 70);
		public static Color culoare_intersectie = Color.DarkGray;
		public static Color culoare_intersectie_inactiva = Color.Black;
		public static Color culoare_background = Color.FromArgb(63, 63, 63);

		public static Color culore_nota_back = Color.FromArgb(202, 230, 255);
		public static Color culore_nota_text = Color.Black;

		#endregion
		#region culoriinversate
		public static Color inv_culoare_sageata;
		public static Color inv_culoare_drum_simplu;
		public static Color inv_culoare_drum_dublu;
		public static Color inv_culoare_intersectie;
		#endregion
		#region draw
		public static Image draw_orizonatal_dreapta(Color col, Size siz)
		{
			siz = new Size(siz.Width, siz.Height);
			Bitmap btm = new Bitmap(siz.Width, siz.Height);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;
			g.DrawLine(pen, new Point(siz.Width / 4, siz.Height / 2), new Point(siz.Width - siz.Width / 4, siz.Height / 2));
			g.Dispose();
			return (Image)btm;
		}
		public static Image draw_orizonatal_stanga(Color col, Size siz)
		{
			Bitmap btm = new Bitmap(siz.Width, siz.Height);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;
			g.DrawLine(pen, new Point(siz.Width - siz.Width / 4, siz.Height / 2), new Point(siz.Width / 4, siz.Height / 2));
			g.Dispose();
			return (Image)btm;
		}
		public static Image draw_vertical_sus(Color col, Size siz)
		{
			Bitmap btm = new Bitmap(siz.Width, siz.Height);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;
			g.DrawLine(pen, new Point(siz.Width / 2, siz.Height - siz.Height / 4), new Point(siz.Width / 2, siz.Height / 4));
			g.Dispose();
			return (Image)btm;
		}
		public static Image draw_vertical_jos(Color col, Size siz)
		{
			Bitmap btm = new Bitmap(siz.Width, siz.Height);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;
			g.DrawLine(pen, new Point(siz.Width / 2, siz.Height / 4), new Point(siz.Width / 2, siz.Height - siz.Height / 4));
			g.Dispose();
			return (Image)btm;
		}
		public static Image draw_diagonala_dreapta(Color col, Color col2, int lat, int gros, int tip)
		{
			Bitmap btm = new Bitmap(lat, lat);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			Pen pen2 = new Pen(col2, gros);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;

			pen2.StartCap = LineCap.Round;
			pen2.EndCap = LineCap.Round;
			g.DrawLine(pen2, new Point(0, lat), new Point(lat, 0));
			if (tip == 0)
			{ //diag dreapta sus

				g.DrawLine(pen, new Point(lat / 4, lat - lat / 4), new Point(lat - lat / 4, lat / 4));
			}
			else if (tip == 1)
			{
				//	g.DrawLine(pen2, new Point(lat, 0),new Point(0, lat));
				g.DrawLine(pen, new Point(lat - lat / 4, lat / 4), new Point(lat / 4, lat - lat / 4));
			} //else g.DrawLine(pen2, new Point(lat, 0), new Point(0, lat));
			g.Dispose();
			pen.Dispose();
			return (Image)btm;
		}
		public static Image draw_diagonala_stanga(Color col, Color col2, int lat, int gros, int tip)
		{
			Bitmap btm = new Bitmap(lat, lat);
			Graphics g = Graphics.FromImage(btm);
			Pen pen = new Pen(col, PEN_SIZE);
			Pen pen2 = new Pen(col2, gros);
			pen.StartCap = LineCap.Flat;
			pen.EndCap = LineCap.ArrowAnchor;

			pen2.StartCap = LineCap.Round;
			pen2.EndCap = LineCap.Round;
			g.DrawLine(pen2, new Point(0, 0), new Point(lat, lat));
			if (tip == 0) //diag stanga jos
				g.DrawLine(pen, new Point(lat / 4, lat / 4), new Point(lat - lat / 4, lat - lat / 4));
			else if (tip == 1) g.DrawLine(pen, new Point(lat - lat / 4, lat - lat / 4), new Point(lat / 4, lat / 4));
			g.Dispose();
			pen.Dispose();
			return (Image)btm;
		}
		public static Image draw_intersectie(Color colo, int Diam)
		{
			Bitmap btm = new Bitmap(Diam * 10, Diam * 10);
			Graphics g = Graphics.FromImage(btm);
			Brush br = new SolidBrush(colo);
			g.FillEllipse(br, 0, 0, Diam * 10, Diam * 10);
			g.Dispose();
			br.Dispose();
			return (Image)btm;
		}

		#endregion
		#region declarai
		//	public static Image car = Image.FromFile(Application.StartupPath + @"\Res\car1.png");
		public static Dictionary<Imagini, Image> imag = new Dictionary<Imagini, Image>();
		public static Dictionary<Rotati_Masina, Image>[] masina_img = new Dictionary<Rotati_Masina, Image>[7];
		public static Dictionary<Masini, Image> car = new Dictionary<Masini, Image>();

		public int[,] matr_map;
		public Size map_size;
		public Dictionary<int, Pic> varf_toint = new Dictionary<int, Pic>();
		public static List<Masina> cars;
		public static List<Nota> notes;

		public int vf_curent = 5;
		public static Pic[,] varfuri;//varfurile create
		public Point vf_max = new Point(10, 10);//numarul maxim de varfuri pe linie+coloana
		public Point vf_start;//coordonatele de start a harti
		public Pic[,] vf_posibile;//varfurile din care se construieste
								  //public Pic[,] drumuri;//drumurile pe orizontala pe vericala
		public const int ET_VF_ST = 5;
		public const int IMAG_SIZE_X = 32;//latimea
		public const int IMAG_SIZE_Y = 100;//lungimea
		public const int IMAG_Diag = IMAG_SIZE_Y;
		public const int IMAG_Inter_SIZE = 60;//
		public static readonly Size Car_Size = new Size(60, 30);
		public static readonly Size car_size45 = new Size(60, 60);
		public const int de_unde = (IMAG_Inter_SIZE - IMAG_SIZE_X) / 2;
		public const int de_dublu = (IMAG_Inter_SIZE - IMAG_SIZE_X * 2) / 2;
		public const int scade = IMAG_Inter_SIZE / 2;

		public float consum = 0.2f;
		public Pic vfcur;

		public bool ales_varf;
		public bool ales_drum;
		public bool drum_dublu;
		public bool ales_masina;
		public bool ales_nota;

		#endregion
		#region declarari_eventuri
		public static event EventHandler event_vizibil;
		public static event EventHandler event_vizibil_apr;
		public static event EventHandler event_actualizare_culori;
		public static event EventHandler event_modificare_harta;
		public static event EventHandler event_reinit_culori;
		public static event EventHandler event_misca;
		public static event EventHandler event_bring_to_front;
		public static event EventHandler event_font_change;
		#endregion
		#region init
		public MainForm()
		{
			InitializeComponent();
			//loads = new Form4();
			//loads.Show();
		}
		public MainForm(Size siz)
		{

			InitializeComponent();
			vf_max = new Point(siz.Width,siz.Height);
		}

		public static void car_poze_intit()
		{
			//maisna alba
			//48 imagini
			for (int i = 1; i <= 6; i++)
			{
				masina_img[i] = new Dictionary<Rotati_Masina, Image>();
				masina_img[i][Rotati_Masina.orizontal_stanga] =
					RotateImage(car[(Masini)(i - 1)], 180, true, false, Color.Transparent);
				masina_img[i][Rotati_Masina.orizontal_dreapta] = car[(Masini)i - 1];

				masina_img[i][Rotati_Masina.vertical_sus] = RotateImage(car[(Masini)(i - 1)], -90, true, false, Color.Transparent);
				masina_img[i][Rotati_Masina.vertical_jos] = RotateImage(car[(Masini)(i - 1)], 90, true, false, Color.Transparent);

				masina_img[i][Rotati_Masina.diagonala_dreapta_sus] = RotateImage(car[(Masini)(i - 1)], -45, true, false, Color.Transparent);
				masina_img[i][Rotati_Masina.diagonala_dreapta_jos] = RotateImage(car[(Masini)(i - 1)], 45, true, false, Color.Transparent);

				masina_img[i][Rotati_Masina.diagonala_stanga_jos] = RotateImage(car[(Masini)(i - 1)], 135, true, false, Color.Transparent);
				masina_img[i][Rotati_Masina.diagonala_stanga_sus] = RotateImage(car[(Masini)(i - 1)], -135, true, false, Color.Transparent);
			}
		}
		public static void dictionary_init_car()
		{
			try
			{
				//+54 imagini
				car[Masini.car1] = Image.FromFile(Application.StartupPath + @"\Res\car1.png");
				car[Masini.car2] = Image.FromFile(Application.StartupPath + @"\Res\car2.png");
				car[Masini.car3] = Image.FromFile(Application.StartupPath + @"\Res\car3.png");
				car[Masini.car4] = Image.FromFile(Application.StartupPath + @"\Res\car4.png");
				car[Masini.car5] = Image.FromFile(Application.StartupPath + @"\Res\car5.png");
				car[Masini.car6] = Image.FromFile(Application.StartupPath + @"\Res\car6.png");
				car[Masini.fantoma] = null;
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.ToString(), "Eroare!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
		}
		public void poze_init()
		{
			//+77 imagini
			imag[Imagini.drum_simplu_orizontal_stanga] = draw_orizonatal_stanga(culoare_sageata, new Size(IMAG_SIZE_Y, IMAG_SIZE_X));
			imag[Imagini.drum_simplu_orizontal_dreapta] = draw_orizonatal_dreapta(culoare_sageata, new Size(IMAG_SIZE_Y, IMAG_SIZE_X));

			imag[Imagini.drum_simplu_vertical_sus] = draw_vertical_jos(culoare_sageata, new Size(IMAG_SIZE_X, IMAG_SIZE_Y));
			imag[Imagini.drum_simplu_vertical_jos] = draw_vertical_sus(culoare_sageata, new Size(IMAG_SIZE_X, IMAG_SIZE_Y));

			imag[Imagini.intersectie] = draw_intersectie(culoare_intersectie, IMAG_Inter_SIZE);
			imag[Imagini.intersectie_inactiva] = draw_intersectie(culoare_intersectie_inactiva, IMAG_Inter_SIZE);

			imag[Imagini.drum_simplu_diagonala_dreapta_sus] = draw_diagonala_dreapta(culoare_sageata, culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 0);
			imag[Imagini.drum_simplu_diagonala_dreapta_jos] = draw_diagonala_dreapta(culoare_sageata, culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 1);

			imag[Imagini.drum_simplu_diagonala_stanga_jos] = draw_diagonala_stanga(culoare_sageata, culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 0);
			imag[Imagini.drum_simplu_diagonala_stanga_sus] = draw_diagonala_stanga(culoare_sageata, culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 1);

			imag[Imagini.drum_dublu_diagonala_stanga] = draw_diagonala_stanga(culoare_drum_dublu, culoare_drum_dublu, IMAG_SIZE_Y, IMAG_SIZE_X * 2, 2);
			imag[Imagini.drum_dublu_diagonala_dreapta] = draw_diagonala_dreapta(culoare_drum_dublu, culoare_drum_dublu, IMAG_SIZE_Y, IMAG_SIZE_X * 2, 2);
			///////////////////////////////////////////////////////inverse
			init_inv_color();
			imag[Imagini.inv_drum_simplu_orizontal_stanga] = draw_orizonatal_stanga(inv_culoare_sageata, new Size(IMAG_SIZE_Y, IMAG_SIZE_X));
			imag[Imagini.inv_drum_simplu_orizontal_dreapta] = draw_orizonatal_dreapta(inv_culoare_sageata, new Size(IMAG_SIZE_Y, IMAG_SIZE_X));

			imag[Imagini.inv_drum_simplu_vertical_sus] = draw_vertical_jos(inv_culoare_sageata, new Size(IMAG_SIZE_X, IMAG_SIZE_Y));
			imag[Imagini.inv_drum_simplu_vertical_jos] = draw_vertical_sus(inv_culoare_sageata, new Size(IMAG_SIZE_X, IMAG_SIZE_Y));

			imag[Imagini.inv_intersectie] = draw_intersectie(inv_culoare_intersectie, IMAG_Inter_SIZE);

			imag[Imagini.inv_drum_simplu_diagonala_dreapta_sus] = draw_diagonala_dreapta(inv_culoare_sageata, inv_culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 0);
			imag[Imagini.inv_drum_simplu_diagonala_dreapta_jos] = draw_diagonala_dreapta(inv_culoare_sageata, inv_culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 1);

			imag[Imagini.inv_drum_simplu_diagonala_stanga_jos] = draw_diagonala_stanga(inv_culoare_sageata, inv_culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 0);
			imag[Imagini.inv_drum_simplu_diagonala_stanga_sus] = draw_diagonala_stanga(inv_culoare_sageata, inv_culoare_drum_simplu, IMAG_SIZE_Y, IMAG_SIZE_X, 1);

			imag[Imagini.inv_drum_dublu_diagonala_stanga] = draw_diagonala_stanga(inv_culoare_drum_dublu, inv_culoare_drum_dublu, IMAG_SIZE_Y, IMAG_SIZE_X * 2, 2);
			imag[Imagini.inv_drum_dublu_diagonala_dreapta] = draw_diagonala_dreapta(inv_culoare_drum_dublu, inv_culoare_drum_dublu, IMAG_SIZE_Y, IMAG_SIZE_X * 2, 2);



		}
		public static void init_inv_color()
		{
			inv_culoare_drum_simplu = InvertColour(culoare_drum_simplu);
			inv_culoare_drum_dublu = InvertColour(culoare_drum_dublu);
			inv_culoare_sageata = InvertColour(culoare_sageata);
			inv_culoare_intersectie = InvertColour(culoare_intersectie);
		}
		public void pictureBox_image_sets()
		{
			pictureBox7.Image = imag[Imagini.intersectie];
			pictureBox8.BackColor = culoare_drum_simplu;
			pictureBox8.Image = draw_orizonatal_dreapta(culoare_sageata, new Size(IMAG_SIZE_Y * 2, IMAG_SIZE_X * 2));
			pictureBox9.BackColor = culoare_drum_dublu;
			pictureBox10.BackColor = culore_nota_back;
		}

		void setari_pic_color()
		{
			pictureBox18.BackColor = culoare_background;
			pictureBox17.BackColor = culoare_sageata;
			pictureBox11.BackColor = culoare_drum_simplu;
			pictureBox12.BackColor = culoare_drum_dublu;
			pictureBox14.BackColor = culoare_intersectie_inactiva;
			pictureBox13.BackColor = culoare_intersectie;
			pictureBox15.BackColor = culore_nota_back;
			pictureBox16.BackColor = culore_nota_text;
			textBox1.Font = nota_fnt;

		}
		public void init()
		{
			dictionary_init_car();
			car_poze_intit();
			poze_init();
			pictureBox_image_sets();
			//in total 77 imagini
			vf_start.X = IMAG_Inter_SIZE / 2;
			vf_start.Y = IMAG_Inter_SIZE / 2;

			map_size.Width = vf_max.X * IMAG_Inter_SIZE + (vf_max.X - 1) * IMAG_SIZE_Y + IMAG_Inter_SIZE;
			map_size.Height = vf_max.Y * IMAG_Inter_SIZE + (vf_max.Y - 1) * IMAG_SIZE_Y + IMAG_Inter_SIZE;
			panel2.Size = map_size;

			vf_posibile = new Pic[vf_max.X + 1, vf_max.Y + 1];//*/
			varfuri = new Pic[vf_max.X + 1, vf_max.Y + 1];
			cars = new List<Masina>();
			notes = new List<Nota>();
			Point aux = vf_start;
			splitContainer1.Panel2.BackColor = panel2.BackColor = culoare_background;

			for (int i = 0; i < vf_max.X; i++)
			{
				aux.X = vf_start.X;
				for (int j = 0; j < vf_max.Y; j++)
				{
					vf_posibile[i, j] = new Pic()
					{
						Size = new Size(IMAG_Inter_SIZE, IMAG_Inter_SIZE),
						Location = aux,
						BackgroundImageLayout = ImageLayout.Zoom,
						BackgroundImage = imag[Imagini.intersectie_inactiva],
						tim = Imagini.intersectie_inactiva,
						// BackColor = Color.DarkBlue,
						Visible = false

					};
					aux.X += IMAG_Inter_SIZE + IMAG_SIZE_Y;
					vf_posibile[i, j].Set(i, j);
					panel2.Controls.Add(vf_posibile[i, j]);

					event_vizibil += new EventHandler(vf_posibile[i, j].Modificare);
					event_vizibil_apr += new EventHandler(vf_posibile[i, j].Aprindere);
					event_actualizare_culori += new EventHandler(vf_posibile[i, j].Actualizare_Culori);
					vf_posibile[i, j].Click += new EventHandler(pic_click);
				}
				aux.Y += IMAG_Inter_SIZE + IMAG_SIZE_Y;
			}

			pictureBox1.Click += new EventHandler(masina_Click);
			pictureBox2.Click += new EventHandler(masina_Click);
			pictureBox3.Click += new EventHandler(masina_Click);
			pictureBox4.Click += new EventHandler(masina_Click);
			pictureBox5.Click += new EventHandler(masina_Click);
			pictureBox6.Click += new EventHandler(masina_Click);
			setari_pic_color();

		}
		#endregion

		#region codenervant
		private void Form1_Load(object sender, EventArgs e)
		{
			Thread thr = new Thread(new ThreadStart(
				delegate 
				{
					Program.loader = new load();
					Application.Run(Program.loader);
				}
				));
			thr.SetApartmentState(ApartmentState.STA);

			//thr.Priority = ThreadPriority.Highest;
			thr.Start();
			//thr.Join();
			CenterToScreen();

			if (clasa.disparitie == 2)
			{
				pictureBox19.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\verde.png");
			}
			else
			{
				pictureBox19.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");
				timer2.Start();
				
			}
			if (Program.file_name != null) { open_file(Program.file_name); }
			else init();
			Program.file_name = null;
			Program.refa = false;

			//	Program.file_name = null;
			//	Program.refa = false;
			tabControl2.Select();
			//Program.rec?.KILL();
			if(Program.loader!=null)
			{
				Program.loader.Invoke(new Action(Program.loader.Close));
				Program.loader.Dispose();
				Program.loader = null;
			}
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			inchidere_check();
			timer2.Stop();
		
			if (Program.loader != null)
			{
				Program.loader.Invoke(new Action(Program.loader.Close));
				Program.loader.Dispose();
				Program.loader = null;
			}
			event_modificare_harta = null;
			cars.Clear();
			car.Clear();
			imag.Clear();
			for (int i = 1; i <= 6; i++)
			{
				masina_img[i].Clear();
			}
				////foreach (var val in masina_img)
				////{
				////	val.Clear();
				////}
				GC.Collect();

			base.OnClosing(e);

		}
		void creare_intersectie(Pic loc)
		{
			//initializarea intersectiei
			varfuri[loc.x, loc.y] = new Pic()
			{
				Location = loc.Location,
				Size = loc.Size,
				BackgroundImageLayout = ImageLayout.Zoom,
				BackgroundImage = imag[Imagini.intersectie],
				tim = Imagini.intersectie,
				BackColor = Color.Transparent,
				Visible = true,
				//initilaizarile drumurilor
				drum_diag = new List<Pic>(),
				drum = new List<Pic>(),
				drumfaraacces = new List<Pic>(),
				drum_diagfaraacces = new List<Pic>()
			};
			//adaugarea intesectie in panou,aducerea in fata, setarea locatiei, si abonarea la evenimente
			varfuri[loc.x, loc.y].SetV(loc.x, loc.y, vf_curent++);
			panel2.Controls.Add(varfuri[loc.x, loc.y]);
			varfuri[loc.x, loc.y].Click += new EventHandler(varf_click);
			varfuri[loc.x, loc.y].BringToFront();
			varfuri[loc.x, loc.y].ContextMenuStrip = contextMenuStrip1;
			event_actualizare_culori += new EventHandler(varfuri[loc.x, loc.y].Actualizare_Culori);
			event_bring_to_front?.Invoke(null, null);
		}
		void cost()
		{
			if(clasa.disparitie==1)// daca gratuit
			{
				toolStripTextBox1.Text ="";
				return;
			}
			int cate_dr_d = 0;//drumuri duble care ies sau intra
			int cate_dr_s = 0;//drumuri simple care ies
			int cate_dr_in = 0;//drumuri siple care intra
			decimal sum = 0;
			int[,] a = matr_generea_adiacenta();//genereaza matricea de adiacenta a proiectului

			for (int i = 0; i < a.GetLength(0); i++)
			{
				cate_dr_d = 0; cate_dr_s = cate_dr_in = 0;
				for (int j = 0; j < a.GetLength(1); j++)
				{
					if (a[i, j] == 1)
					{
						if (a[j, i] == 1) cate_dr_d++;
						else cate_dr_s++;
					}
					else if (a[j, i] == 1) cate_dr_in++;
				}
				if (cate_dr_s + cate_dr_d + cate_dr_in == 3) sum += numericUpDown6.Value;
				else if (cate_dr_d + cate_dr_s + cate_dr_in == 4) sum += numericUpDown3.Value;
				else if (cate_dr_s + cate_dr_d + cate_dr_in > 4) sum += numericUpDown4.Value;

				sum += cate_dr_d * numericUpDown2.Value;
				sum += cate_dr_s * numericUpDown1.Value;
			}
			toolStripTextBox1.Text = sum + " Ron";

		}

		void creare_drum(Pic point1, Pic point2, int tip)//tip va fi 1(drum unic)/2 (drum dublu)
		{
			if (point1 == null || point2 == null) return;
			Pic aux = null;
			if (tip == 1)
			{
				if (point1.y == point2.y)//verical
				{
					if (point2.x > point1.x) aux = new Pic()
						{
							Location = new Point(point1.Location.X + de_unde, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_SIZE_X, IMAG_SIZE_Y),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_simplu_vertical_sus,
							BackgroundImage = imag[Imagini.drum_simplu_vertical_sus],
							BackColor = culoare_drum_simplu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					else aux = new Pic()
						{
							Location = new Point(point2.Location.X + de_unde, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_SIZE_X, IMAG_SIZE_Y),
							BackgroundImageLayout = ImageLayout.Center,
							tim = Imagini.drum_simplu_vertical_jos,
							BackgroundImage = imag[Imagini.drum_simplu_vertical_jos],
							BackColor = culoare_drum_simplu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					aux.setif(point1, point2);
					aux.eticheta = 1;
					panel2.Controls.Add(aux);
					aux.Click += new EventHandler(drum_click);
					point1.drum.Add(aux);
					point2.drumfaraacces.Add(aux);
				}
				else if (point2.x == point1.x)//orizontal
				{
					if (point2.y > point1.y)
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point1.Location.Y + de_unde),
							Size = new Size(IMAG_SIZE_Y, IMAG_SIZE_X),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_simplu_orizontal_dreapta,
							BackgroundImage = imag[Imagini.drum_simplu_orizontal_dreapta],

							BackColor = culoare_drum_simplu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};

					else
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point2.Location.Y + de_unde),
							Size = new Size(IMAG_SIZE_Y, IMAG_SIZE_X),
							BackgroundImageLayout = ImageLayout.Center,
							tim = Imagini.drum_simplu_orizontal_stanga,
							BackgroundImage = imag[Imagini.drum_simplu_orizontal_stanga],

							BackColor = culoare_drum_simplu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					aux.setif(point1, point2);
					aux.eticheta = 1;
					panel2.Controls.Add(aux);
					aux.Click += new EventHandler(drum_click);
					point1.drum.Add(aux);
					point2.drumfaraacces.Add(aux);
				}
				else
				{
					if (point2.x < point1.x && point1.y > point2.y)//sus stanga
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_simplu_diagonala_stanga_sus,
							BackgroundImage = imag[Imagini.drum_simplu_diagonala_stanga_sus],

							//   BackColor = Color.Violet,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					else if (point2.x < point1.x && point1.y < point2.y)//sus dreapta
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							BackgroundImage = imag[Imagini.drum_simplu_diagonala_dreapta_sus],

							tim = Imagini.drum_simplu_diagonala_dreapta_sus,
							//   BackColor = Color.Violet,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					else if (point2.x > point1.x && point1.y > point2.y)//jos stanga
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_simplu_diagonala_dreapta_jos,
							BackgroundImage = imag[Imagini.drum_simplu_diagonala_dreapta_jos],

							// BackColor = Color.Violet,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					else if (point2.x > point1.x && point1.y < point2.y)//jos dreapta
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_simplu_diagonala_stanga_jos,
							BackgroundImage = imag[Imagini.drum_simplu_diagonala_stanga_jos],

							//  BackColor = Color.Violet,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};

					}
					aux.Click += new EventHandler(drum_click);
					aux.setif(point1, point2);
					aux.eticheta = 3;
					panel2.Controls.Add(aux);
					point1.drum_diag.Add(aux);
					point2.drum_diagfaraacces.Add(aux);
				}
			}
			else
			{
				if (point2.y == point1.y)//verical
				{
					if (point2.x > point1.x)
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + de_dublu, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_SIZE_X * 2, IMAG_SIZE_Y),
							BackgroundImageLayout = ImageLayout.Center,
							//   BackgroundImage = 
							BackColor = culoare_drum_dublu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}

					else
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + de_dublu, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_SIZE_X * 2, IMAG_SIZE_Y),
							BackgroundImageLayout = ImageLayout.Center,
							//BackgroundImage = drum_simplu_vertical,
							BackColor = culoare_drum_dublu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					aux.eticheta = 2;
					aux.setif(point1, point2);
					panel2.Controls.Add(aux);
					aux.Click += new EventHandler(drum_click);
					point1.drum.Add(aux);
					point2.drum.Add(aux);
				}
				else if (point2.x == point1.x)//orizontal
				{
					if (point2.y > point1.y)
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point1.Location.Y + de_dublu),
							Size = new Size(IMAG_SIZE_Y, IMAG_SIZE_X * 2),
							BackgroundImageLayout = ImageLayout.Center,
							//      BackgroundImage = drum_simplu_orizontal,
							BackColor = culoare_drum_dublu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}

					else
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point2.Location.Y + de_dublu),
							Size = new Size(IMAG_SIZE_Y, IMAG_SIZE_X * 2),
							BackgroundImageLayout = ImageLayout.Center,
							//   BackgroundImage = drum_simplu_orizontal,
							BackColor = culoare_drum_dublu,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};

					}
					aux.eticheta = 2;
					aux.setif(point1, point2);
					panel2.Controls.Add(aux);
					aux.Click += new EventHandler(drum_click);
					point1.drum.Add(aux);
					point2.drum.Add(aux);
				}
				else
				{
					if (point2.x < point1.x && point1.y > point2.y)//sus stanga
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_dublu_diagonala_stanga,
							BackgroundImage = imag[Imagini.drum_dublu_diagonala_stanga],
							//  BackColor = Color.Violet,
							Visible = true,
							ContextMenuStrip = contextMenuStrip1

						};
					}
					else if (point2.x < point1.x && point1.y < point2.y)//sus dreapta
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point2.Location.Y + point2.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_dublu_diagonala_dreapta,
							BackgroundImage = imag[Imagini.drum_dublu_diagonala_dreapta],
							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					else if (point2.x > point1.x && point1.y > point2.y)//jos stanga
					{
						aux = new Pic()
						{
							Location = new Point(point2.Location.X + point2.Width, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_dublu_diagonala_dreapta,
							BackgroundImage = imag[Imagini.drum_dublu_diagonala_dreapta],

							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					else if (point2.x > point1.x && point1.y < point2.y)//jos dreapta
					{
						aux = new Pic()
						{
							Location = new Point(point1.Location.X + point1.Width, point1.Location.Y + point1.Height),
							Size = new Size(IMAG_Diag, IMAG_Diag),
							BackgroundImageLayout = ImageLayout.Zoom,
							tim = Imagini.drum_dublu_diagonala_stanga,
							BackgroundImage = imag[Imagini.drum_dublu_diagonala_stanga],

							Visible = true,
							ContextMenuStrip = contextMenuStrip1
						};
					}
					aux.Click += new EventHandler(drum_click);

					aux.setif(point1, point2);
					aux.eticheta = 4;
					panel2.Controls.Add(aux);
					point1.drum_diag.Add(aux);
					point2.drum_diag.Add(aux);
				}
			}
			event_actualizare_culori += new EventHandler(aux.Actualizare_Culori);
			event_vizibil(null, null);
			event_modificare_harta?.Invoke(null, null);
			event_bring_to_front?.Invoke(null, null);
			cost();

		}
		public static bool coord_egale(coord c1, Pic c2)
		{
			return (c1.x == c2.x) && (c1.y == c2.y);
		}
		void conflict_pe_diagonala(Pic p1, Pic p2)
		{
			if (varfuri[p2.x, p1.y] == null || varfuri[p1.x, p2.y] == null) return;
			Pic pnt1 = varfuri[p2.x, p1.y],
				pnt2 = varfuri[p1.x, p2.y];

			foreach (Pic c in pnt1.drum_diag)
			{
				if (coord_egale(c.f, pnt2))
				{
					//	MessageBox.Show("sergem");
					eliminare_drum(c); return;
				}

			}
			foreach (Pic c in pnt2.drum_diag)
			{
				if (coord_egale(c.f, pnt1))
				{
					//MessageBox.Show("sergem");
					eliminare_drum(c); return;
				}

			}

		}
		bool exista_drum(Pic pnt1, Pic pnt2, int tip)
		{
			if ((pnt1.y == pnt2.y) || (pnt1.x == pnt2.x))
			{
				foreach (Pic c in pnt1.drum)
				{
					if (c.eticheta == 1)
					{
						if (coord_egale(c.f, pnt2))
						{
							if (c.eticheta == tip) return true;
							eliminare_drum(c);
							break;
						}
					}
					else if (coord_egale(c.f, pnt2) || coord_egale(c.i, pnt2))
					{
						if (c.eticheta == tip) return true;
						eliminare_drum(c);
						break;
					}
				}
				foreach (Pic c in pnt1.drumfaraacces)
				{
					if (coord_egale(c.i, pnt2))
					{
						eliminare_drum(c);
						break;
					}
				}
				return false;
			}
			else
			{
				foreach (Pic c in pnt1.drum_diag)
				{
					if (c.eticheta == 3)
					{
						if (coord_egale(c.f, pnt2))
						{
							if (c.eticheta == tip) return true;
							eliminare_drum(c);
							break;
						}

					}
					else if (coord_egale(c.f, pnt2) || coord_egale(c.i, pnt2))
					{
						if (c.eticheta == tip) return true;
						eliminare_drum(c);
						break;
					}

				}
				foreach (Pic c in pnt1.drum_diagfaraacces)
				{
					if (coord_egale(c.i, pnt2))
					{
						eliminare_drum(c);
						break;
					}
				}
				conflict_pe_diagonala(pnt1, pnt2);
				return false;
			}
		}
		void pic_click(object sender, EventArgs e)
		{
			event_reinit_culori?.Invoke(null, null);

			Pic pnt1 = sender as Pic;

			if (vfcur == pnt1) return;

			if (ales_varf)
			{
				creare_intersectie(pnt1);
			}

			else
			{
				Pic pnt2 = vfcur;
				int tip = 0;
				if (vfcur == null) return;
				if (ales_drum) tip = 1;
				else if (drum_dublu) tip = 2;
				if (tip > 0)
				{
					if (pnt1.y == pnt2.y)
					{
						if (pnt1.x < pnt2.x)
						{
							for (int i = pnt2.x - 1; i >= pnt1.x; i--)
							{
								if (varfuri[i, pnt1.y] == null)
								{
									creare_intersectie(vf_posibile[i, pnt1.y]);
									creare_drum(varfuri[i + 1, pnt1.y], varfuri[i, pnt1.y], tip);
								}
								else
									if (!exista_drum(varfuri[i + 1, pnt1.y], varfuri[i, pnt1.y], tip)) creare_drum(varfuri[i + 1, pnt1.y], varfuri[i, pnt1.y], tip);
							}
						}
						else if (pnt1.x > pnt2.x)
						{
							for (int i = pnt2.x; i < pnt1.x; i++)
							{
								if (varfuri[i + 1, pnt1.y] == null)
								{
									creare_intersectie(vf_posibile[i + 1, pnt1.y]);
									creare_drum(varfuri[i, pnt1.y], varfuri[i + 1, pnt1.y], tip);
								}
								else if (!exista_drum(varfuri[i, pnt1.y], varfuri[i + 1, pnt1.y], tip)) creare_drum(varfuri[i, pnt1.y], varfuri[i + 1, pnt1.y], tip);
							}
						}
						event_vizibil(null, null); vfcur = null;

					}
					else
					{
						if (pnt1.x == pnt2.x)
						{
							if (pnt1.y < pnt2.y)
							{
								for (int i = pnt2.y - 1; i >= pnt1.y; i--)
								{

									if (varfuri[pnt1.x, i] == null)
									{
										creare_intersectie(vf_posibile[pnt1.x, i]);
										creare_drum(varfuri[pnt1.x, i + 1], varfuri[pnt1.x, i], tip);
									}
									else if (!exista_drum(varfuri[pnt1.x, i + 1], varfuri[pnt1.x, i], tip)) creare_drum(varfuri[pnt1.x, i + 1], varfuri[pnt1.x, i], tip);
								}
							}
							else if (pnt1.y > pnt2.y)
							{
								for (int i = pnt2.y; i < pnt1.y; i++)
								{
									if (varfuri[pnt1.x, i + 1] == null)
									{
										creare_intersectie(vf_posibile[pnt1.x, i + 1]);
										creare_drum(varfuri[pnt1.x, i], varfuri[pnt1.x, i + 1], tip);
									}
									else if (!exista_drum(varfuri[pnt1.x, i], varfuri[pnt1.x, i + 1], tip)) creare_drum(varfuri[pnt1.x, i], varfuri[pnt1.x, i + 1], tip);
								}
							}
							event_vizibil(null, null); vfcur = null;

						}
						else
						{
							if (((pnt2.x - pnt1.x) == (pnt2.y - pnt1.y)) || ((pnt2.x - pnt1.x) == (pnt1.y - pnt2.y)))
							{

								if (pnt1.x < pnt2.x && pnt2.y > pnt1.y)//sus stanga
								{
									for (int i = pnt2.x - 1, j = pnt2.y - 1; i >= pnt1.x && j >= pnt1.y; i--, j--)
									{
										if (varfuri[i, j] == null)
										{
											creare_intersectie(vf_posibile[i, j]);
											conflict_pe_diagonala(varfuri[i + 1, j + 1], varfuri[i, j]);
											creare_drum(varfuri[i + 1, j + 1], varfuri[i, j], tip);
										}
										else if (!exista_drum(varfuri[i + 1, j + 1], varfuri[i, j], tip)) creare_drum(varfuri[i + 1, j + 1], varfuri[i, j], tip);
									}
								}
								else if (pnt1.x < pnt2.x && pnt2.y < pnt1.y)//sus dreapta
								{
									for (int i = pnt2.x - 1, j = pnt2.y + 1; i >= pnt1.x && j <= pnt1.y; i--, j++)
									{
										if (varfuri[i, j] == null)
										{
											creare_intersectie(vf_posibile[i, j]);
											conflict_pe_diagonala(varfuri[i + 1, j - 1], varfuri[i, j]);
											creare_drum(varfuri[i + 1, j - 1], varfuri[i, j], tip);
										}
										else if (!exista_drum(varfuri[i + 1, j - 1], varfuri[i, j], tip)) creare_drum(varfuri[i + 1, j - 1], varfuri[i, j], tip);
									}
								}
								else if (pnt1.x > pnt2.x && pnt2.y > pnt1.y)//jos stanga
								{
									for (int i = pnt2.x + 1, j = pnt2.y - 1; i <= pnt1.x && j >= pnt1.y; i++, j--)
									{
										if (varfuri[i, j] == null)
										{
											creare_intersectie(vf_posibile[i, j]);
											conflict_pe_diagonala(varfuri[i - 1, j + 1], varfuri[i, j]);
											creare_drum(varfuri[i - 1, j + 1], varfuri[i, j], tip);
										}
										else if (!exista_drum(varfuri[i - 1, j + 1], varfuri[i, j], tip)) creare_drum(varfuri[i - 1, j + 1], varfuri[i, j], tip);
									}
								}
								else if (pnt1.x > pnt2.x && pnt2.y < pnt1.y)//jos dreapta
								{
									for (int i = pnt2.x + 1, j = pnt2.y + 1; i <= pnt1.x && j <= pnt1.y; i++, j++)
									{
										if (varfuri[i, j] == null)
										{
											creare_intersectie(vf_posibile[i, j]);
											conflict_pe_diagonala(varfuri[i - 1, j - 1], varfuri[i, j]);
											creare_drum(varfuri[i - 1, j - 1], varfuri[i, j], tip);
										}
										else if (!exista_drum(varfuri[i - 1, j - 1], varfuri[i, j], tip)) creare_drum(varfuri[i - 1, j - 1], varfuri[i, j], tip);
									}
								}
								event_vizibil(null, null); vfcur = null;

							}
							else
							{
								event_vizibil(null, null);
								vfcur = pnt1;
								aprindere(pnt1);
							}
						}
					}
				}
			}

		}
		void varf_click(object sender, EventArgs e)
		{
			event_reinit_culori?.Invoke(null, null);

			if (ales_drum || drum_dublu)
			{
				if (vfcur == null)
				{
					vfcur = sender as Pic;
					event_vizibil(null, null);
					aprindere(vfcur);
					pic_click(sender, e);
				}
				else { event_vizibil(null, null); aprindere(vfcur); pic_click(sender, e); }



			}
			else if (ales_masina)
			{
				//MessageBox.Show("Dg");
				Masina aux = cars.Last();
				aux.gaseste_drum(sender as Pic);

				//event_vizibil(null, null);
				if (aux.ExistaDrum)
				{
					aux.arata_drum();
				}
				ales_masina = false;
				vfcur = null;

			}
			else vfcur = sender as Pic;
		}
		//void stergere_drum()

		void drum_click(object sender, EventArgs e)
		{
			event_reinit_culori?.Invoke(null, null);

			// contextMenuStrip1.s
		}
		void aprindere(Pic vf)
		{
			for (int i = 0; i < vf_max.X; i++)
			{
				vf_posibile[i, vf.y].Visible = true;
			}
			for (int i = 0; i < vf_max.Y; i++)
			{
				vf_posibile[vf.x, i].Visible = true;
			}
			//merge in sus stanga
			for (int i = vf.x, j = vf.y; i >= 0 && j >= 0; --j, --i) vf_posibile[i, j].Visible = true;
			for (int i = vf.x, j = vf.y; i < vf_max.X && j < vf_max.Y; ++j, ++i) vf_posibile[i, j].Visible = true;
			for (int i = vf.x, j = vf.y; i >= 0 && j < vf_max.Y; --i, ++j) vf_posibile[i, j].Visible = true;
			for (int i = vf.x, j = vf.y; i < vf_max.X && j >= 0; --j, ++i) vf_posibile[i, j].Visible = true;
		}
		void eliminare_drum(Pic vf)
		{
			if (vf.eticheta == 1)//daca e drum simplu verical sau orizontal 
			{
				varfuri[vf.i.x, vf.i.y].drum.Remove(vf);
				varfuri[vf.f.x, vf.f.y].drumfaraacces.Remove(vf);
				vf.Dispose();

			}
			else if (vf.eticheta == 2)//daca e drum dublu verical sau orizontal
			{
				varfuri[vf.i.x, vf.i.y].drum.Remove(vf);
				varfuri[vf.f.x, vf.f.y].drum.Remove(vf);
				vf.Dispose();
			}
			else if (vf.eticheta == 3)//daca e drum simplu pe diagonala
			{
				varfuri[vf.i.x, vf.i.y].drum_diag.Remove(vf);
				varfuri[vf.f.x, vf.f.y].drum_diagfaraacces.Remove(vf);
				vf.Dispose();
			}
			else //daca e drum dublu pe diagonala
			{
				varfuri[vf.i.x, vf.i.y].drum_diag.Remove(vf);
				varfuri[vf.f.x, vf.f.y].drum_diag.Remove(vf);
				vf.Dispose();
			}
		}
		private void stergeToolStripMenuItem_Click(object sender, EventArgs e)
		{

			ales_drum = ales_masina = ales_nota = ales_varf = drum_dublu = false;
			vfcur = null;
			// Try to cast the sender to a ToolStripItem
			ToolStripItem menuItem = sender as ToolStripItem;
			if (menuItem != null)
			{
				// Retrieve the ContextMenuStrip that owns this ToolStripItem
				ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
				if (owner != null)
				{
					// Get the control that is displaying this context menu
					Control sourceControl = owner.SourceControl;
					if (sourceControl is Masina)
					{
						event_reinit_culori?.Invoke(null, null);
						(sourceControl as Masina).MORTE();
						return;
					}
					else if (sourceControl is Nota)
					{
						Nota nota = sourceControl as Nota;
						notes.Remove(nota);
						nota.Dispose();
						return;
					}


					event_vizibil(null, null);
					Pic vf = sourceControl as Pic;
					// MessageBox.Show(aux.eticheta.ToString());
					if (vf.eticheta < 5)
					{
						eliminare_drum(vf);
					}
					else
					{
						//eliminare drumurile de vf curent
						foreach (Pic i in vf.drum)
						{
							if (i.eticheta == 2)//daca e drum vericalorizontal
							{
								if (varfuri[i.f.x, i.f.y] == vf)
									varfuri[i.i.x, i.i.y].drum.Remove(i);
								else varfuri[i.f.x, i.f.y].drum.Remove(i);
							}
							else
							{
								varfuri[i.f.x, i.f.y].drumfaraacces.Remove(i);
							}
							i.Dispose();
						}
						//eiminare drumurilor de pe diagonale 
						foreach (Pic i in vf.drum_diag)
						{
							if (i.eticheta == 4)
							{
								if (varfuri[i.f.x, i.f.y] == vf)
									varfuri[i.i.x, i.i.y].drum_diag.Remove(i);
								else varfuri[i.f.x, i.f.y].drum_diag.Remove(i);
							}
							else
							{
								///	if(i!=null && varfuri[i.f.x, i.f.y]!=null)
								varfuri[i.f.x, i.f.y].drum_diagfaraacces.Remove(i);
							}
							i.Dispose();
						}
						foreach (Pic i in vf.drumfaraacces)
						{
							{
								//  if (i.eticheta == 1)//daca e drum vericalorizontal
								{
									varfuri[i.i.x, i.i.y].drum.Remove(i);

								}
								i.Dispose();
							}
						}
						foreach (Pic i in vf.drum_diagfaraacces)
						{
							//   if (i.eticheta == 4)
							{
								varfuri[i.i.x, i.i.y].drum_diag.Remove(i);
							}
							i.Dispose();
						}

						varfuri[vf.x, vf.y].Dispose();
						varfuri[vf.x, vf.y] = null;
						//   panel2.Refresh();
					}
					cost();
					event_modificare_harta?.Invoke(null, null);

				}
			}

		}
		int[,] matr_generea_adiacenta()
		{

			int aux1, aux2;
			int vf = 0;
			Dictionary<int, int> t = new Dictionary<int, int>();
			foreach (Pic p in varfuri)
			{
				if (p != null)
				{
					t.Add(p.eticheta, vf);
					vf++;
				}
			}
			int[,] a = new int[t.Count, t.Count];

			foreach (Pic p in varfuri)
			{
				if (p != null)
				{
					foreach (Pic d in p.drum)
					{
						if (d == null) continue;
						//	if (varfuri[d.i.x, d.i.y] == null || varfuri[d.f.x, d.f.y] == null) continue;

						aux1 = t[varfuri[d.i.x, d.i.y].eticheta];
						aux2 = t[varfuri[d.f.x, d.f.y].eticheta];
						if (d.eticheta == 2)
						{
							a[aux1, aux2] = 1;
							a[aux2, aux1] = 1;
						}
						else a[aux1, aux2] = 1;
					}
					foreach (Pic d in p.drum_diag)
					{
						if (d == null) continue;
						//	if (varfuri[d.i.x, d.i.y] == null || varfuri[d.f.x, d.f.y] == null) continue;
						aux1 = t[varfuri[d.i.x, d.i.y].eticheta];
						aux2 = t[varfuri[d.f.x, d.f.y].eticheta];
						if (d.eticheta == 4)
						{
							a[aux1, aux2] = 1;
							a[aux2, aux1] = 1;
						}
						else a[aux1, aux2] = 1;
					}
				}
			}
			return a;
		}
		public void open_file(string file)
		{
			int[,] a = null;// initilaizam o matrice nu null 
			try
			{
				if (File.Exists(file)) //verificam daca exista fisierul 
				{
					string[] aux;
					string[] info = File.ReadAllLines(file);//citim toate liniele 
					string[] size = info[0].Split(' '); //impartim textul
					vf_max = new Point(int.Parse(size[0]), int.Parse(size[1]));
	
					size = info[1].Split(' ');
					int n = int.Parse(size[0]), m = int.Parse(size[1]);
					int loc = n * 2 + 2;
					//initializam culorile
					aux = info[loc++].Split(' ');
					culoare_background = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culoare_intersectie_inactiva = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culoare_intersectie = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culoare_drum_simplu = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culoare_drum_dublu = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culoare_sageata = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culore_nota_back = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split(' ');
					culore_nota_text = Color.FromArgb(int.Parse(aux[0]), int.Parse(aux[1]), int.Parse(aux[2]));
					aux = info[loc++].Split('|');
					nota_fnt = new Font(aux[0], float.Parse(aux[2]), ((FontStyle)int.Parse(aux[1])));
					init();//
					int nnn = int.Parse(info[loc++]);
					string[] aux2, aux3;
					for (int i = 0; i < nnn; i++)
					{
						aux = info[loc + i].Split('|');
						aux2 = aux[0].Split(' ');
						aux3 = aux[1].Split(' ');
						creare_TextBox(new Point(int.Parse(aux2[0]), int.Parse(aux2[1])), new Size(int.Parse(aux3[0]), int.Parse(aux3[1])), aux[2]);
					}
					loc += nnn;
					numericUpDown1.Value = decimal.Parse(info[loc++]);
					numericUpDown2.Value = decimal.Parse(info[loc++]);
					numericUpDown6.Value = decimal.Parse(info[loc++]);
					numericUpDown3.Value = decimal.Parse(info[loc++]);
					numericUpDown4.Value = decimal.Parse(info[loc++]);
					numericUpDown5.Value = decimal.Parse(info[loc++]);
					a = new int[n, m];
					//citirea matricei
					for (int i = 0; i < n; i++)
					{
						aux = info[i + 2].Split(' ');
						for (int j = 0; j < m; j++)
							a[i, j] = int.Parse(aux[j]);
					}
					Dictionary<int, coord> d = new Dictionary<int, coord>();
					for (int i = 0; i < n; i++)
					{
						aux = info[n + 2 + i].Split(' ');
						creare_intersectie(vf_posibile[int.Parse(aux[0]), int.Parse(aux[1])]);
						d.Add(i + 5, new coord(int.Parse(aux[0]), int.Parse(aux[1])));
					}
                    //creare drumurilor din prima matrice
                    bool[,] creat = new bool[n, m];

					for (int i = 0; i < n; i++)
						for (int j = 0; j < m; j++)
							if (a[i, j] != 0)
							{
								coord pnt1 = d[i + 5];
								coord pnt2 = d[j + 5];
                                if (a[i, j] == a[j, i]) 
                                {
                                    if (creat[i, j] == false)
                                    {
                                        creat[j, i] = creat[i, j] = true;
                                        creare_drum(varfuri[pnt1.x, pnt1.y], varfuri[pnt2.x, pnt2.y], 2);
                                    }
                                }
                                else
                                    creare_drum(varfuri[pnt1.x, pnt1.y], varfuri[pnt2.x, pnt2.y], 1);
							}

				}
			}
			catch (Exception ee) { MessageBox.Show(ee.ToString()); }
		}
		void creare_masina(int img)
		{
			Masina mas = new Masina(vfcur, img, panel2, Car_Size,algoritm);
			event_modificare_harta += new EventHandler(mas.schimbare_harta);
			event_reinit_culori += new EventHandler(mas.init_culori);
			event_misca += new EventHandler(mas.Misca);
			mas.Click += new EventHandler(masina_click);
			mas.ContextMenuStrip = contextMenuStrip1;
			cars.Add(mas);
			new Form3(cars.Count - 1).ShowDialog();
		}


		private void masina_Click(object sender, EventArgs e)
		{
			if (vfcur != null)
			{
				ales_drum = ales_varf = ales_nota = drum_dublu = false;
				ales_masina = true;
				PictureBox aux = sender as PictureBox;
				int numar = aux.Name[aux.Name.Length - 1] - '0';
				creare_masina(numar);
			}
			else MessageBox.Show("Selecteaza o intersectie ca punct de refetinta.", "Detali...", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}
		public void masina_click(object sender, EventArgs e)
		{
			event_reinit_culori?.Invoke(null, null);
			(sender as Masina).arata_drum();


		}

		public static bool[,] genereaza_matr_bool()
		{
			bool[,] matr = new bool[varfuri.GetLength(0), varfuri.GetLength(1)];
			for (int i = 0; i < varfuri.GetLength(0); i++)
				for (int j = 0; j < varfuri.GetLength(1); j++)
					if (varfuri[i, j] != null) matr[i, j] = true;
					else matr[i, j] = false;
			return matr;
		}


		private void timer1_Tick(object sender, EventArgs e)
		{

			event_misca?.Invoke(null, null);
			event_bring_to_front?.Invoke(null, null);
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			Masina.speed = trackBar1.Value;
			if (Masina.speed == 0) timer1.Stop();
			else if (!timer1.Enabled && car != null && car.Count > 0) timer1.Start();
		}
		public static void apel_event_reinit_color()
		{
			event_reinit_culori?.Invoke(null, null);
		}
		Point first_prs = Point.Empty;
		void creare_TextBox(Point location, Size siz, string text)
		{
			Nota txt = new Nota
			{
				BorderStyle = BorderStyle.None,
				Multiline = true,
				BackColor = culore_nota_back,
				ForeColor = culore_nota_text,
				Font = nota_fnt,
				Location = location,
				Size = siz,
				Text = text,
				ContextMenuStrip = contextMenuStrip1

			};

			panel2.Controls.Add(txt);
			txt.BringToFront();
			txt.Focus();
			event_bring_to_front += new EventHandler(txt.bring);
			event_font_change += new EventHandler(txt.fnt_chng);
			notes.Add(txt);
		}
        #region nimic important 2
        private void panel2_MouseDown(object sender, MouseEventArgs e)
		{
			if (ales_nota)
				first_prs = e.Location;
		}

		private void panel2_MouseUp(object sender, MouseEventArgs e)
		{
			if (ales_nota)
			{
				creare_TextBox(new Point(Math.Min(first_prs.X, e.Location.X), Math.Min(first_prs.Y, e.Location.Y)), new Size(Math.Abs(e.Location.X - first_prs.X), Math.Abs(e.Location.Y - first_prs.Y)), "");
			}
		}

		private void pictureBox7_Click(object sender, EventArgs e)//intersectie
		{
			ales_masina = ales_drum = drum_dublu = ales_nota = false;
			ales_varf = true;
			vfcur = null;
			if (vf_posibile[0, 0].Visible == true) event_vizibil(sender, e);
			else event_vizibil_apr(sender, e);
		}

		private void pictureBox8_Click(object sender, EventArgs e)//drum simplu
		{
			if (vf_posibile[0, 0].Visible) event_vizibil(sender, e);
			if (vfcur != null)
			{
				aprindere(vfcur);
				ales_masina = ales_varf = ales_nota = drum_dublu = false;
				ales_drum = true;
			}
			else MessageBox.Show("Selecteaza o intersectie ca punct de refetinta.", "Detali...", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void pictureBox9_Click(object sender, EventArgs e)//drum dublu
		{
			if (vf_posibile[0, 0].Visible) event_vizibil(sender, e);
			if (vfcur != null)
			{
				aprindere(vfcur);
				ales_masina = ales_varf = ales_nota = ales_drum = false;
				drum_dublu = true;
			}
			else MessageBox.Show("Selecteaza o intersectie ca punct de refetinta.", "Detali...", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void pictureBox10_Click(object sender, EventArgs e)//notita
		{
			if (vf_posibile[0, 0].Visible) event_vizibil(sender, e);
			ales_masina = ales_drum = drum_dublu = ales_varf = false;
			ales_nota = true;
		}

		private void tabControl2_Selected(object sender, TabControlEventArgs e)
		{
			//	trackBar1.Show();
		}

		private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(clasa.disparitie==1&&tabControl2.SelectedIndex!=0)
			{
				MessageBox.Show("Ai nevoie de versiunea Pro pentru a avea acces la aceasta optiune.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tabControl2.SelectedIndex = 0;
				return;
			}


			if (tabControl2.SelectedIndex == 1)
			{
				trackBar1.Show();
				toolStripTextBox2.Enabled = true;
			}

			else
			{
				trackBar1.Hide();
				toolStripTextBox2.Enabled = false;
				event_reinit_culori?.Invoke(null, null);
			}
		}

		private void hIdeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			splitContainer1.Panel1Collapsed = true;
		}

		private void showToolStripMenuItem_Click(object sender, EventArgs e)
		{
			splitContainer1.Panel1Collapsed = false;
		}
		Color obtine_culoare(Color col)
		{
			ColorDialog clr = new ColorDialog();
			clr.AllowFullOpen = true;
			clr.FullOpen = true;
			clr.Color = culoare_drum_simplu;
			if (clr.ShowDialog() == DialogResult.OK)
			{
				clr.Dispose();
				return clr.Color;
			}
			return col;
		}
        #endregion
        #region paginadesetari
        private void pictureBox18_Click(object sender, EventArgs e)//culoare background
		{
			pictureBox18.BackColor = culoare_background = obtine_culoare(culoare_background);
			panel2.BackColor = culoare_background;
			splitContainer1.Panel2.BackColor = culoare_background;
		}

		private void pictureBox17_Click(object sender, EventArgs e)
		{
			pictureBox17.BackColor = culoare_sageata = obtine_culoare(culoare_sageata);
			poze_init();
			event_actualizare_culori?.Invoke(null, null);
			pictureBox_image_sets();
		}

		private void pictureBox11_Click(object sender, EventArgs e)
		{
			pictureBox11.BackColor = culoare_drum_simplu = obtine_culoare(culoare_drum_simplu);
			poze_init();
			event_actualizare_culori?.Invoke(null, null);
			pictureBox_image_sets();
		}

		private void pictureBox12_Click(object sender, EventArgs e)
		{
			pictureBox12.BackColor = culoare_drum_dublu = obtine_culoare(culoare_drum_dublu);
			poze_init();
			event_actualizare_culori?.Invoke(null, null);
			pictureBox_image_sets();
		}

		private void pictureBox14_Click(object sender, EventArgs e)
		{
			pictureBox14.BackColor = culoare_intersectie_inactiva = obtine_culoare(culoare_intersectie_inactiva);
			poze_init();
			event_actualizare_culori?.Invoke(null, null);
		}

		private void pictureBox13_Click(object sender, EventArgs e)
		{
			pictureBox13.BackColor = culoare_intersectie = obtine_culoare(culoare_intersectie);
			poze_init();
			event_actualizare_culori?.Invoke(null, null);
			pictureBox_image_sets();

		}

		private void pictureBox15_Click(object sender, EventArgs e)
		{
			pictureBox15.BackColor = culore_nota_back = obtine_culoare(culore_nota_back);
			event_font_change?.Invoke(null, null);
			pictureBox_image_sets();

		}

		private void pictureBox16_Click(object sender, EventArgs e)
		{
			pictureBox16.BackColor = culore_nota_text = obtine_culoare(culore_nota_text);
			event_font_change?.Invoke(null, null);
		}
		#endregion
        #region butone
		private void textBox1_Click(object sender, EventArgs e)
		{
			FontDialog fnd = new FontDialog();
			if (fnd.ShowDialog() == DialogResult.OK)
			{
				textBox1.Font = nota_fnt = fnd.Font;
				event_font_change?.Invoke(null, null);
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
			toolStripButton1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
		}

		private void ajutorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutBox1().ShowDialog();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			cost();
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			cost();
		}

		private void numericUpDown6_ValueChanged(object sender, EventArgs e)
		{
			cost();
		}

		private void numericUpDown3_ValueChanged(object sender, EventArgs e)
		{
			cost();
		}

		private void numericUpDown4_ValueChanged(object sender, EventArgs e)
		{
			cost();
		}

		public decimal get_pret()
		{
			return numericUpDown5.Value;
		}
        #endregion
        #region nimic important 1
        private void salvareFisierToolStripMenuItem_Click(object sender, EventArgs e)
		{
			matr_map = matr_generea_adiacenta();
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.RestoreDirectory = true;
			sfd.Title = "Salvare sesiune:";
			sfd.Filter = "Drumuinator File(*.drumuinator)| *.drumuinator";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				using (StreamWriter sw = new StreamWriter(sfd.FileName))
				{
					sw.WriteLine(vf_max.X + " " + vf_max.Y);
					sw.WriteLine(matr_map.GetLength(0) + " " + matr_map.GetLength(1));
					for (int i = 0; i < matr_map.GetLength(0); i++)
					{
						for (int j = 0; j < matr_map.GetLength(1); j++)
						{
							sw.Write(matr_map[i, j] + " ");
						}
						sw.WriteLine();
					}
					foreach (Pic p in varfuri)
					{
						if (p != null)
						{
							sw.WriteLine(p.x + " " + p.y);
						}
					}
					sw.WriteLine(culoare_background.R + " " + culoare_background.G + " " + culoare_background.B);
					sw.WriteLine(culoare_intersectie_inactiva.R + " " + culoare_intersectie_inactiva.G + " " + culoare_intersectie_inactiva.B);
					sw.WriteLine(culoare_intersectie.R + " " + culoare_intersectie.G + " " + culoare_intersectie.B);
					sw.WriteLine(culoare_drum_simplu.R + " " + culoare_drum_simplu.G + " " + culoare_drum_simplu.B);
					sw.WriteLine(culoare_drum_dublu.R + " " + culoare_drum_dublu.G + " " + culoare_drum_dublu.B);
					sw.WriteLine(culoare_sageata.R + " " + culoare_sageata.G + " " + culoare_sageata.B);
					sw.WriteLine(culore_nota_back.R + " " + culore_nota_back.G + " " + culore_nota_back.B);
					sw.WriteLine(culore_nota_text.R + " " + culore_nota_text.G + " " + culore_nota_text.B);
					sw.WriteLine(nota_fnt.Name + "|" + (int)nota_fnt.Style + "|" + nota_fnt.Size);
					sw.WriteLine(notes.Count);
					foreach (Nota nnn in notes)
					{
						sw.WriteLine(nnn.Location.X + " " + nnn.Location.Y + "|" + nnn.Size.Width + " " + nnn.Size.Height + "|" + nnn.Text);
					}
					sw.WriteLine(numericUpDown1.Value);
					sw.WriteLine(numericUpDown2.Value);
					sw.WriteLine(numericUpDown6.Value);
					sw.WriteLine(numericUpDown3.Value);
					sw.WriteLine(numericUpDown4.Value);
					sw.WriteLine(numericUpDown5.Value);
				}
			}
		}

		private void deschideFisierExistentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Drumuinator File(*.drumuinator)| *.drumuinator";
			ofd.RestoreDirectory = true;
			ofd.Title = "Incarcare sesiune :";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				if (Path.GetExtension(ofd.FileName) == Program.exte)
				{
					//	MessageBox.Show(ofd.FileName);
					Program.file_name = ofd.FileName;
					Program.refa = true;
					this.Close();
					//   Application.Run(new Form1());
					// Environment.Exit(0);
				}

			}
		}
		void inchidere_check()
		{
			if (MessageBox.Show("Doriti sa salvati proiectul?", "Detali...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				salvareFisierToolStripMenuItem_Click(null, null);
			}
		}
		private void extindereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (splitContainer1.Panel1Collapsed == false) return;
			splitContainer1.Panel1Collapsed = false;
			toolStripButton1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
			toolStrip1.Refresh();
		}

		private void restrangereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (splitContainer1.Panel1Collapsed == true) return;
			splitContainer1.Panel1Collapsed = true;
			toolStripButton1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
			toolStrip1.Refresh();

		}

		private void fisierNouToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.file_name = null;
			Program.refa = true;
			Close();
		}

		private void deconectareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.file_name = null;
			Program.refa = false;
			Close();
			
		}

		private void pictureBox19_Click(object sender, EventArgs e)
		{
			if (clasa.disparitie == 1)
			{
				timer2.Stop();
				Hide();
				activare a = new activare();
				a.ShowDialog();
				this.Show();
				CenterToScreen();
				if (clasa.disparitie == 2)
				{
					pictureBox19.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\verde.png");
				}
				else
				{
					pictureBox19.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Res\\galben.png");
					timer2.Start();

				}

			}
			else 
			if (clasa.disparitie == 0)
			{
				MessageBox.Show("Trebuie să vă conectați mai întai", "Ateentie", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else 
			if (clasa.disparitie == 2)
			{
				MessageBox.Show("Contul este deja activat in versiunea PRO", "PRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		bool prima_rlare = true;
		private void timer2_Tick(object sender, EventArgs e)
		{
			if(prima_rlare )
			{
				prima_rlare = false;
				return;
			}
			Random rnd = new Random();
			int care = rnd.Next(0, 100);
			if (care > 70)
			{
				timer2.Stop();
				new reclama2().ShowDialog();
			}
			else if (care < 69 && care > 40) { timer2.Stop(); new reclama4().ShowDialog(); }
			else if (care < 39 && care > 20) { timer2.Stop(); new reclama3().ShowDialog(); }
			else if (care < 19) { timer2.Stop(); new Reclame().ShowDialog(); }
			else return;
			//if (timer2.Enabled != true) 
			timer2.Start();
		}
        #endregion
        #region lene
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
		//	splitContainer1.Height = Height - toolStrip1.Height;
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{

			//new AboutBox1().Show();
		}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            algoritm = Cautare.A8;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            algoritm = Cautare.Dijkastra;
        }
        #endregion
    }
    #endregion
    //IEnumerator Timp()
    //{
    //    yield return new WaitForSeconds(8f);
    //}
    //IEnumerator SimulateProjectile()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //}
    //public class NumericUpDownEx : NumericUpDown
    //{

    //	public string un = " lei";
    //	public string unitate
    //	{
    //		get
    //		{
    //			return un;
    //		}
    //		set
    //		{
    //			un = value;
    //		}
    //	}
    //	public NumericUpDownEx()
    //	{
    //	}
    //	private bool Debounce = false;
    //	//protected override void ValidateEditText()
    //	//{
    //	//	if (!Debounce) //I had to use a debouncer because any time you update the 'this.Text' field it calls this method.
    //	//	{
    //	//		Debounce = true; //Make sure we don't create a stack overflow.

    //	//		string tempText = this.Text; //Get the text that was put into the box.
    //	//		string numbers = ""; //For holding the numbers we find.
    //	//		bool aparut = false;
    //	//		foreach (char item in tempText) //Implement whatever check wizardry you like here using 'tempText' string.
    //	//		{
    //	//			if (Char.IsDigit(item))
    //	//			{
    //	//				numbers += item;
    //	//			}
    //	//			else
    //	//			{
    //	//				////if(item=='.'&&!aparut)
    //	//				////{
    //	//				////	aparut = true;
    //	//				////	numbers +=',';
    //	//				////}
    //	//				////else
    //	//				break;
    //	//			}
    //	//		}
    //	//	//	MessageBox.Show(numbers);
    //	//		decimal actualNum = decimal.Parse(numbers, System.Globalization.NumberStyles.AllowLeadingSign);
    //	//		if (actualNum > this.Maximum) //Make sure our number is within min/max
    //	//			this.Value = this.Maximum;
    //	//		else if (actualNum < this.Minimum)
    //	//			this.Value = this.Minimum;
    //	//		else
    //	//			this.Value = actualNum;

    //	//		ParseEditText(); //Carry on with the normal checks.
    //	//		UpdateEditText();

    //	//		Debounce = false;
    //	//	}

    //	//}
    //	protected override void UpdateEditText()
    //	{
    //		// Append the units to the end of the numeric value

    //		this.Text = this.Value + unitate;
    //	}
    //}
    public enum Cautare
    {
        Dijkastra=0,
        A8=1,
        Zero = 100
    }
	public enum Imagini
	{
		drum_simplu_orizontal_stanga,
		drum_simplu_orizontal_dreapta,

		drum_simplu_vertical_sus,
		drum_simplu_vertical_jos,

		drum_simplu_diagonala_dreapta_sus,
		drum_simplu_diagonala_dreapta_jos,

		drum_simplu_diagonala_stanga_sus,
		drum_simplu_diagonala_stanga_jos,

		//drum_dublu_orizontal,
		//drum_dublu_vertical,

		drum_dublu_diagonala_dreapta,
		drum_dublu_diagonala_stanga,

		intersectie,
		intersectie_inactiva,

		inv_drum_simplu_orizontal_stanga,
		inv_drum_simplu_orizontal_dreapta,

		inv_drum_simplu_vertical_sus,
		inv_drum_simplu_vertical_jos,

		inv_drum_simplu_diagonala_dreapta_sus,
		inv_drum_simplu_diagonala_dreapta_jos,

		inv_drum_simplu_diagonala_stanga_sus,
		inv_drum_simplu_diagonala_stanga_jos,

		//drum_dublu_orizontal,
		//drum_dublu_vertical,

		inv_drum_dublu_diagonala_dreapta,
		inv_drum_dublu_diagonala_stanga,

		inv_intersectie
	}
	public enum Rotati_Masina
	{
		orizontal_stanga,
		orizontal_dreapta,

		vertical_sus,
		vertical_jos,

		diagonala_dreapta_sus,
		diagonala_dreapta_jos,

		diagonala_stanga_sus,
		diagonala_stanga_jos,
	}
	public enum Masini
	{
		car1,//masina alba
		car2,//albastra
		car3,//orange
		car4,//rosie
		car5,//verde
		car6,//gri
		fantoma
	}
	public struct coord
	{
		public int x, y;
		public coord(int X, int Y)
		{
			x = X;
			y = Y;
		}

	}
	public class Pic : PictureBox
	{
		const int dif_enum = 12;
		#region codenervant
		public coord i, f;//capetele drumului
		public List<Pic> drum_diag;//lista drumurilor diagonale 
		public List<Pic> drum_diagfaraacces;
		public List<Pic> drum;//drumurile peverticala orizontala
		public List<Pic> drumfaraacces;
		public Imagini tim;//tipul imagini
		public int x, y;//coordonatele in matrici
		public int eticheta = 0;//varfului sau tipul drumului 1 drum simoplu 2 drum dublu 3 dru simplu diag 4 drum dubludiag
		public void Modificare(object sender, EventArgs e)
		{
			this.Visible = false;
		}
		public void Aprindere(object sender, EventArgs e)
		{
			this.Visible = true;
		}
		public void Set(int X, int Y)
		{
			x = X;
			y = Y;
		}
		public void SetV(int X, int Y, int Eticheta)
		{
			x = X;
			y = Y;
			eticheta = Eticheta;
		}
		public void setif(Pic I, Pic F)
		{
			i = new coord(I.x, I.y);
			f = new coord(F.x, F.y);
		}
		#endregion
		public void Actualizare_Culori(object sender, EventArgs e)
		{
			if (eticheta < 5 && eticheta > 0)
			{
				if (eticheta == 1)
				{
					BackColor = MainForm.culoare_drum_simplu;
					BackgroundImage =MainForm.imag[tim];
				}
				else if (eticheta == 2) BackColor =MainForm.culoare_drum_dublu;
				else BackgroundImage =MainForm.imag[tim];
			}
			else
				BackgroundImage =MainForm.imag[tim];

		}
		public void Inversare_Culori()
		{
			if (eticheta < 5)
			{
				if (eticheta == 1)
				{
					BackColor =MainForm.inv_culoare_drum_simplu;
					BackgroundImage =MainForm.imag[tim + dif_enum];
				}
				else if (eticheta == 2) BackColor =MainForm.inv_culoare_drum_dublu;
				else BackgroundImage =MainForm.imag[tim + dif_enum];
			}
			else
				BackgroundImage =MainForm.imag[tim + dif_enum];

		}
		public Pic drumu_dela_xlay(Pic end)
		{
			if (end == null) return null;
			if ((y == end.y) || (x == end.x))
			{
				foreach (Pic c in drum)
				{
					if (c.eticheta == 1)
					{
						if (MainForm.coord_egale(c.f, end))
						{
							return c;
						}
					}
					else if (MainForm.coord_egale(c.f, end) ||MainForm.coord_egale(c.i, end))
					{
						return c;
					}
				}
			}
			else
			{
				foreach (Pic c in drum_diag)
				{
					if (c.eticheta == 3)
					{
						if (MainForm.coord_egale(c.f, end))
						{
							return c;
						}

					}
					else if (MainForm.coord_egale(c.f, end) ||MainForm.coord_egale(c.i, end))
					{
						return c;
					}

				}
			}

			return null;
		}
	}
	public class Masina : PictureBox
	{
		const int dif_enum = 12;
		public readonly double rad2 = Math.Sqrt(2);
		public bool afiseaza;
		public Image img;
		public Pic cur = null;
		public Pic next = null;
		public Pic final;
		public List<Point> drumuri;
		public bool ExistaDrum;
		public static int speed = 2;
		public decimal consum;
		public decimal va_consuma;
		public SearchParameters sp;
		public PathFinder pf;
		public bool se_misca = false;
		int care_maisna = 0;
        public Cautare cautare = Cautare.Dijkastra;
		public Masina(Pic start, int img, Panel parent, Size siz,Cautare cautare)
		{
			Location = new Point(start.Location.X + (MainForm.IMAG_Inter_SIZE - siz.Width) / 2, start.Location.Y + (MainForm.IMAG_Inter_SIZE - siz.Height) / 2);
			Size = siz;
			//BackColor = Color.Transparent;
			Image =MainForm.car[(Masini)(img - 1)];
			care_maisna = img;
			SizeMode = PictureBoxSizeMode.Zoom;
			//ImageLayout = ImageLayout.Zoom;
			//BackgroundImage = img;
			BackgroundImageLayout = ImageLayout.Zoom;
			parent.Controls.Add(this);
			Visible = true;
			BringToFront();
			cur = start;
			ExistaDrum = false;
			afiseaza = false;
            this.cautare = Cautare.Zero;
		}
		int da;
		public decimal cost = 0;
		public int val_cur = 0;
		public void Vireaza(Pic pnt1, Pic pnt2)
		{

			if (pnt1.x == pnt2.x)
			{
				BackgroundImage = null;
				Size =MainForm.Car_Size;
				if (pnt1.y > pnt2.y)//mere stanga;
					Image =MainForm.masina_img[care_maisna][Rotati_Masina.orizontal_stanga];
				else //mere dreapta;
					Image =MainForm.masina_img[care_maisna][Rotati_Masina.orizontal_dreapta];
				return;
			}
			else if (pnt1.y == pnt2.y)
			{
				BackgroundImage = null;
				Size = new Size(MainForm.Car_Size.Height,MainForm.Car_Size.Width);
				if (pnt1.x > pnt2.x) //mere sus;
					Image =MainForm.masina_img[care_maisna][Rotati_Masina.vertical_sus];
				else //mere jos;
					Image =MainForm.masina_img[care_maisna][Rotati_Masina.vertical_jos];
				return;
			}
			else
			{
				if (afiseaza) da = dif_enum;
				else da = 0;
				Size =MainForm.car_size45;
				if (pnt1.Location.X < pnt2.Location.X)
				{

					if (pnt1.Location.Y < pnt2.Location.Y)//mere dreapta jos;
					{
						Image =MainForm.masina_img[care_maisna][Rotati_Masina.diagonala_dreapta_jos];
						if (drum_dublu) BackgroundImage =MainForm.imag[Imagini.drum_dublu_diagonala_stanga + da];
						else BackgroundImage =MainForm.imag[Imagini.drum_simplu_diagonala_stanga_jos + da];
						return;
					}
					else//mere dreapta sus;
					{
						Image =MainForm.masina_img[care_maisna][Rotati_Masina.diagonala_dreapta_sus];
						if (drum_dublu) BackgroundImage =MainForm.imag[Imagini.drum_dublu_diagonala_dreapta + da];
						else BackgroundImage =MainForm.imag[Imagini.drum_simplu_diagonala_dreapta_jos + da];

						return;

					}
				}
				else
				{
					if (pnt1.Location.Y < pnt2.Location.Y) //mere stanga jos
					{
						Image =MainForm.masina_img[care_maisna][Rotati_Masina.diagonala_stanga_jos];
						if (drum_dublu) BackgroundImage =MainForm.imag[Imagini.drum_dublu_diagonala_dreapta + da];
						else BackgroundImage =MainForm.imag[Imagini.drum_simplu_diagonala_dreapta_jos + da];
						return;
					}
					else//mere stanga sus; 
					{
						Image =MainForm.masina_img[care_maisna][Rotati_Masina.diagonala_stanga_sus];
						if (drum_dublu) BackgroundImage =MainForm.imag[Imagini.drum_dublu_diagonala_stanga + da];
						else BackgroundImage =MainForm.imag[Imagini.drum_simplu_diagonala_stanga_jos + da];
						return;

					}
				}
			}
		}
		//		if (pictureBox1.Bounds.IntersectsWith(pictureBox2.Bounds))
		//{		
		//    //They have collided
		//}
		void cazuri(Pic pnt1, Pic pnt2, int spd)
		{
			if (pnt1.x == pnt2.x)
			{
				if (pnt1.y > pnt2.y)//mere stanga;
				{
					// Location.Offset(-spd, 0);
					Left -= spd;
					//Location = new Point(Location.X - spd, Location.Y);
					return;
				}
				else //mere dreapta;
				{
					//Location.Offset(+spd, 0);
					Left += spd;
					//Location = new Point(Location.X + spd, Location.Y); 
					return;


				}
			}
			else if (pnt1.y == pnt2.y)
			{
				if (pnt1.x > pnt2.x) //mere sus;
				{
					//Location.Offset(0,-spd);
					Top -= spd;
					//Location = new Point(Location.X, Location.Y - spd);
					return;

				}
				else //mere jos;
				{
					//Location.Offset(0,spd);
					Top += spd;
					//Location = new Point(Location.X, Location.Y + spd);
					return;
				}
			}
			else
			{
				if (pnt1.Location.X < pnt2.Location.X)
				{
					if (pnt1.Location.Y < pnt2.Location.Y)//mere dreapta jos;
					{
						//Location.Offset(spd, spd);
						Top += spd;
						Left += spd;
						//Location = new Point(Location.X + spd, Location.Y + spd); 
						return;
						//	MessageBox.Show("dc");

					}
					else//mere dreapta sus;
					{
						//MessageBox.Show("ce plm");
						//Location.Offset(spd, -spd);
						Top -= spd;
						Left += spd;
						//Location = new Point(Location.X + spd, Location.Y-spd);
						return;

					}
				}
				else
				{
					if (pnt1.Location.Y < pnt2.Location.Y) //mere stanga jos
					{
						//Location.Offset(-spd, spd);
						//Location = new Point(Location.X-spd, Location.Y + spd); 
						Top += spd;
						Left -= spd;
						return;
					}
					else//mere stanga sus; 
					{
						//Location.Offset(-spd, -spd);
						Top -= spd;
						Left -= spd;
						//Location = new Point(Location.X-spd, Location.Y - spd);
						return;

					}
				}
			}
		}
		int distance(PictureBox pnt1, PictureBox pnt2)
		{
			return (int)Math.Sqrt((pnt1.Location.X - pnt2.Location.X) * (pnt1.Location.X - pnt2.Location.X) + (pnt1.Location.Y - pnt2.Location.Y) * (pnt1.Location.Y - pnt2.Location.Y));
		}
		void set_center(Pic loc)
		{
			Location = new Point(loc.Location.X + (MainForm.IMAG_Inter_SIZE - Width) / 2, loc.Location.Y + (MainForm.IMAG_Inter_SIZE - Height) / 2);
		}
		bool e_drept(Pic a, Pic b)
		{
			return (a.x == b.x) || (a.y == b.y);
		}


		int cat = 0;
		double cat_cur = 0;
		double spd;
		int diag_est;
		double vitgula = 0;
		double aux;
		bool drum_dublu;
		int max = 0;
		public void Misca(object sender, EventArgs e)
		{
			if (drumuri != null && drumuri.Count > 0)
			{
				if (cur == null || next == null)//deobicei la prima rulare
				{
					next = MainForm.varfuri[drumuri[0].X, drumuri[0].Y];//seteza urmatoare 'statie' cu intersectia data de coordonatele drumului
					cat = distance(cur, next);//calculeaza distanta intre locatia curenta si urmatoarea
					Vireaza(cur, next);//funtia care schimba imaginile aplicatiei in functie de tipul de drum
					set_center(cur);//seteaza in centrul imagini curente
					cat_cur = 0;//distanta parcursa
					vitgula = 0;//la deplsarea pe verticala calculelel dau cu virgula
				}
				se_misca = true;//folosita pentru mai multe verificari  
				if (cat_cur >= cat)//daca nu a ajuns la destinatie
				{
					if (afiseaza)//daca ea este selecata
					{
						cur.Actualizare_Culori(null, null);//face culorile normale
						cur.drumu_dela_xlay(next).Actualizare_Culori(null, null);//face culorile normale la drumul dintre puncte
					}
					cur = next;
					drumuri.RemoveAt(0);//eliminam din lista primu drum
					val_cur++;//creste progresul 
					if (afiseaza) Program.fmc.toolStripProgressBar1.Value = val_cur;//modifica progressbar-ul daca ea este selectata 
					if (drumuri.Count == 0 || drumuri == null)//daca nu mai sunt drumuri
					{
						se_misca = false; 
						if (afiseaza) Program.fmc.toolStripProgressBar1.Value = 0;
						afiseaza = false; Vireaza(cur, cur); set_center(cur);//setam la mijloc dupa ce setam masina in pozitia standar
						return;
					}
					next = MainForm.varfuri[drumuri[0].X, drumuri[0].Y];//luam urmatoare 'statie'
					drum_dublu = cur.drumu_dela_xlay(next).eticheta == 4;//verificam daca nu este drum dublu pentru functie Vireza()
					cat = distance(cur, next);//calculam distanta
					Vireaza(cur, next);//funtia care schimba imaginile aplicatiei in functie de tipul de drum
					set_center(cur);//seteaza in centrum imagini
					cat_cur = 0;//cat mai avem
					vitgula = 0;//pediagonala da cu vrgula

				}
				if (speed != 0)//daca tarckbarul nu are valoare 0
				{
					spd = speed;
					if (!e_drept(cur, next))//daca este drum pe diagonala
					{
						aux = speed / rad2;//viteaza/ radical din 2
						diag_est = (int)aux;//distanta pe care o va parcurge pentruca totul trebuie sa fie int
						vitgula += aux - Math.Truncate(aux);//calculam marja de eroare
						if (vitgula >= 1)
						{
							diag_est++;
							vitgula -= 1;
						}
						if (diag_est == 0) { diag_est = 1; spd = 2.4f; }// pentruca se mai ajunge
					}
					else diag_est = speed;//daca nu este pe diagonala eeste doar viteza
					cazuri(cur, next, diag_est);//functia care misca masina
					cat_cur += spd;//distanta parcursa
				}
			}
		}
		public void gaseste_drum(Pic end)
		{
			init_culori(null, null);// readucerea tuturor drumurilor la culorile normale
			final = end;
			if (MainForm.varfuri[cur.x, cur.y] == null) { MORTE(); return; } //daca sa sters intersectia unde ne aflam, sa se sterga si masina 
                                                                             //trimite parametri pentru algoritmul de cautare al drumului (sursa, dest, matricea cu varfurile existente)
            if (cautare == Cautare.Zero) cautare = MainForm.algoritm;
			sp = new SearchParameters(new Point(cur.x, cur.y), new Point(end.x, end.y),MainForm.genereaza_matr_bool(),cautare);
			//se initializeza algoritmul de cautare
			pf = new PathFinder(sp);
			//verificare, caci mai crapa din cand in cand
			if (drumuri != null) drumuri.Clear();
			// se cauta drumul si il saveza in lista drumuri
			drumuri = pf.FindPath();
			if (drumuri.Count > 0) ExistaDrum = true;
			else{
				MessageBox.Show("NU exista un drum intre cele 2 puncte!", "Detali...", MessageBoxButtons.OK, MessageBoxIcon.Information);
				MORTE(); return;}
			//daca sa modificat harta si algortimul de cautare a gasit alt drum decat cel initial 
			if (ExistaDrum && next != null && next !=MainForm.varfuri[drumuri[0].X, drumuri[0].Y])
			{
				//daca nu a mers prea departe de intersectie va intoarce
				if (distance(this, cur) < 15)
				{
					next = MainForm.varfuri[drumuri[0].X, drumuri[0].Y];
					cat = distance(cur, next);
					Vireaza(cur, next);//functia de deplasare a masini
					set_center(cur);// pune masinia in mijlocul intersectiei
					cat_cur = 0;vitgula = 0;
				}
				else//dacaam mers prea departe vom cauta drumul pornind de la urmatorul drum  
				{
					Masina aua = new Masina(next, 7, Parent as Panel, new Size(0, 0),cautare);//creem o masina fantoma care se va deplasa
					aua.gaseste_drum(final);
					if (aua.ExistaDrum)
					{
						drumuri.Clear();
						drumuri = aua.drumuri;
						drumuri.Insert(0, new Point(next.x, next.y));
					}
					aua.Dispose();
				}
			}
			if (ExistaDrum)
			{
				//calculeaza costul deplasari
				max = drumuri.Count + 1;
				va_consuma = 0;
				if (drumuri.Count > 1)
				{
					Point last = new Point(cur.x, cur.y);
					for (int i = 0; i < drumuri.Count; i++)
					{
						if (last.X == drumuri[i].X || last.Y == drumuri[i].Y)	va_consuma += consum;
						else va_consuma += consum * (decimal)rad2;
						last = drumuri[i];
					}
				}
				val_cur = 1;//unde se situeaza bara de progres
				cost = Program.fmc.get_pret() * va_consuma;
				cost = Math.Round(cost, 2);
			}
		}
		public void schimbare_harta(object sender, EventArgs e)
		{
			if (cur == final) return;
			init_culori(null, null);
			if (next != null &&MainForm.varfuri[next.x, next.y] == null)
			{
				MORTE();
				return;
			}
			if (cur != null &&MainForm.varfuri[cur.x, cur.y] == null)
			{
				MORTE();
				return;
			}
			if ((se_misca) && cur.drumu_dela_xlay(next) == null)
			{
				MORTE();
				return;
			}

			BringToFront();
			if (final != null)
				gaseste_drum(final);
		}
		public void arata_drum()
		{
			afiseaza = true;
			Program.fmc.toolStripTextBox2.Text = cost + " Ron";
			Pic aux;
			if (drumuri == null || drumuri.Count <= 0) return;

			cur?.Inversare_Culori();
			cur?.drumu_dela_xlay(MainForm.varfuri[drumuri[0].X, drumuri[0].Y]).Inversare_Culori();

			MainForm.varfuri[drumuri[0].X, drumuri[0].Y].Inversare_Culori();
			for (int i = 1; i < drumuri.Count; i++)
			{
				aux =MainForm.varfuri[drumuri[i - 1].X, drumuri[i - 1].Y];
				aux.Inversare_Culori();
				aux.drumu_dela_xlay(MainForm.varfuri[drumuri[i].X, drumuri[i].Y]).Inversare_Culori();
			}

			Program.fmc.toolStripProgressBar1.Maximum = max;
			Program.fmc.toolStripProgressBar1.Value = val_cur;
		}

		public void init_culori(object sender, EventArgs e)
		{
			if (afiseaza)
			{
				Program.fmc.toolStripTextBox2.Text = ""; Program.fmc.toolStripProgressBar1.Value = 0;
				afiseaza = false;
				Pic aux;
				if (cur == null)
				{
					if (drumuri == null || drumuri.Count <= 0) return;
				}
				else
				{
					cur.Actualizare_Culori(null, null);
					if (drumuri == null || drumuri.Count == 0) return;
					cur.drumu_dela_xlay(MainForm.varfuri[drumuri[0].X, drumuri[0].Y])?.Actualizare_Culori(null, null);
					MainForm.varfuri[drumuri[0].X, drumuri[0].Y]?.Actualizare_Culori(null, null);
				}
				for (int i = 1; i < drumuri.Count; i++)
				{
					aux =MainForm.varfuri[drumuri[i - 1].X, drumuri[i - 1].Y];
					if (aux == null) continue;

					aux.Actualizare_Culori(null, null);
					aux.drumu_dela_xlay(MainForm.varfuri[drumuri[i].X, drumuri[i].Y])?.Actualizare_Culori(null, null);
				}

			}
		}
		public void MORTE()
		{
            MainForm.event_misca -= Misca;
			MainForm.cars.Remove(this);
			//	MainForm.panel2.Controls.Remove(this);
			MainForm.event_modificare_harta -= schimbare_harta;
			afiseaza = false;
			Dispose();

		}
	}
	public class Nota : TextBox
	{
		public void bring(object sender, EventArgs e)
		{
			BringToFront();
		}
		public void fnt_chng(object sender, EventArgs e)
		{
			BackColor =MainForm.culore_nota_back;
			ForeColor =MainForm.culore_nota_text;
			Font =MainForm.nota_fnt;
		}
		Point mousedownloc = Point.Empty;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			BringToFront();
			base.OnMouseDown(e);
			mousedownloc = e.Location;
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (mousedownloc != Point.Empty)
			{
				Left = e.X + Left - mousedownloc.X;
				Top = e.Y + Top - mousedownloc.Y;
			}
		}
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			mousedownloc = Point.Empty;
		}

	}
}
