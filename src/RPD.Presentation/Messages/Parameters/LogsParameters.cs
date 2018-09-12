using System.Collections.Generic;
using RPD.DAL;

namespace RPD.Presentation.Messages.Parameters {
	/// <summary>
	/// Параметр сообщения для диалога прогресса копирования. Содержит список данных, которые нужно экспортировать.
	/// </summary>
	class LogsParameters {
		public LogsParameters() {
			RpdLogs = new List<IFaultLog>();
			PsnLogs = new List<IPsnLog>();
		}

		public IRepository Repository { get; set; }
		public IList<IFaultLog> RpdLogs { get; set; }
		public IList<IPsnLog> PsnLogs { get; set; }
	}
}
