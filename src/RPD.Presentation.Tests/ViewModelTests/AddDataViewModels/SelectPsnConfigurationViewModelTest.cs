using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.DAL;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Messages;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    public class SelectPsnConfigurationViewModelTests
    {
        private SelectPsnConfigurationViewModel _target;
        private Mock<ILoader> _loader;
        private Mock<IAddDataParameters> _addDataParameters;
        private IMessenger _messenger;
        private List<PsnConfigurationMock> _configs;
        private Mock<IDeviceNumberToPsnConfigurationRepository> _deviceConfigsMock;
        //private int _deviceNumber;

        Dictionary<int, string> _savedConfigs;
            
        [SetUp]
        public void SetUp()
        {
            _messenger = new Messenger();

            _configs = new List<PsnConfigurationMock>();

            _loader = new Mock<ILoader>();
            _loader.Setup(m => m.AvailablePsnConfigruations).Returns(() => _configs);

            _addDataParameters = new Mock<IAddDataParameters>();
            _deviceConfigsMock = new Mock<IDeviceNumberToPsnConfigurationRepository>();

            _savedConfigs = new Dictionary<int, string>();
            _deviceConfigsMock.Setup(m => m.Configurtions).Returns(_savedConfigs);
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        private void CreateTarget(int deviceNumber)
        {
            _target = new SelectPsnConfigurationViewModel(_loader.Object, _messenger, 
                _deviceConfigsMock.Object, _addDataParameters.Object, deviceNumber);
        }

        private void FillSampleData()
        {
            _configs.Add(new PsnConfigurationMock("1", "1", "1", Guid.NewGuid()));
            _configs.Add(new PsnConfigurationMock("1", "1", "1", Guid.NewGuid()));
            _configs.Add(new PsnConfigurationMock("1", "1", "1", Guid.NewGuid()));
        }

        [Test(Description = "it displays previously selected configuration")]
        public void Constructor_AppSettingsLastSelectedConfigIsSet_SelectedConfigurationIsSet()
        {
            //arrange
            FillSampleData();
            _savedConfigs.Add(1, "2");

            //act
            CreateTarget(1);

            //assert
            Assert.That(_target.SelectedConfiguration, Is.Not.Null);
            Assert.That(_target.SelectedConfiguration.Model.Id.ToString(), Is.EqualTo("2"));
        }

        [Test]
        public void Constructor_NoConfigigurationForDeviceSaved_SelectedConfigurationIsNull()
        {
            //arrange
            FillSampleData();
            _savedConfigs.Add(2, "2");

            //act
            CreateTarget(1);

            //assert
            Assert.That(_target.SelectedConfiguration, Is.Null);
        }

        [Test]
        public void NextCommandExecute_SelectedConfigIsNull_NextCommandCanNotExecute()
        {
            //arrange
            FillSampleData();
            CreateTarget(1);

            //act
            var can = _target.NextCommand.CanExecute(null);

            //assert
            Assert.That(can, Is.False);
        }

        [Test]
        public void NextCommandExecute_SelectedConfigSetAndAddDataParamsSet()
        {
            //arrange
           
            _deviceConfigsMock.Setup(m => m.Save()).Verifiable();
            
            _addDataParameters.SetupProperty(m => m.PsnConfiguration);

            bool messageSent = false;

            _messenger.Register<NavigateMessage<AddDataWindowMessages>>(this,
                (m) =>
                {
                    if (m.To == AddDataWindowMessages.ReadProgressPage)
                        messageSent = true;
                });

            FillSampleData();
            CreateTarget(1);

            _target.SelectedConfiguration = _target.AvailableConfigurations[1];

            //act
            _target.NextCommand.Execute(null);

            //assert
            _deviceConfigsMock.Verify();

            Assert.That(_deviceConfigsMock.Object.Configurtions.ContainsKey(1), Is.True);
            Assert.That(_deviceConfigsMock.Object.Configurtions[1], Is.EqualTo("2"));
            Assert.That(_addDataParameters.Object.PsnConfiguration, Is.Not.Null);
            Assert.That(_addDataParameters.Object.PsnConfiguration.Id.ToString(), Is.EqualTo("2"));
            Assert.That(messageSent, Is.True);
        }


        [Test]
        public void BackCommandExecute_ReturnsToAddDataSource()
        {
            //arrange
            bool executed = false;

            _messenger.Register<NavigateMessage<AddDataWindowMessages>>(this, 
                (m) =>
                {
                    if (m.To == AddDataWindowMessages.SelectDataSourcePage)
                        executed = true;
                });

            CreateTarget(1);

            //act
            _target.BackCommand.Execute(null);

            //assert
            Assert.That(executed, Is.True);
        }
    }
}