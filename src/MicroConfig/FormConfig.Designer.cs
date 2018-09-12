using System.Windows.Forms;

namespace MicroConfig
{
	partial class FormConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
			this.comboBoxDrives = new System.Windows.Forms.ComboBox();
			this.buttonClearAll = new System.Windows.Forms.Button();
			this.labelDevName = new System.Windows.Forms.Label();
			this.buttonRefreshDevs = new System.Windows.Forms.Button();
			this.statusStripMain = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBarMain = new System.Windows.Forms.ToolStripProgressBar();
			this.buttonLinkTest = new System.Windows.Forms.Button();
			this.buttonGetDeviceInfo = new System.Windows.Forms.Button();
			this.buttonFlashFirmware = new System.Windows.Forms.Button();
			this.openFileDialogSelectFirmwareFile = new System.Windows.Forms.OpenFileDialog();
			this.buttonSetTime = new System.Windows.Forms.Button();
			this.dateTimePickerRpdTime = new System.Windows.Forms.DateTimePicker();
			this.buttonSetCurrentTime = new System.Windows.Forms.Button();
			this.comboBoxSectionNumber = new System.Windows.Forms.ComboBox();
			this.buttonFormatRpd = new System.Windows.Forms.Button();
			this.numericUpDownDevAddr = new System.Windows.Forms.NumericUpDown();
			this.buttonSetNetAddress = new System.Windows.Forms.Button();
			this.textBoxLocName = new System.Windows.Forms.TextBox();
			this.labelForUserModifyConf = new System.Windows.Forms.Label();
			this.labelForUserDeviceName = new System.Windows.Forms.Label();
			this.labelForUserSectionNumber = new System.Windows.Forms.Label();
			this.labelForUserDevAddr = new System.Windows.Forms.Label();
			this.labelForUserLocNumber = new System.Windows.Forms.Label();
			this.numericUpDownLocNumber = new System.Windows.Forms.NumericUpDown();
			this.labelForUserNetAddr = new System.Windows.Forms.Label();
			this.numericUpDownNetAddr = new System.Windows.Forms.NumericUpDown();
			this.logPsnBox = new System.Windows.Forms.CheckBox();

