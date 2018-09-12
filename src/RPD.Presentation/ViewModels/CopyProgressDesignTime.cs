using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.ViewModels {
	class CopyProgressDesignTime : ICopyProgressViewModel {
		public CopyProgressDesignTime() {
			Close = new RelayCommand(Execute);
			Progress = 30;
			Minimum = 0;
			Maximum = 100;

			Header = "Экспорт данных";
			PsnLogString = "Скопировано аварий ПСН: 5/10";
			FaultsString = "Скопировано аварий РПД: 0/3";
			WindowTitle = "Копирование данных РПД";
		}

		private void Execute() {
			;
		}

		#region Implementation of ICopyProgressViewModel

		public int Progress { get; set; }
		public int Minimum { get; }
		public int Maximum { get; }
		public string WindowTitle { get; set; }
		public string Header { get; set; }
		public string Status { get; set; }
		public string FaultsString { get; set; }
		public string PsnLogString { get; set; }
		public RelayCommand Close { get; set; }

		#endregion
	}
}
