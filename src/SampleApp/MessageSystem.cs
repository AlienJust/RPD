using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SampleApp
{
	static class MessageSystem
	{
		public static void ShowMessage(string messageText) {
			MessageBox.Show(messageText);
		}
	}
}
