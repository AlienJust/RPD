using System.Globalization;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	public class FtpRepositoryInfoViewModel : ViewModelBase, IFtpRepositoryInfoViewModel {
		private readonly IFtpRepositoryInfo _ftpRepositoryInfo;
		private bool _isNameSet;
		private string _locomotiveName;
		private string _sectionName;

		public IFtpRepositoryInfo Model => _ftpRepositoryInfo;

		public FtpRepositoryInfoViewModel(IFtpRepositoryInfo ftpRepositoryInfo, IDeviceInfoRepository deviceInfoRepository) {
			_ftpRepositoryInfo = ftpRepositoryInfo;

			var deviceInfo = deviceInfoRepository.GetByDeviceNumber(_ftpRepositoryInfo.DeviceNumber);
			if (deviceInfo != null) {
				IsNameSet = true;
				LocomotiveName = deviceInfo.LocomotiveName;
				SectionName = "Секция " + deviceInfo.SectionNumber;
			}
		}

		public string DeviceNumber => _ftpRepositoryInfo.DeviceNumber.ToString(CultureInfo.InvariantCulture);

		public bool IsNameSet {
			get => _isNameSet;
			set { Set(() => IsNameSet, ref _isNameSet, value); }
		}

		public string LocomotiveName {
			get => _locomotiveName;
			set { Set(() => LocomotiveName, ref _locomotiveName, value); }
		}

		public string SectionName {
			get => _sectionName;
			set { Set(() => SectionName, ref _sectionName, value); }
		}

		public string LocomotiveAndSectionString {
			get {
				if (string.IsNullOrWhiteSpace(LocomotiveName))
					return "";

				return LocomotiveName + ". " + SectionName + ".";
			}
		}
	}
}
