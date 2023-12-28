namespace Sectia_de_drumuri
{
    partial class conectarecs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conectarecs));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.customGrpBox1 = new Sectia_de_drumuri.CustomGrpBox();
            this.customGrpBox2 = new Sectia_de_drumuri.CustomGrpBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.customGrpBox1.SuspendLayout();
            this.customGrpBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(378, 20);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 18);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(378, 20);
            this.textBox2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(268, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(206, 50);
            this.button1.TabIndex = 4;
            this.button1.Text = "Conectare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(84, 222);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 50);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // customGrpBox1
            // 
            this.customGrpBox1.Controls.Add(this.textBox1);
            this.customGrpBox1.Location = new System.Drawing.Point(84, 71);
            this.customGrpBox1.Name = "customGrpBox1";
            this.customGrpBox1.Size = new System.Drawing.Size(391, 44);
            this.customGrpBox1.TabIndex = 9;
            this.customGrpBox1.TabStop = false;
            this.customGrpBox1.Text = "Username sau email";
            // 
            // customGrpBox2
            // 
            this.customGrpBox2.Controls.Add(this.textBox2);
            this.customGrpBox2.Location = new System.Drawing.Point(84, 130);
            this.customGrpBox2.Name = "customGrpBox2";
            this.customGrpBox2.Size = new System.Drawing.Size(391, 44);
            this.customGrpBox2.TabIndex = 10;
            this.customGrpBox2.TabStop = false;
            this.customGrpBox2.Text = "Parola";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(144, 222);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 50);
            this.button2.TabIndex = 8;
            this.button2.Text = "Am uitat parola";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // conectarecs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(582, 357);
            this.Controls.Add(this.customGrpBox2);
            this.Controls.Add(this.customGrpBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "conectarecs";
            this.Text = "Conectare";
            this.Load += new System.EventHandler(this.conectarecs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.customGrpBox1.ResumeLayout(false);
            this.customGrpBox1.PerformLayout();
            this.customGrpBox2.ResumeLayout(false);
            this.customGrpBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
        private CustomGrpBox customGrpBox1;
        private CustomGrpBox customGrpBox2;
        private System.Windows.Forms.Button button2;
    }
}