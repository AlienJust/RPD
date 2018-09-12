using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.Presentation.Messages;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Tests.ViewModelTests
{
    [TestFixture]
    public class SettingsViewModelTest
    {
        private IMessenger _messenger;
        private Mock<IApplicationSettings> _appSettings;
        private SettingsViewModel _viewModel;
        private const string ExistingPath = "c:\\windows";
        private bool _cancelExecuted = false;
        private bool _okExecuted = false;

        [SetUp]
        public void SetUp()
        {
            _cancelExecuted = false;
            _okExecuted = false;

            _messenger = new Messenger();
            _messenger.Register<ViewMessage>(this, Views.Views.Settings,
                message =>
                    {
                        if (message.Action == ViewAction.Close)
                            _cancelExecuted = true;
                    });
            
            _appSettings = new Mock<IApplicationSettings>();
            _appSettings.SetupProperty(m => m.IsRepositoryPathSetted);
            _appSettings.SetupProperty(m => m.RepositoryPath);
            _appSettings.Setup(m => m.Save()).
                Callback(() => _okExecuted = true).
                Verifiable();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        [Test]
        public void Constructor_AppSettingsRepositoryPathNotEmptyAndExists_RepositoryPathPropertyNotEmpty()
        {
            _appSettings.Object.IsRepositoryPathSetted = true;
            _appSettings.Object.RepositoryPath = ExistingPath;

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.Cancel.Execute(null);
            _viewModel.OK.Execute(null);

            Assert.That(_viewModel.IsRepositoryPathSetted, Is.True);
            Assert.That(_viewModel.RepositoryPath, Is.EqualTo(ExistingPath));
            Assert.That(_viewModel.StatusString, Is.Empty);

            Assert.That(_cancelExecuted, Is.True);
            Assert.That(_okExecuted, Is.True);
        }

        [Test]
        public void Constructor_AppSettingsRepositoryPathNotEmptyAndNotExists_IsRepositoryPathSettedIsFalse()
        {
            _appSettings.Object.IsRepositoryPathSetted = true;
            _appSettings.Object.RepositoryPath = "bobo";

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.Cancel.Execute(null);
            _viewModel.OK.Execute(null);

            Assert.That(_viewModel.IsRepositoryPathSetted, Is.False);
            Assert.That(_viewModel.RepositoryPath, Is.EqualTo("bobo"));
            Assert.That(_viewModel.StatusString, Is.EqualTo("Указанная папка не существует."));
            Assert.That(_cancelExecuted, Is.False);
            Assert.That(_okExecuted, Is.False);
        }

        [Test]
        public void Constructor_AppSettingsIsRepositoryPathSettedIsFalse_RepositoryPathPropertyIsEmpty()
        {
            _appSettings.Object.IsRepositoryPathSetted = false;
            _appSettings.Object.RepositoryPath = "bobo";

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.Cancel.Execute(null);
            _viewModel.OK.Execute(null);

            Assert.That(_viewModel.IsRepositoryPathSetted, Is.False);
            Assert.That(_viewModel.RepositoryPath, Is.Empty);
            Assert.That(_viewModel.StatusString, Is.EqualTo("Укажите папку хранилища."));

            Assert.That(_cancelExecuted, Is.False);
            Assert.That(_okExecuted, Is.False);
        }

        [Test]
        public void Constructor_AppSettingsRepositoryPathExistsButUserSetInvalidPathAndCancel_StatusStringIsSetAndCancelCanExecute()
        {
            _appSettings.Object.IsRepositoryPathSetted = true;
            _appSettings.Object.RepositoryPath = ExistingPath;

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.RepositoryPath = "bobo";
            _viewModel.Cancel.Execute(null);
            _viewModel.OK.Execute(null);

            Assert.That(_viewModel.IsRepositoryPathSetted, Is.False);
            Assert.That(_viewModel.StatusString, Is.EqualTo("Указанная папка не существует."));

            Assert.That(_cancelExecuted, Is.True);
            Assert.That(_okExecuted, Is.False);
        }

        
        [Test]
        public void Constructor_RepositoryPathIsValid_CancelCanExecuteAndOkCanExecuteAndSaveCalled()
        {
            _appSettings.Object.IsRepositoryPathSetted = false;
            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);

            _viewModel.RepositoryPath = ExistingPath;
            _viewModel.Cancel.Execute(null);
            _viewModel.OK.Execute(null);

            Assert.That(_viewModel.IsRepositoryPathSetted, Is.True);

            Assert.That(_cancelExecuted, Is.True);
            Assert.That(_okExecuted, Is.True);
            Assert.That(_appSettings.Object.IsRepositoryPathSetted, Is.True);
            Assert.That(_appSettings.Object.RepositoryPath, Is.EqualTo(ExistingPath));

            _appSettings.Verify();
        }

        [Test]
        public void BrowseCommand_DialogResultIsOk_RepositoryPathProppertySet()
        {
            bool wasExecuted = false;

            _messenger.Register<DialogMessage<FolderBrowserDialog>>(this, Views.Views.Settings,
                (msg) =>
                {
                    wasExecuted = true;
                    msg.Result.DialogResult = DialogResult.OK;
                    msg.Dialog.SelectedPath = ExistingPath;
                    msg.ProcessResult();
                });

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.BrowseRepositoryPath.Execute(null);

            Assert.That(wasExecuted, Is.True);
            Assert.That(_viewModel.IsRepositoryPathSetted, Is.True);
            Assert.That(_viewModel.RepositoryPath, Is.EqualTo(ExistingPath));
        }

        [Test]
        public void BrowseCommand_DialogResultIsNotOkAndAppSettingsIsRepoPathSetIsFalse_RepositoryPathProppertyIsFalse()
        {
            _appSettings.Object.IsRepositoryPathSetted = false;

            bool wasExecuted = false;
            _messenger.Register<DialogMessage<FolderBrowserDialog>>(this, Views.Views.Settings,
                (msg) =>
                {
                    wasExecuted = true;
                    msg.Result.DialogResult = DialogResult.Cancel;
                    msg.ProcessResult();
                });

            _viewModel = new SettingsViewModel(_appSettings.Object, _messenger);
            _viewModel.BrowseRepositoryPath.Execute(null);

            Assert.That(wasExecuted, Is.True);
            Assert.That(_viewModel.IsRepositoryPathSetted, Is.False);
        }
    }
}
