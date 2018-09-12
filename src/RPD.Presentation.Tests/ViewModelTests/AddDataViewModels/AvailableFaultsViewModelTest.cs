using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.DAL;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels {
	[TestFixture]
	public class AvailableFaultsViewModelTest {
		private Mock<IAddDataParameters> _addData;
		private Mock<IRepository> _repository;
		private IList<ILocomotive> _locomotives;
		private IMessenger _messenger;

		[SetUp]
		public void SetUp() {
			_addData = new Mock<IAddDataParameters>();
			_repository = new Mock<IRepository>();
			_locomotives = new List<ILocomotive>();
			_messenger = new Messenger();

			GenerateLocomotives();
		}

		private void GenerateLocomotives() {
			var loco = new LocomotiveMock() { Name = "1" };
			loco.Sections.Add(GenerateSection(loco, "1"));
			loco.Sections.Add(GenerateSection(loco, "2"));
			_locomotives.Add(loco);

			var loco2 = new LocomotiveMock() { Name = "2" };
			loco2.Sections.Add(GenerateSection(loco2, "3"));
			loco2.Sections.Add(GenerateSection(loco2, "4"));
			_locomotives.Add(loco2);
		}

		private static SectionMock GenerateSection(LocomotiveMock loco, string name) {
			var section = new SectionMock(loco) { Name = name };
			section.Psns.Add(new PsnLogMock(section, PsnLogType.FixedLength,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())));
			section.Psns.Add(new PsnLogMock(section, PsnLogType.FixedLength,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())));

			section.Psns.Add(new PsnLogMock(section, PsnLogType.PowerDepended,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())));
			section.Psns.Add(new PsnLogMock(section, PsnLogType.PowerDepended,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())));

			section.Faults.Add(new FaultLogMock(section));
			section.Faults.Add(new FaultLogMock(section));
			return section;
		}

		[Test]
		public void Constructor_LocomotivesAndSectionsFilled() {
			var viewModel = CreateViewModel(false);

			Assert.That(viewModel.Locomotives.Count, Is.EqualTo(2));
			Assert.That(viewModel.Locomotives[0].Locomotive, Is.SameAs(_locomotives[0]));
			Assert.That(viewModel.Locomotives[1].Locomotive, Is.SameAs(_locomotives[1]));

			Assert.That(viewModel.Sections.Count, Is.EqualTo(4));
			Assert.That(viewModel.Sections[0].Section.Name, Is.EqualTo("1"));
			Assert.That(viewModel.Sections[1].Section.Name, Is.EqualTo("2"));
			Assert.That(viewModel.Sections[2].Section.Name, Is.EqualTo("3"));
			Assert.That(viewModel.Sections[3].Section.Name, Is.EqualTo("4"));
		}

		private AvailableFaultsViewModel CreateViewModel(bool isSimpleMode) {
			return new AvailableFaultsViewModel(_messenger, _locomotives, _addData.Object, _repository.Object, () => { }, isSimpleMode);
		}

		[Test]
		public void Constructor_SimpleModeIsFalse_IsSavedToRepositorySet() {
			_repository.Setup(m => m.IsExist(It.IsAny<IFaultLog>())).Returns(true).Verifiable();
			_repository.Setup(m => m.IsExist(It.IsAny<IPsnLog>())).Returns(true).Verifiable();

			var viewModel = CreateViewModel(false);
			_repository.Verify();

			foreach (var sec in viewModel.Sections) {
				foreach (var fault in sec.Faults)
					Assert.That(fault.IsSavedToRepository, Is.True);

				foreach (var psnLog in sec.PsnLogs)
					Assert.That(psnLog.IsSavedToRepository, Is.True);

				foreach (var psnPowerOn in sec.PsnPowerOnLogs)
					Assert.That(psnPowerOn.IsSavedToRepository, Is.True);
			}

		}

		[Test]
		public void NextCommand_NothingChecked_CanNotExecute() {
			bool executed = false;
			var viewModel = new AvailableFaultsViewModel(_messenger, _locomotives, _addData.Object, _repository.Object, () => {
				executed = true;
			}, false);
			viewModel.NextCommand.Execute(null);

			Assert.That(executed, Is.False);
		}

		[Test]
		public void NextCommand_SomethingChecked_DataParamsSetAndNavigateNextPageExecuted() {
			var faults = new List<IFaultLog>();
			var psnLogs = new List<IPsnLog>();
			_addData.Setup(m => m.FaultsToRead).Returns(faults);
			_addData.Setup(m => m.PsnLogsToRead).Returns(psnLogs);

			bool executed = false;
			var viewModel = new AvailableFaultsViewModel(_messenger, _locomotives, _addData.Object, _repository.Object, () => {
				executed = true;
			}, false);

			viewModel.Sections[0].PsnLogs[0].IsChecked = true;
			viewModel.Sections[1].Faults[0].IsChecked = true;
			viewModel.Sections[3].PsnPowerOnLogs[0].IsChecked = true;

			viewModel.NextCommand.Execute(null);

			Assert.That(executed, Is.True);
			Assert.That(faults.Count, Is.EqualTo(1));
			Assert.That(psnLogs.Count, Is.EqualTo(2));
		}

		[Test]
		public void SelectAllFaultsCommand_FaultsIsChecked() {
			var viewModel = CreateViewModel(false);
			viewModel.SelectAllFaultsCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.Faults)
					Assert.That(log.IsChecked, Is.True);
			}
		}

		[Test]
		public void UnselectAllFaultsCommand_FaultsIsUnchecked() {
			var viewModel = CreateViewModel(false);
			viewModel.UnselectAllFaultsCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.Faults)
					Assert.That(log.IsChecked, Is.False);
			}
		}

		[Test]
		public void SelectAllPsnCommand_PsnLogsIsChecked() {
			var viewModel = CreateViewModel(false);
			viewModel.SelectAllPsnCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.PsnLogs)
					Assert.That(log.IsChecked, Is.True);
			}
		}

		[Test]
		public void UnselectAllPsnCommand_PsnLogsIsUnchecked() {
			var viewModel = CreateViewModel(false);
			viewModel.UnselectAllPsnCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.PsnLogs)
					Assert.That(log.IsChecked, Is.False);
			}
		}

		[Test]
		public void SelectAllPsnPowerOnCommand_PsnPowerOnLogsIsChecked() {
			var viewModel = CreateViewModel(false);
			viewModel.SelectAllPsnPowerOnCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.PsnPowerOnLogs)
					Assert.That(log.IsChecked, Is.True);
			}
		}

		[Test]
		public void UnselectAllPsnPowerOnCommand_PsnPowerOnLogsIsUnchecked() {
			var viewModel = CreateViewModel(false);
			viewModel.UnselectAllPsnCommand.Execute(null);

			foreach (var sec in viewModel.Sections) {
				foreach (var log in sec.PsnPowerOnLogs)
					Assert.That(log.IsChecked, Is.False);
			}
		}
	}
}
