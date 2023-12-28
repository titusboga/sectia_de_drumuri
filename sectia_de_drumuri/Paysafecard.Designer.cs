namespace Sectia_de_drumuri
{
    partial class Paysafecard
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Paysafecard));
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.label17 = new System.Windows.Forms.Label();
			this.textBox12 = new System.Windows.Forms.TextBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.SuspendLayout();
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(229, 188);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(43, 13);
			this.linkLabel2.TabIndex = 47;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "termenii";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Location = new System.Drawing.Point(154, 187);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(191, 17);
			this.checkBox6.TabIndex = 46;
			this.checkBox6.Text = "Da, accept                pyasafe card.";
			this.checkBox6.UseVisualStyleBackColor = true;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(201, 133);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(110, 13);
			this.label17.TabIndex = 45;
			this.label17.Text = "Plata cu paysafecard:";
			// 
			// textBox12
			// 
			this.textBox12.Location = new System.Drawing.Point(119, 149);
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new System.Drawing.Size(261, 20);
			this.textBox12.TabIndex = 44;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
			this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBox2.Location = new System.Drawing.Point(12, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(475, 89);
			this.pictureBox2.TabIndex = 48;
			this.pictureBox2.TabStop = false;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(172, 243);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(208, 42);
			this.button1.TabIndex = 49;
			this.button1.Text = "Confirm\r\n";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox4
			// 
			this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
			this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pictureBox4.Location = new System.Drawing.Point(119, 243);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(47, 42);
			this.pictureBox4.TabIndex = 56;
			this.pictureBox4.TabStop = false;
			this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
			// 
			// Paysafecard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(499, 313);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.checkBox6);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.textBox12);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Paysafecard";
			this.Text = "Paysafecard";
			this.Load += new System.EventHandler(this.Paysafecard_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox4;
	}
}