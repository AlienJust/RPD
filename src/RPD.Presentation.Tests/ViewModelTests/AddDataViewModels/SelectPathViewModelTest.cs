using System;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.Presentation.ViewModels.AddDataViewModels;
using RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    public class SelectPathViewModelTest
    {
        private Mock<IMessenger> _messenger;
        private Mock<IAddDataParameters> _addDataParams;
        private Mock<ISelectPathHelperViewModel> _selectPathHelper;
        private SelectPathViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _messenger = new Mock<IMessenger>();
            _addDataParams = new Mock<IAddDataParameters>();
            _addDataParams.SetupProperty(m => m.DevicePath);

            _selectPathHelper = new Mock<ISelectPathHelperViewModel>();
            _selectPathHelper.SetupGet(m => m.ToolTipText).Returns("1");
            _selectPathHelper.SetupGet(m => m.Label).Returns("2");
            _selectPathHelper.SetupProperty(m => m.LastPath);
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Object.Unregister(this);
        }

        private void CreateViewModel()
        {
            _viewModel = new SelectPathViewModel(_addDataParams.Object, _selectPathHelper.Object);
        }

        [Test]
        public void Constructor_ToolTipAndLabelAndLastPathSet()
        {
            _selectPathHelper.Object.LastPath = "path";

            CreateViewModel();

            Assert.That(_viewModel.ToolTipText, Is.EqualTo("1"));
            Assert.That(_viewModel.Label, Is.EqualTo("2"));
            Assert.That(_viewModel.Path, Is.EqualTo("path"));
        }

        [Test]
        public void PathPropertySet_ValueEmpty_IsPathSetFalse()
        {
            CreateViewModel();
            _viewModel.Path = string.Empty;

            Assert.That(_viewModel.IsPathSet, Is.False);
        }


        [Test]
        public void PathPropertySet_ValueCorrect_IsPathSetTrueAddDataParamsDevicePathSetAndPathHelperLastPathSet()
        {
            const string path = "c:\\";

            CreateViewModel();
            _viewModel.Path = path;

            Assert.That(_viewModel.IsPathSet, Is.True);
            Assert.That(_addDataParams.Object.DevicePath, Is.EqualTo(path));
            Assert.That(_selectPathHelper.Object.LastPath, Is.EqualTo(path));
        }

        [Test]
        public void BrowseCommand_PathHelper_BrowseCalled()
        {
            const string path = "c:\\";

            _selectPathHelper.Setup(m => m.ShowBrowseDialog(It.IsAny<Action<string>>())).
                Callback<Action<string>>(m => m(path)).
                Verifiable();

            CreateViewModel();

            _viewModel.BrowseCommand.Execute(null);

            Assert.That(_viewModel.Path, Is.EqualTo(path));
        }

        /*
        [Test]
        public void Contructor_SelectFileTrue_PathIsSetFromAppSetting()
        {
            const string path = "rpdpath";
            _appSettings.SetupProperty(m => m.LastRpdDataImageFile, path);
            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, true);

            Assert.That(viewModel.Path, Is.EqualTo(path));
            Assert.That(viewModel.IsPathSet, Is.True);
        }

        [Test]
        public void Constructor_SelectFileFalse_PathIsSetFromAppSetting()
        {
            const string path = "folder";
            _appSettings.SetupProperty(m => m.LastAddDataFolder, path);
            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, false);

            Assert.That(viewModel.Path, Is.EqualTo(path));
            Assert.That(viewModel.IsPathSet, Is.True);
        }

        [Test]
        public void Constructor_LastPathIsEmpty_IsPathIsFalse()
        {
            var path = "";
            _appSettings.SetupProperty(m => m.LastAddDataFolder, path);
            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, true);

            Assert.That(viewModel.IsPathSet, Is.False);
        }

        [Test]
        public void PathSet_SelectFileFalse_IsPathSetAndAddDataParametersDevicePathChangesAndAppSettingsSaveCalled()
        {
            _appSettings.SetupProperty(m => m.LastAddDataFolder);

            var viewModel = TestPathSetCommon(false);

            _appSettings.VerifySet(a => a.LastAddDataFolder = viewModel.Path);
        }

        [Test]
        public void PathSet_SelectFileTrue_IsPathSetAndAddDataParametersDevicePathChangesAndAppSettingsSaveCalled()
        {
            _appSettings.SetupProperty(m => m.LastRpdDataImageFile);

            var viewModel = TestPathSetCommon(true);

            _appSettings.VerifySet(a => a.LastRpdDataImageFile = viewModel.Path);
            
        }

        private SelectPathViewModel TestPathSetCommon(bool selectFile)
        {
            _appSettings.Setup(m => m.Save());
            _addDataParams.SetupProperty(p => p.DevicePath);

            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, selectFile);

            Assert.That(viewModel.IsPathSet, Is.False);

            viewModel.Path = "k";

            Assert.That(viewModel.IsPathSet, Is.True);
            _addDataParams.VerifySet(a => a.DevicePath = viewModel.Path);
            _appSettings.Verify(a => a.Save(), Times.Once);

            return viewModel;
        }

        [Test]
        public void BrowseCommand_SelectFileTrue_OpenFileDialogShown()
        {
            TestBrowseCommand<OpenFileDialog>(true);
        }

        [Test]
        public void BrowseCommand_SelectFileFalse_FolderBrowserDialogShown()
        {
            TestBrowseCommand<FolderBrowserDialog>(false);
        }

        private void TestBrowseCommand<TDialogType>(bool selectFile) where TDialogType: CommonDialog
        {
            _messenger.Setup(m => m.Send(It.Is<DialogMessage<TDialogType>>(p => true),
                            It.Is<string>(p => p == AppMessages.ShowSelectRdpDataPathDialog))).
                Verifiable();

            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, selectFile);
            viewModel.BrowseCommand.Execute(null);

            _messenger.Verify();
        }

        [Test]
        public void NextCommand_PathIsEmpty_CanNotNavigateToReadProgressPage()
        {
            _messenger.Setup(m => m.Send(It.IsAny<AppMessages.AddDataWindow>()));

            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, true);
            viewModel.Path = "";
            viewModel.NextCommand.Execute(null);

            _messenger.Verify(m => m.Send(It.IsAny<AppMessages.AddDataWindow>()), Times.Never);
        }

        [Test]
        public void NextCommand_PathIsNotEmpty_CanNavigateToReadProgressPage()
        {
            _messenger.Setup(m => m.Send(It.Is<AppMessages.AddDataWindow>(p => p == AppMessages.AddDataWindow.ReadProgressPage))).
                Verifiable();

            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, true)
                                {Path = "c:\\lilu"};
            viewModel.NextCommand.Execute(null);

            _messenger.Verify();
        }

        [Test]
        public void BackCommand_NavigatesToSelectDataSourcePage()
        {
            _messenger.Setup(m => m.Send(It.Is<AppMessages.AddDataWindow>(p => p == AppMessages.AddDataWindow.SelectDataSourcePage))).
                Verifiable();

            var viewModel = new SelectPathViewModel(_messenger.Object, _addDataParams.Object, _appSettings.Object, true);
            viewModel.BackCommand.Execute(null);

            _messenger.Verify();
        }
        */
    }
}
