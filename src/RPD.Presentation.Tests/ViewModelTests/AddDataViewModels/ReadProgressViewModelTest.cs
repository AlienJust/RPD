using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPD.DAL;
using RPD.EventArgs;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.Presentation.Messages;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    public class ReadProgressViewModelTest
    {
        private Mock<IMessenger> _messenger;
        private Mock<IRepository> _repository;
        private Mock<IAddDataParameters> _addData;

        [SetUp]
        public void SetUp()
        {
            _messenger = new Mock<IMessenger>();
            _repository = new Mock<IRepository>();
            _addData = new Mock<IAddDataParameters>();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Object.Unregister(this);
        }

        [Test]
        public void Constructor_OpenCalled()
        {
            //arrange
            _repository.Setup(m => m.Open(It.IsAny<Action<OnCompleteEventArgs>>(), It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Verifiable();
            
            //act
            var viewModel = new ReadProgressViewModel(_messenger.Object, _repository.Object, _addData.Object);

            //assert
            _repository.Verify();
        }

        [Test]
        public void Constructor_OpenReturnsError_ErrorDialogShown()
        {
            //arrange
            _repository.Setup(m => m.Open(It.IsAny<Action<OnCompleteEventArgs>>(), It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Callback<Action<OnCompleteEventArgs>, Action<OnProgressChangeEventArgs>>(
                    (act, pr) => act(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "error"))).
                    Verifiable();

            _messenger.Setup(m => m.Send(It.IsAny<DialogMessage>(), It.Is<string>(p => p == AppMessages.ErrorDialogMessage))).
                Verifiable();

            //act
            var viewModel = new ReadProgressViewModel(_messenger.Object, _repository.Object, _addData.Object);

            //assert
            _repository.Verify();
            _messenger.Verify();
        }

        [Test]
        public void Constructor_OpenSuccess_AddDataParamsLocomotiveSetAndNavigatesAvailFaultsPage()
        {
            //arrange
            _addData.SetupProperty(m => m.ReadLocomotives);

            var list = new ObservableCollection<ILocomotive>() { new LocomotiveMock(){Name = "bobo1"} };

            _repository.Setup(m => m.Open(It.IsAny<Action<OnCompleteEventArgs>>(), It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Callback<Action<OnCompleteEventArgs>, Action<OnProgressChangeEventArgs>>(
                    (complete, progress) => complete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "ok", list))).
                Verifiable();

            _repository.Setup(m => m.Locomotives).
                Returns(() => list);

            _messenger.Setup(m => m.Send(It.Is<AddDataWindowMessages>(p => p == AddDataWindowMessages.AvailableFaultsPage))).
                Verifiable();

            //act
            var viewModel = new ReadProgressViewModel(_messenger.Object, _repository.Object, _addData.Object);

            //assert
            _repository.Verify();
            _messenger.Verify();

            Assert.That(_addData.Object.ReadLocomotives.Count, Is.GreaterThan(0));
            Assert.That(_addData.Object.ReadLocomotives[0].Name, Is.EqualTo(list[0].Name));
        }
    }
}
