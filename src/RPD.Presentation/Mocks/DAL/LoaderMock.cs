using System;
using System.Collections.Generic;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Mocks.Helpers;

namespace RPD.Presentation.Mocks.DAL {
	class LoaderMock : ILoader {
		public LoaderMock() {
			AvailablePsnConfigruations = new List<IPsnConfiguration>
			{
				new PsnConfigurationMock("1", "ТРАМВАЙ", "Трамвайная прошивка", Guid.NewGuid()),
				new PsnConfigurationMock("1.1", "Локомотив", "Локомотивная прошивка", Guid.NewGuid()),
				new PsnConfigurationMock("2", "Локомотив РП", "Локомотивная прошивка РП", Guid.NewGuid())
			};
		}

		public IDeviceConfiguration CreateDeviceConfiguration() {
			throw new NotImplementedException();
		}

		public IRepository GetLocalDirectoryRepository(string directoryPath) {
			var repo = new RepositoryMock();
			repo.Locomotives.Add(LocomotiveGenerator.GenerateLocomotive("Локомотив 77РР"));

			return repo;
		}

		public IRepository GetZippedRepository(string zipFileName) {
			var repo = new RepositoryMock();
			repo.Locomotives.Add(LocomotiveGenerator.GenerateLocomotive("Локомотив ZIPPED"));

			return repo;
		}

		public IRepository GetNandFlashRepository(string nandPath, IPsnConfiguration psnConfiguration) {
			var repo = new RepositoryMock();
			repo.Locomotives.Add(LocomotiveGenerator.GenerateLocomotive("Локомотив NAND"));

			return repo;
		}

		public IRepository GetFtpFilesRepository(string ftpFilesDirecotyPath, string locomotiveName, string sectionName,
				IPsnConfiguration psnConfiguration) {
			throw new NotImplementedException();
		}

		public IRepository GetFtpClientRepository(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, int deviceNumber,
				string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration) {
			throw new NotImplementedException();
		}

		public void GetFtpRepositoryInfosAsync(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword,
				Action<OnCompleteEventArgs, IEnumerable<IFtpRepositoryInfo>> callbackAction) {
			throw new NotImplementedException();
		}

		public IRepository GetFtpRepository(IFtpRepositoryInfo ftpRepositoryInfo, string locomotiveName, string sectionName,
				IPsnConfiguration psnConfiguration) {
			throw new NotImplementedException();
		}
		public IEnumerable<IPsnConfiguration> AvailablePsnConfigruations { get; }
		public void Dispose() {

		}
	}
}
