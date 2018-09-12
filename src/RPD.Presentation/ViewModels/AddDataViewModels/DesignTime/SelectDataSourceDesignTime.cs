using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace RPD.Presentation.ViewModels.AddDataViewModels.DesignTime {
	class SelectDataSourceDesignTime : ISelectDataSourceViewModel {
		public SelectDataSourceDesignTime() {
			ReadFromFlashCommand = new RelayCommand(Execute);
			ReadFromImageCommand = new RelayCommand(Execute);
			ReadFromFolderCommand = new RelayCommand(Execute);
		}

		private void Execute() {
			;
		}

		#region Implementation of ISelectDataSourceViewModel

		public bool IsSimpleMode => false;

		public RelayCommand ReadFromFlashCommand { get; set; }
		public RelayCommand ReadFromImageCommand { get; set; }
		public RelayCommand ReadFromFolderCommand { get; set; }

		public RelayCommand ReadFromFtpServerCommand {
			get => throw new System.NotImplementedException();
			set => throw new System.NotImplementedException();
		}

		#endregion

		#region Implementation of IMessageable

		public Messenger Messenger { get; set; }

		#endregion
	}
}
