namespace Payor_Recipient_Information
{
    partial class Payor_Recipient_Information
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Payor_Recipient_Information));
            browseButton = new Button();
            label1 = new Label();
            processButton = new Button();
            exitButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            destinationButton = new Button();
            toolTip1 = new ToolTip(components);
            infoIcon = new PictureBox();
            helpButton = new Button();
            loadingCircle = new PictureBox();
            textBox1 = new TextBox();
            saveLocationLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)infoIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)loadingCircle).BeginInit();
            SuspendLayout();
            // 
            // browseButton
            // 
            browseButton.Location = new Point(426, 94);
            browseButton.Name = "browseButton";
            browseButton.Size = new Size(75, 23);
            browseButton.TabIndex = 1;
            browseButton.Text = "Browse...";
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += BrowseButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(75, 69);
            label1.Name = "label1";
            label1.Size = new Size(131, 19);
            label1.TabIndex = 2;
            label1.Text = "Upload LIS File";
            // 
            // processButton
            // 
            processButton.BackColor = Color.White;
            processButton.Font = new Font("Tahoma", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            processButton.Location = new Point(160, 194);
            processButton.Name = "processButton";
            processButton.Size = new Size(248, 48);
            processButton.TabIndex = 3;
            processButton.Text = "Process";
            processButton.UseVisualStyleBackColor = false;
            processButton.Click += ProcessButton_Click;
            // 
            // exitButton
            // 
            exitButton.Location = new Point(472, 288);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(80, 32);
            exitButton.TabIndex = 4;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.InitialDirectory = "S:\\it_apps\\SUBPOENA";
            openFileDialog1.Title = "Select Subpoena Document";
            // 
            // destinationButton
            // 
            destinationButton.Location = new Point(200, 144);
            destinationButton.Name = "destinationButton";
            destinationButton.Size = new Size(168, 23);
            destinationButton.TabIndex = 5;
            destinationButton.Text = "Select Custom Destination...";
            destinationButton.UseVisualStyleBackColor = true;
            destinationButton.Click += DestinationButton_Click;
            // 
            // infoIcon
            // 
            infoIcon.BackColor = Color.Transparent;
            infoIcon.Image = (Image)resources.GetObject("infoIcon.Image");
            infoIcon.Location = new Point(368, 144);
            infoIcon.Name = "infoIcon";
            infoIcon.Size = new Size(25, 23);
            infoIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            infoIcon.TabIndex = 6;
            infoIcon.TabStop = false;
            toolTip1.SetToolTip(infoIcon, "By default, the generated Excel document is saved in the same location as the loaded LIS file.");
            // 
            // helpButton
            // 
            helpButton.Location = new Point(8, 288);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(80, 31);
            helpButton.TabIndex = 7;
            helpButton.Text = "Help";
            helpButton.UseVisualStyleBackColor = true;
            helpButton.Click += HelpButton_Click;
            // 
            // loadingCircle
            // 
            loadingCircle.BackColor = Color.Transparent;
            loadingCircle.BackgroundImageLayout = ImageLayout.Stretch;
            loadingCircle.Image = (Image)resources.GetObject("loadingCircle.Image");
            loadingCircle.Location = new Point(250, 248);
            loadingCircle.Name = "loadingCircle";
            loadingCircle.Size = new Size(68, 69);
            loadingCircle.TabIndex = 8;
            loadingCircle.TabStop = false;
            loadingCircle.Visible = false;
            // 
            // textBox1
            // 
            textBox1.AcceptsReturn = true;
            textBox1.AllowDrop = true;
            textBox1.Enabled = false;
            textBox1.Location = new Point(50, 95);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.RightToLeft = RightToLeft.Yes;
            textBox1.Size = new Size(370, 23);
            textBox1.TabIndex = 9;
            // 
            // saveLocationLabel
            // 
            saveLocationLabel.BackColor = Color.Transparent;
            saveLocationLabel.ForeColor = SystemColors.ButtonShadow;
            saveLocationLabel.Location = new Point(60, 169);
            saveLocationLabel.Name = "saveLocationLabel";
            saveLocationLabel.Size = new Size(451, 15);
            saveLocationLabel.TabIndex = 10;
            saveLocationLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // Payor_Recipient_Information
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(561, 329);
            Controls.Add(saveLocationLabel);
            Controls.Add(textBox1);
            Controls.Add(loadingCircle);
            Controls.Add(helpButton);
            Controls.Add(infoIcon);
            Controls.Add(destinationButton);
            Controls.Add(exitButton);
            Controls.Add(processButton);
            Controls.Add(label1);
            Controls.Add(browseButton);
            Name = "Payor_Recipient_Information";
            Text = "DT529PAYOR Information Proccessing";
            Load += Payor_Recipient_Information_Load;
            ((System.ComponentModel.ISupportInitialize)infoIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)loadingCircle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button browseButton;
        private Label label1;
        private Button processButton;
        private Button exitButton;
        private OpenFileDialog openFileDialog1;
        private Button destinationButton;
        private ToolTip toolTip1;
        private PictureBox infoIcon;
        private Button helpButton;
        private PictureBox loadingCircle;
        private TextBox textBox1;
        private Label saveLocationLabel;
    }
}