			this.statusStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevAddr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNetAddr)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBoxDrives
			// 
			this.comboBoxDrives.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDrives.FormattingEnabled = true;
			this.comboBoxDrives.Location = new System.Drawing.Point(88, 12);
			this.comboBoxDrives.Name = "comboBoxDrives";
			this.comboBoxDrives.Size = new System.Drawing.Size(348, 21);
			this.comboBoxDrives.TabIndex = 0;
			// 
			// buttonClearAll
			// 
			this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClearAll.Location = new System.Drawing.Point(15, 68);
			this.buttonClearAll.Name = "buttonClearAll";
			this.buttonClearAll.Size = new System.Drawing.Size(502, 23);
			this.buttonClearAll.TabIndex = 3;
			this.buttonClearAll.Text = "Стереть все архивы";
			this.buttonClearAll.UseVisualStyleBackColor = true;
			this.buttonClearAll.Click += new System.EventHandler(this.ButtonClearAllClick);
			// 
			// labelDevName
			// 
			this.labelDevName.AutoSize = true;
			this.labelDevName.Location = new System.Drawing.Point(12, 15);
			this.labelDevName.Name = "labelDevName";
			this.labelDevName.Size = new System.Drawing.Size(70, 13);
			this.labelDevName.TabIndex = 5;
			this.labelDevName.Text = "Устройство:";
			// 
			// buttonRefreshDevs
			// 
			this.buttonRefreshDevs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRefreshDevs.Location = new System.Drawing.Point(442, 10);
			this.buttonRefreshDevs.Name = "buttonRefreshDevs";
			this.buttonRefreshDevs.Size = new System.Drawing.Size(75, 23);
			this.buttonRefreshDevs.TabIndex = 1;
			this.buttonRefreshDevs.Text = "Обновить";
			this.buttonRefreshDevs.UseVisualStyleBackColor = true;
			this.buttonRefreshDevs.Click += new System.EventHandler(this.ButtonRefreshDevsClick);
			// 
			// statusStripMain
			// 
			this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMain,
            this.toolStripProgressBarMain});
			this.statusStripMain.Location = new System.Drawing.Point(0, 440);
			this.statusStripMain.Name = "statusStripMain";
			this.statusStripMain.Size = new System.Drawing.Size(534, 22);
			this.statusStripMain.TabIndex = 0;
			this.statusStripMain.Text = "Строка статуса";
			// 
			// toolStripStatusLabelMain
			// 
			this.toolStripStatusLabelMain.Name = "toolStripStatusLabelMain";
			this.toolStripStatusLabelMain.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabelMain.Text = "Загрузка завершена";
			// 
			// toolStripProgressBarMain
			// 
			this.toolStripProgressBarMain.Name = "toolStripProgressBarMain";
			this.toolStripProgressBarMain.Size = new System.Drawing.Size(100, 16);
			this.toolStripProgressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.toolStripProgressBarMain.Visible = false;
			// 
			// buttonLinkTest
			// 
			this.buttonLinkTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLinkTest.Location = new System.Drawing.Point(15, 126);
			this.buttonLinkTest.Name = "buttonLinkTest";
			this.buttonLinkTest.Size = new System.Drawing.Size(502, 23);
			this.buttonLinkTest.TabIndex = 5;
			this.buttonLinkTest.Text = "Тест связи с измерителями";
			this.buttonLinkTest.UseVisualStyleBackColor = true;
			this.buttonLinkTest.Click += new System.EventHandler(this.ButtonLinkTestClick);
			// 
			// buttonGetDeviceInfo
			// 
			this.buttonGetDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGetDeviceInfo.Location = new System.Drawing.Point(15, 39);
			this.buttonGetDeviceInfo.Name = "buttonGetDeviceInfo";
			this.buttonGetDeviceInfo.Size = new System.Drawing.Size(502, 23);
			this.buttonGetDeviceInfo.TabIndex = 2;
			this.buttonGetDeviceInfo.Text = "Считать конфигурацию";
			this.buttonGetDeviceInfo.UseVisualStyleBackColor = true;
			this.buttonGetDeviceInfo.Click += new System.EventHandler(this.ButtonGetDeviceInfoClick);
			// 
			// buttonFlashFirmware
			// 
			this.buttonFlashFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonFlashFirmware.Location = new System.Drawing.Point(15, 155);
			this.buttonFlashFirmware.Name = "buttonFlashFirmware";
			this.buttonFlashFirmware.Size = new System.Drawing.Size(502, 23);
			this.buttonFlashFirmware.TabIndex = 6;
			this.buttonFlashFirmware.Text = "Обновить прошивку...";
			this.buttonFlashFirmware.UseVisualStyleBackColor = true;
			this.buttonFlashFirmware.Click += new System.EventHandler(this.ButtonFlashFirmwareClick);
			// 
			// openFileDialogSelectFirmwareFile
			// 
			this.openFileDialogSelectFirmwareFile.Filter = "Бинарные данные (*.bin)|*.bin|Все файлы (*.*)|*.*";
			this.openFileDialogSelectFirmwareFile.RestoreDirectory = true;
			this.openFileDialogSelectFirmwareFile.Title = "Выбор файла с прошивкой";
			// 
			// buttonSetTime
			// 
			this.buttonSetTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSetTime.Location = new System.Drawing.Point(96, 213);
			this.buttonSetTime.Name = "buttonSetTime";
			this.buttonSetTime.Size = new System.Drawing.Size(421, 23);
			this.buttonSetTime.TabIndex = 9;
			this.buttonSetTime.Text = "<--- Установить указанное время РПД";
			this.buttonSetTime.UseVisualStyleBackColor = true;
			this.buttonSetTime.Click += new System.EventHandler(this.ButtonSetTimeClick);
			// 
			// dateTimePickerRpdTime
			// 
			this.dateTimePickerRpdTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateTimePickerRpdTime.Location = new System.Drawing.Point(15, 213);
			this.dateTimePickerRpdTime.Name = "dateTimePickerRpdTime";
			this.dateTimePickerRpdTime.Size = new System.Drawing.Size(75, 20);
			this.dateTimePickerRpdTime.TabIndex = 8;
			// 
			// buttonSetCurrentTime
			// 
			this.buttonSetCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSetCurrentTime.Location = new System.Drawing.Point(15, 184);
			this.buttonSetCurrentTime.Name = "buttonSetCurrentTime";
			this.buttonSetCurrentTime.Size = new System.Drawing.Size(502, 23);
			this.buttonSetCurrentTime.TabIndex = 7;
			this.buttonSetCurrentTime.Text = "Синхронизовать время блока со временем компьютера";
			this.buttonSetCurrentTime.UseVisualStyleBackColor = true;
			this.buttonSetCurrentTime.Click += new System.EventHandler(this.ButtonSetCurrentTimeClick);
			// 
			// comboBoxSectionNumber
			// 
			this.comboBoxSectionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxSectionNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSectionNumber.FormattingEnabled = true;
			this.comboBoxSectionNumber.Items.AddRange(new object[] {
            "Секция 1",
            "Секция 2"});
			this.comboBoxSectionNumber.Location = new System.Drawing.Point(416, 331);
			this.comboBoxSectionNumber.Name = "comboBoxSectionNumber";
			this.comboBoxSectionNumber.Size = new System.Drawing.Size(101, 21);
			this.comboBoxSectionNumber.TabIndex = 12;
			// 
			// buttonFormatRpd
			// 
			this.buttonFormatRpd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonFormatRpd.Location = new System.Drawing.Point(15, 97);
			this.buttonFormatRpd.Name = "buttonFormatRpd";
			this.buttonFormatRpd.Size = new System.Drawing.Size(502, 23);
			this.buttonFormatRpd.TabIndex = 4;
			this.buttonFormatRpd.Text = "Переформатировать РПД";
			this.buttonFormatRpd.UseVisualStyleBackColor = true;
			this.buttonFormatRpd.Click += new System.EventHandler(this.ButtonFormatRpdClick);
			// 
			// numericUpDownDevAddr
			// 
			this.numericUpDownDevAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownDevAddr.Location = new System.Drawing.Point(416, 358);
			this.numericUpDownDevAddr.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericUpDownDevAddr.Name = "numericUpDownDevAddr";
			this.numericUpDownDevAddr.Size = new System.Drawing.Size(101, 20);
			this.numericUpDownDevAddr.TabIndex = 14;
			this.numericUpDownDevAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// buttonSetNetAddress
			// 
			this.buttonSetNetAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSetNetAddress.Location = new System.Drawing.Point(15, 440);
			this.buttonSetNetAddress.Name = "buttonSetNetAddress";
			this.buttonSetNetAddress.Size = new System.Drawing.Size(502, 23);
			this.buttonSetNetAddress.TabIndex = 13;
			this.buttonSetNetAddress.Text = "Записать конфигурацию (SYSCONF, RPDCONF)";
			this.buttonSetNetAddress.UseVisualStyleBackColor = true;
			this.buttonSetNetAddress.Click += new System.EventHandler(this.ButtonSetNetAddressClick);
			// 
			// textBoxLocName
			// 
			this.textBoxLocName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLocName.Location = new System.Drawing.Point(280, 278);
			this.textBoxLocName.Name = "textBoxLocName";
			this.textBoxLocName.Size = new System.Drawing.Size(237, 20);
			this.textBoxLocName.TabIndex = 15;
			// 
			// labelForUserModifyConf
			// 
			this.labelForUserModifyConf.AutoSize = true;
			this.labelForUserModifyConf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelForUserModifyConf.Location = new System.Drawing.Point(114, 255);
			this.labelForUserModifyConf.Name = "labelForUserModifyConf";
			this.labelForUserModifyConf.Size = new System.Drawing.Size(317, 13);
			this.labelForUserModifyConf.TabIndex = 16;
			this.labelForUserModifyConf.Text = "Модификация конфигурации (SYSCONF, RPDCONF):";
			// 
			// labelForUserDeviceName
			// 
			this.labelForUserDeviceName.AutoSize = true;
			this.labelForUserDeviceName.Location = new System.Drawing.Point(12, 281);
			this.labelForUserDeviceName.Name = "labelForUserDeviceName";
			this.labelForUserDeviceName.Size = new System.Drawing.Size(181, 13);
			this.labelForUserDeviceName.TabIndex = 17;
			this.labelForUserDeviceName.Text = "Название устройства (RPDCONF):";
			// 
			// labelForUserSectionNumber
			// 
			this.labelForUserSectionNumber.AutoSize = true;
			this.labelForUserSectionNumber.Location = new System.Drawing.Point(12, 334);
			this.labelForUserSectionNumber.Name = "labelForUserSectionNumber";
			this.labelForUserSectionNumber.Size = new System.Drawing.Size(206, 13);
			this.labelForUserSectionNumber.TabIndex = 18;
			this.labelForUserSectionNumber.Text = "Номер секции (SYSCONF и RPDCONF):";
			// 
			// labelForUserDevAddr
			// 
			this.labelForUserDevAddr.AutoSize = true;
			this.labelForUserDevAddr.Location = new System.Drawing.Point(12, 360);
			this.labelForUserDevAddr.Name = "labelForUserDevAddr";
			this.labelForUserDevAddr.Size = new System.Drawing.Size(302, 13);
			this.labelForUserDevAddr.TabIndex = 19;
			this.labelForUserDevAddr.Text = "Адрес устройства (Intelecon netaddr, ftpdiraddr) (SYSCONF):";
			// 
			// labelForUserLocNumber
			// 
			this.labelForUserLocNumber.AutoSize = true;
			this.labelForUserLocNumber.Location = new System.Drawing.Point(12, 306);
			this.labelForUserLocNumber.Name = "labelForUserLocNumber";
			this.labelForUserLocNumber.Size = new System.Drawing.Size(167, 13);
			this.labelForUserLocNumber.TabIndex = 21;
			this.labelForUserLocNumber.Text = "Номер локомотива (SYSCONF):";
			// 
			// numericUpDownLocNumber
			// 
			this.numericUpDownLocNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownLocNumber.Location = new System.Drawing.Point(416, 304);
			this.numericUpDownLocNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericUpDownLocNumber.Name = "numericUpDownLocNumber";
			this.numericUpDownLocNumber.Size = new System.Drawing.Size(101, 20);
			this.numericUpDownLocNumber.TabIndex = 20;
			this.numericUpDownLocNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownLocNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// labelForUserNetAddr
			// 
			this.labelForUserNetAddr.AutoSize = true;
			this.labelForUserNetAddr.Location = new System.Drawing.Point(12, 386);
			this.labelForUserNetAddr.Name = "labelForUserNetAddr";
			this.labelForUserNetAddr.Size = new System.Drawing.Size(144, 13);
			this.labelForUserNetAddr.TabIndex = 23;
			this.labelForUserNetAddr.Text = "Сетевой адрес (SYSCONF):";
			// 
			// numericUpDownNetAddr
			// 
			this.numericUpDownNetAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownNetAddr.Location = new System.Drawing.Point(416, 384);
			this.numericUpDownNetAddr.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
			this.numericUpDownNetAddr.Name = "numericUpDownNetAddr";
			this.numericUpDownNetAddr.Size = new System.Drawing.Size(101, 20);
			this.numericUpDownNetAddr.TabIndex = 22;
			this.numericUpDownNetAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

			// logPsnBox

			this.logPsnBox.Location = new System.Drawing.Point(12, 414);
			this.logPsnBox.Name = "logPsnBox";
			this.logPsnBox.Text= "Вести лог магистрали ПСН?";
			this.logPsnBox.TabIndex = 24;
			this.logPsnBox.AutoSize = true;


			// 
			// FormConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 500);
			this.Controls.Add(this.labelForUserNetAddr);
			this.Controls.Add(this.numericUpDownNetAddr);
			this.Controls.Add(this.labelForUserLocNumber);
			this.Controls.Add(this.numericUpDownLocNumber);
			this.Controls.Add(this.labelForUserDevAddr);
			this.Controls.Add(this.labelForUserSectionNumber);
			this.Controls.Add(this.labelForUserDeviceName);
			this.Controls.Add(this.labelForUserModifyConf);
			this.Controls.Add(this.textBoxLocName);
			this.Controls.Add(this.numericUpDownDevAddr);
			this.Controls.Add(this.buttonSetNetAddress);
			this.Controls.Add(this.buttonFormatRpd);
			this.Controls.Add(this.comboBoxSectionNumber);
			this.Controls.Add(this.buttonSetCurrentTime);
			this.Controls.Add(this.dateTimePickerRpdTime);
			this.Controls.Add(this.buttonSetTime);
			this.Controls.Add(this.buttonFlashFirmware);
			this.Controls.Add(this.buttonGetDeviceInfo);
			this.Controls.Add(this.buttonLinkTest);
			this.Controls.Add(this.statusStripMain);
			this.Controls.Add(this.buttonRefreshDevs);
			this.Controls.Add(this.labelDevName);
			this.Controls.Add(this.buttonClearAll);
			this.Controls.Add(this.comboBoxDrives);
			this.Controls.Add(this.logPsnBox);

			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1024, 530);
			this.MinimumSize = new System.Drawing.Size(500, 530);
			this.Name = "FormConfig";
			this.Text = "RPD MicroConfig";
			this.statusStripMain.ResumeLayout(false);
			this.statusStripMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevAddr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNetAddr)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxDrives;
		private System.Windows.Forms.Button buttonClearAll;
		private System.Windows.Forms.Label labelDevName;
		private System.Windows.Forms.Button buttonRefreshDevs;
		private System.Windows.Forms.StatusStrip statusStripMain;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMain;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarMain;
		private System.Windows.Forms.Button buttonLinkTest;
		private System.Windows.Forms.Button buttonGetDeviceInfo;
		private System.Windows.Forms.Button buttonFlashFirmware;
		private System.Windows.Forms.OpenFileDialog openFileDialogSelectFirmwareFile;
		private System.Windows.Forms.Button buttonSetTime;
		private System.Windows.Forms.DateTimePicker dateTimePickerRpdTime;
		private System.Windows.Forms.Button buttonSetCurrentTime;
		private System.Windows.Forms.ComboBox comboBoxSectionNumber;
		private System.Windows.Forms.Button buttonFormatRpd;
		private System.Windows.Forms.NumericUpDown numericUpDownDevAddr;
		private System.Windows.Forms.Button buttonSetNetAddress;
		private System.Windows.Forms.TextBox textBoxLocName;
		private System.Windows.Forms.Label labelForUserModifyConf;
		private System.Windows.Forms.Label labelForUserDeviceName;
		private System.Windows.Forms.Label labelForUserSectionNumber;
		private System.Windows.Forms.Label labelForUserDevAddr;
		private System.Windows.Forms.Label labelForUserLocNumber;
		private System.Windows.Forms.NumericUpDown numericUpDownLocNumber;
		private System.Windows.Forms.Label labelForUserNetAddr;
		private System.Windows.Forms.NumericUpDown numericUpDownNetAddr;

		private System.Windows.Forms.CheckBox logPsnBox;
	}
}

