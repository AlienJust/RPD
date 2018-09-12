using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.Model;
namespace RPD.Presentation.ViewModels.DalViewModels {
	internal class FtpServerViewModel : ViewModelBase {
		private readonly IFtpServer _ftpServer;

		public IFtpServer Model => _ftpServer;

		public FtpServerViewModel(IFtpServer ftpServer) {
			_ftpServer = ftpServer;
		}

		public string Name {
			get => _ftpServer.Name;
			set {
				if (_ftpServer.Name == value)
					return;

				_ftpServer.Name = value;
				RaisePropertyChanged(() => Name);
			}
		}

		public string Host {
			get => _ftpServer.Host;
			set {
				if (_ftpServer.Host == value)
					return;

				_ftpServer.Host = value;
				RaisePropertyChanged(() => Host);
			}
		}

		public int Port {
			get => _ftpServer.Port;
			set {
				if (_ftpServer.Port == value)
					return;

				_ftpServer.Port = value;
				RaisePropertyChanged(() => Port);
			}
		}

		public string User {
			get => _ftpServer.User;
			set {
				if (_ftpServer.User == value)
					return;

				_ftpServer.User = value;
				RaisePropertyChanged(() => User);
			}
		}

		public string Password {
			get => _ftpServer.Password;
			set {
				if (_ftpServer.Password == value)
					return;

				_ftpServer.Password = value;
				RaisePropertyChanged(() => Password);
			}
		}
	}
}