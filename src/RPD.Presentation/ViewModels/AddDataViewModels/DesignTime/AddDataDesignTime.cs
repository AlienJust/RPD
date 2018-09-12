using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.AddDataViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels.DesignTime {
	class AddDataDesignTime : IAddDataViewModel {
		public AddDataDesignTime() {
			SelectDataSource = new SelectDataSourceDesignTime();
			AvailableFaults = new AvailableFaultsDesignTime();
			SelectPsnConfiguration = new SelectPsnConfigurationDesignTime();
			SelectFtpServer = new SelectFtpServerDesignTime();
		}

		#region Implementation of IMessageable

		public Messenger Messenger { get; set; }

		#endregion

		#region Implementation of IAddDataViewModel

		public string Title => "Сохранение данных РПД";

		public ISelectDataSourceViewModel SelectDataSource { get; }
		public ISelectUsbDeviceViewModel SelectUsbDevice { get; private set; }
		public ISelectPathViewModel SelectPath { get; private set; }
		public ISelectPathViewModel SelectRpdDump { get; private set; }
		public IAvailableFaultsViewModel AvailableFaults { get; }
		public IProgressViewModel ReadProgress { get; private set; }

		public ISelectPsnConfigurationViewModel SelectPsnConfiguration { get; }

		public ISelectFtpServerViewModel SelectFtpServer { get; }

		public ISelectFtpDeviceViewModel SelectFtpDevice => throw new System.NotImplementedException();

		public IDeviceLocomotiveNameViewModel DeviceLocomotiveName => throw new System.NotImplementedException();

		#endregion
	}
}
