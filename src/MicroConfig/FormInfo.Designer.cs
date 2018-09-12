namespace MicroConfig
{
	partial class FormInfo
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.pictureBoxImage = new System.Windows.Forms.PictureBox();
			this.textBoxText = new System.Windows.Forms.TextBox();
			this.buttonCopyToClipboard = new System.Windows.Forms.Button();
			this.linkLabelFullInfo = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(266, 231);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOkClick);
			// 
			// pictureBoxImage
			// 
			this.pictureBoxImage.Image = global::MicroConfig.Properties.Resources.ok;
			this.pictureBoxImage.Location = new System.Drawing.Point(12, 12);
			this.pictureBoxImage.Name = "pictureBoxImage";
			this.pictureBoxImage.Size = new System.Drawing.Size(64, 64);
			this.pictureBoxImage.TabIndex = 1;
			this.pictureBoxImage.TabStop = false;
			// 
			// textBoxText
			// 
			this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxText.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBoxText.Location = new System.Drawing.Point(96, 12);
			this.textBoxText.Multiline = true;
			this.textBoxText.Name = "textBoxText";
			this.textBoxText.ReadOnly = true;
			this.textBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxText.Size = new System.Drawing.Size(404, 213);
			this.textBoxText.TabIndex = 3;
			// 
			// buttonCopyToClipboard
			// 
			this.buttonCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCopyToClipboard.Location = new System.Drawing.Point(347, 231);
			this.buttonCopyToClipboard.Name = "buttonCopyToClipboard";
			this.buttonCopyToClipboard.Size = new System.Drawing.Size(75, 23);
			this.buttonCopyToClipboard.TabIndex = 2;
			this.buttonCopyToClipboard.Text = "В буфер";
			this.buttonCopyToClipboard.UseVisualStyleBackColor = true;
			this.buttonCopyToClipboard.Click += new System.EventHandler(this.ButtonCopyToClipboardClick);
			// 
			// linkLabelFullInfo
			// 
			this.linkLabelFullInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabelFullInfo.AutoSize = true;
			this.linkLabelFullInfo.Location = new System.Drawing.Point(428, 236);
			this.linkLabelFullInfo.Name = "linkLabelFullInfo";
			this.linkLabelFullInfo.Size = new System.Drawing.Size(72, 13);
			this.linkLabelFullInfo.TabIndex = 4;
			this.linkLabelFullInfo.TabStop = true;
			this.linkLabelFullInfo.Text = "Подробнее...";
			this.linkLabelFullInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelFullInfoLinkClicked);
			// 
			// FormInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(512, 266);
			this.Controls.Add(this.linkLabelFullInfo);
			this.Controls.Add(this.buttonCopyToClipboard);
			this.Controls.Add(this.textBoxText);
			this.Controls.Add(this.pictureBoxImage);
			this.Controls.Add(this.buttonOK);
			this.Name = "FormInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "RPD MicroConfig Информация";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.PictureBox pictureBoxImage;
		private System.Windows.Forms.TextBox textBoxText;
		private System.Windows.Forms.Button buttonCopyToClipboard;
		private System.Windows.Forms.LinkLabel linkLabelFullInfo;
	}
}