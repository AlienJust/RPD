using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    class SelectDeviceViewModelTest
    {
        private Mock<IMessenger> _messenger;
        private Mock<IAddDataParameters> _dataParamsMoq;
        private Mock<IUsbRemovableDrives> _usbDrives;
        private SelectUsbUsbDeviceViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _messenger = new Mock<IMessenger>();
            _messenger.Setup(m => m.Send(It.IsAny<AddDataWindowMessages>()));

            _dataParamsMoq = new Mock<IAddDataParameters>();
            _dataParamsMoq.SetupProperty(p => p.DevicePath);

            _usbDrives = new Mock<IUsbRemovableDrives>();
            _usbDrives.SetupGet(x => x.Items).
                Returns(new ObservableCollection<DriveInfo>());

            _viewModel = new SelectUsbUsbDeviceViewModel(_messenger.Object, _dataParamsMoq.Object, _usbDrives.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Object.Unregister(this);
        }

        [Test]
        public void NextCommand_SelectedDriveChanged_ChangesAddDataParametersDevicePathAndNavigatesToSelectPsnConfigurationPage()
        {
            _viewModel.SelectedDrive = new DriveInfo("p:\\");
            _viewModel.NextCommand.Execute(null);

            _dataParamsMoq.VerifySet(s => s.DevicePath = "p:\\");
            _messenger.Verify(m => m.Send(It.Is<AddDataWindowMessages>(p => p == AddDataWindowMessages.SelectPsnConfiguration)));
        }

        [Test]
        public void BackCommand_NavigatesToSelectDataSourcePage()
        {
            _viewModel.BackCommand.Execute(null);

            _messenger.Verify(m => m.Send(It.Is<AddDataWindowMessages>(p => p == AddDataWindowMessages.SelectDataSourcePage)));
        }
    }
}
