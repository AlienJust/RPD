using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.Presentation.Messages;
using RPD.Presentation.Settings;
using RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    public class SelectExportImagePathHelperTests
    {
        private SelectExportImagePathHelper _target;
        private Mock<IApplicationSettings> _applicationSettings;
        private IMessenger _messenger;
        private const string FileName = "c:\\bobo\\1.rpd";

        [SetUp]
        public void SetUp()
        {
            _applicationSettings = new Mock<IApplicationSettings>();
            _applicationSettings.SetupProperty(m => m.LastExportFolder);
            _applicationSettings.Setup(m => m.Save()).Verifiable();

            _messenger = new Messenger();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        private void CreateTarget()
        {
            _target = new SelectExportImagePathHelper(_messenger, _applicationSettings.Object, () => { });
        }

        [Test]
        public void LastPathSet_PathSavedCorretly()
        {
            //arrange
            CreateTarget();

            //act
            _target.LastPath = FileName;

            //assert
            _applicationSettings.Verify();
            Assert.That(_applicationSettings.Object.LastExportFolder, Is.EqualTo("c:\\bobo"));
        }

        [Test]
        public void ShowBrowseDialog_Conditions_FileNameReturned()
        {
            //arrange
            bool executed = false;
            
            _messenger.Register<DialogMessage<SaveFileDialog>>(this, AppMessages.ShowSelectRdpDataPathDialog,
                message =>
                {
                    executed = true;
                    Assert.That(message.Dialog.CheckFileExists, Is.False);
                    Assert.That(message.Dialog.CheckPathExists, Is.True);
                    message.Dialog.FileName = FileName;
                    message.Result.DialogResult = DialogResult.OK;
                    message.ProcessResult();
                });

            CreateTarget();

            //act
            string result = "";
            _target.ShowBrowseDialog(s => result = s);

            //assert
            Assert.That(executed, Is.True);
            Assert.That(result, Is.EqualTo(FileName));
        }  
    }
}