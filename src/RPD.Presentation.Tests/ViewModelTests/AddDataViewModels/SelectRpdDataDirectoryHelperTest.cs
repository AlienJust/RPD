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
    public class SelectRpdDataDirectoryHelperTests
    {
        private SelectRpdDataDirectoryHelper _target;
        private IMessenger _messenger;

        private Mock<IApplicationSettings> _applicationSettings;


        [SetUp]
        public void SetUp()
        {
            _messenger = new Messenger();

            _applicationSettings = new Mock<IApplicationSettings>();
            _applicationSettings.SetupProperty(m => m.LastAddDataFolder);
            _applicationSettings.Setup(m => m.Save()).Verifiable();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        private void CreateTarget()
        {
            _target = new SelectRpdDataDirectoryHelper(_messenger, _applicationSettings.Object);
        }

        [Test]
        public void LastPathSet_PathSaved()
        {
            //arrange
            CreateTarget();

            //act
            _target.LastPath = "bobo";

            //assert
            _applicationSettings.Verify();
            Assert.That(_applicationSettings.Object.LastAddDataFolder, Is.EqualTo("bobo"));
        }
        
        [Test]
        public void ShowBrowseDialog_Conditions_PathReturned()
        {
            //arrange
            _messenger.Register<DialogMessage<FolderBrowserDialog>>(this, AppMessages.ShowSelectRdpDataPathDialog, 
                message =>
                    {
                        message.Dialog.SelectedPath = "c:\\bobo";
                        message.Result.DialogResult = DialogResult.OK;
                        message.ProcessResult();
                    });

            CreateTarget();

            //act
            string result = "";
            _target.ShowBrowseDialog(s => result = s);

            //assert
            Assert.That(result, Is.EqualTo("c:\\bobo"));
        }   
    } 
}