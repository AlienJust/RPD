using System;
using System.Windows.Forms;

namespace MicroConfig
{
	public partial class FormInfo : Form
	{
		public FormInfo(string text)
		{
			InitializeComponent();
			textBoxText.Text = text;
		}

		private void ButtonOkClick(object sender, EventArgs e)
		{
			Close();
		}

		private void ButtonCopyToClipboardClick(object sender, EventArgs e)
		{
			Clipboard.SetText(textBoxText.Text);
		}

		private void LinkLabelFullInfoLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			MessageBox.Show("Данная функция находится в процессе разработки\nПриносим извинения за временные неудобства", "Внимание");
			//Process.Start("LastReadedSysConf.txt");
		}
	}
}
