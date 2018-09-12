using RPD.EventArgs;
using System;
using System.Windows.Forms;
using AlienJust.Adaptation.WinForms;
using RPD.DAL;
namespace MicroConfig {
	public partial class FormConfig : Form {
		public FormConfig() {
			InitializeComponent();
			FillDrives();
		}

		void FillDrives() {
			try {
				var drives = new UsbRemovableDrives();
				comboBoxDrives.Items.Clear();
				foreach (var driveInfo in drives.Items) {
					comboBoxDrives.Items.Add(driveInfo.RootDirectory.FullName);
				}
				if (comboBoxDrives.Items.Count > 0) {
					comboBoxDrives.SelectedIndex = 0;
				}
				LogStatusMessage("Список устройств получен успешно");
			}
			catch (Exception) {
				LogStatusMessage("Ошибка получения списка устройств");
			}
		}

		private void ButtonRefreshDevsClick(object sender, EventArgs e) {
			FillDrives();
		}

		private void LogStatusMessage(string message) {
			toolStripStatusLabelMain.Text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message;
		}

		private void LockInterface() {
			LogStatusMessage("Ждите...");
			ChangeFace(false);
		}
		private void UnlockInterface() {
			ChangeFace(true);
		}
		private void ChangeFace(bool unlock) {
			foreach (Control control in Controls) {
				control.Enabled = unlock;
			}
			statusStripMain.Enabled = true; // Allways
			toolStripStatusLabelMain.Enabled = true;
			toolStripProgressBarMain.Visible = !unlock;
		}

		private void ButtonClearAllClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					LockInterface();
					var loader = new Loader(new WinFormsUiNotifier(this));
					var devConf = loader.CreateDeviceConfiguration();
					devConf.ClearArchivesAsync(comboBoxDrives.SelectedItem.ToString().Substring(0, 1), this.OnCommandComplete);
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}

		}
		private void OnCommandComplete(OnCompleteEventArgs e) {
			UnlockInterface();
			if (e.ResultCode == OnCompleteEventArgs.CompleteResult.Ok) {
				LogStatusMessage(e.Message);
			}
			else if (e.ResultCode == OnCompleteEventArgs.CompleteResult.Error) {
				LogStatusMessage("Ошибка: " + e.Message);
			}
		}

		private void ButtonLinkTestClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					LockInterface();
					var loader = new Loader(new WinFormsUiNotifier(this));
					var devConf = loader.CreateDeviceConfiguration();
					devConf.TestLinkWithMetersAsync(comboBoxDrives.SelectedItem.ToString().Substring(0, 1), OnCommandComplete);
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private IDeviceConfiguration _lastReadedConf;

		private void ButtonGetDeviceInfoClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					LockInterface();
					var loader = new Loader(new WinFormsUiNotifier(this));
					_lastReadedConf = loader.CreateDeviceConfiguration();
					_lastReadedConf.Read(comboBoxDrives.SelectedItem.ToString(), OnConfigReadComplete);
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void OnConfigReadComplete(OnCompleteEventArgs e) {
			UnlockInterface();
			if (e.ResultCode == OnCompleteEventArgs.CompleteResult.Ok) {
				LogStatusMessage(e.Message);
				numericUpDownDevAddr.Value = _lastReadedConf.Address;
				textBoxLocName.Text = _lastReadedConf.LocomotiveName;
				comboBoxSectionNumber.SelectedIndex = _lastReadedConf.SectionNumber - 1;
				logPsnBox.Checked = _lastReadedConf.LogPsn;

				var configInfo = new FormInfo(_lastReadedConf.ToString());
				configInfo.ShowDialog();
			}
			else if (e.ResultCode == OnCompleteEventArgs.CompleteResult.Error) {
				LogStatusMessage("Ошибка: " + e.Message);
				textBoxLocName.Text = string.Empty;
				numericUpDownLocNumber.Value = 0;
				comboBoxSectionNumber.SelectedItem = null;
				numericUpDownDevAddr.Value = 0;
				numericUpDownNetAddr.Value = 0;
			}
		}


		private void ButtonFlashFirmwareClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					var result = openFileDialogSelectFirmwareFile.ShowDialog(this);
					if (result == DialogResult.OK) {
						LockInterface();
						var loader = new Loader(new WinFormsUiNotifier(this));
						var devConf = loader.CreateDeviceConfiguration();
						devConf.WriteFirmware(openFileDialogSelectFirmwareFile.FileName, comboBoxDrives.SelectedItem.ToString().Substring(0, 1), OnCommandComplete);
					}
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void ButtonSetTimeClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					LockInterface();
					var loader = new Loader(new WinFormsUiNotifier(this));
					var devConf = loader.CreateDeviceConfiguration();
					devConf.SetTime(dateTimePickerRpdTime.Value, comboBoxDrives.SelectedItem.ToString().Substring(0, 1), OnCommandComplete);
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void ButtonSetCurrentTimeClick(object sender, EventArgs e) {
			dateTimePickerRpdTime.Value = DateTime.Now;
			ButtonSetTimeClick(sender, e);
		}

		private void ButtonFormatRpdClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null) {
					var choise = MessageBox.Show(this, "Вы уверены? Переформатирование сотрет все данные и обновит файловую систему РПД", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if (choise == DialogResult.OK) {
						LogStatusMessage("Внимание: не отсоединяйте блок от компьютера до окончания операции!");
						LockInterface();
						var loader = new Loader(new WinFormsUiNotifier(this));
						var devConf = loader.CreateDeviceConfiguration();
						devConf.FormatRpdAsync(comboBoxDrives.SelectedItem.ToString().Substring(0, 1), OnCommandComplete);
					}
					else if (choise == DialogResult.Cancel) {
						LogStatusMessage("Переформатирование отменено пользователем");
					}
					else throw new Exception("Неподдерживаемый результат работы с диалоговым окном");
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void ButtonSetNetAddressClick(object sender, EventArgs e) {
			try {
				if (comboBoxDrives.SelectedItem != null && _lastReadedConf != null) {
					LockInterface();
					_lastReadedConf.LocomotiveName = textBoxLocName.Text;
					_lastReadedConf.LocomotiveNumber = (int) numericUpDownLocNumber.Value;
					_lastReadedConf.SectionNumber = comboBoxSectionNumber.SelectedIndex + 1;
					_lastReadedConf.Address = (int)numericUpDownDevAddr.Value;
					_lastReadedConf.NetAddress = (int)numericUpDownNetAddr.Value;
					_lastReadedConf.LogPsn = logPsnBox.Checked;
					_lastReadedConf.Write(comboBoxDrives.SelectedItem.ToString(), ea => {
						if (ea.ResultCode == OnCompleteEventArgs.CompleteResult.Ok) {
							LogStatusMessage("Конфигурация успешно записана");
						}
						else { LogStatusMessage("Ошибка: " + ea.Message); }
						UnlockInterface();
					});
				}
				else {
					LogStatusMessage("Ошибка: сперва необходимо выбрать устройство и прочитать конфигурацию");
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
