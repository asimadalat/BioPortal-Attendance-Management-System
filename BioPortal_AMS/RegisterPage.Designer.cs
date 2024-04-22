namespace BioPortal_AMS
{
    partial class RegisterPage
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
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 31, 31);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(899, 149);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo_bioportaldarkm;
            pictureBox1.Location = new Point(331, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(264, 92);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Metropolis Medium", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ControlLightLight;
            label2.Location = new Point(349, 181);
            label2.Name = "label2";
            label2.Size = new Size(222, 48);
            label2.TabIndex = 3;
            label2.Text = "Register";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(55, 55, 55);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = SystemColors.ControlLightLight;
            textBox1.Location = new Point(360, 260);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = " Institution Code";
            textBox1.Size = new Size(326, 46);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(55, 55, 55);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.ForeColor = SystemColors.ControlLightLight;
            textBox2.Location = new Point(360, 335);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = " Institution";
            textBox2.Size = new Size(326, 46);
            textBox2.TabIndex = 5;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(45, 45, 45);
            button1.BackgroundImage = Properties.Resources.HidePass;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(45, 45, 45);
            button1.Location = new Point(639, 566);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(43, 33);
            button1.TabIndex = 6;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.Cursor = Cursors.Hand;
            button2.Font = new Font("Tahoma", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(370, 646);
            button2.Name = "button2";
            button2.Size = new Size(155, 71);
            button2.TabIndex = 7;
            button2.Text = " ➤";
            button2.TextAlign = ContentAlignment.TopCenter;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(21, 750);
            label1.Name = "label1";
            label1.Size = new Size(112, 29);
            label1.TabIndex = 8;
            label1.Text = "< Back";
            label1.Click += label1_Click;
            // 
            // textBox3
            // 
            textBox3.BackColor = Color.FromArgb(55, 55, 55);
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox3.ForeColor = SystemColors.ControlLightLight;
            textBox3.Location = new Point(360, 410);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = " Email";
            textBox3.Size = new Size(326, 46);
            textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            textBox4.BackColor = Color.FromArgb(55, 55, 55);
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox4.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox4.ForeColor = SystemColors.ControlLightLight;
            textBox4.Location = new Point(360, 485);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = " Fullname";
            textBox4.Size = new Size(326, 46);
            textBox4.TabIndex = 10;
            // 
            // textBox5
            // 
            textBox5.BackColor = Color.FromArgb(55, 55, 55);
            textBox5.BorderStyle = BorderStyle.FixedSingle;
            textBox5.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox5.ForeColor = SystemColors.ControlLightLight;
            textBox5.Location = new Point(360, 560);
            textBox5.Name = "textBox5";
            textBox5.PasswordChar = '*';
            textBox5.PlaceholderText = " Password";
            textBox5.Size = new Size(273, 46);
            textBox5.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = Cursors.Hand;
            label3.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ControlLightLight;
            label3.Location = new Point(107, 271);
            label3.Name = "label3";
            label3.Size = new Size(247, 29);
            label3.TabIndex = 12;
            label3.Text = "Institution Code";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = Cursors.Hand;
            label4.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ControlLightLight;
            label4.Location = new Point(193, 346);
            label4.Name = "label4";
            label4.Size = new Size(161, 29);
            label4.TabIndex = 13;
            label4.Text = "Institution";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Cursor = Cursors.Hand;
            label5.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ControlLightLight;
            label5.Location = new Point(262, 421);
            label5.Name = "label5";
            label5.Size = new Size(92, 29);
            label5.TabIndex = 14;
            label5.Text = "Email";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Cursor = Cursors.Hand;
            label6.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = SystemColors.ControlLightLight;
            label6.Location = new Point(208, 496);
            label6.Name = "label6";
            label6.Size = new Size(146, 29);
            label6.TabIndex = 15;
            label6.Text = "Fullname";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Cursor = Cursors.Hand;
            label7.Font = new Font("Metropolis Medium", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ControlLightLight;
            label7.Location = new Point(199, 571);
            label7.Name = "label7";
            label7.Size = new Size(155, 29);
            label7.TabIndex = 16;
            label7.Text = "Password";
            // 
            // RegisterPage
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(900, 800);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ButtonHighlight;
            FormBorderStyle = FormBorderStyle.None;
            Name = "RegisterPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RegisterPage";
            Load += RegisterPage_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}