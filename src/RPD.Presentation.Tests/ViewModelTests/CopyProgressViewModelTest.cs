using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Messages;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests
{
    [TestFixture]
    public class CopyProgressViewModelTest
    {
        private Mock<IAddDataParameters> _addDataParams;
        private Mock<IRepository> _repository;
        private CopyProgressViewModel _viewModel;
        private IMessenger _messenger;

        [SetUp]
        public void SetUp()
        {
            _addDataParams = new Mock<IAddDataParameters>();
            _repository = new Mock<IRepository>();

            InitializeSaveDataAsyncMock(false, OnCompleteEventArgs.CompleteResult.Ok);

            var loco = new LocomotiveMock() { Name = "Bobo" };
            var section = new SectionMock(loco);

            _addDataParams.SetupProperty(m => m.SourceRepository);
            _addDataParams.Object.SourceRepository = _repository.Object;

            _addDataParams.Setup(m => m.ReadLocomotives).
                Returns(new List<ILocomotive>() { loco });

            _addDataParams.Setup(m => m.FaultsToRead).
                Returns(new List<IFaultLog>() { new FaultLogMock(section), new FaultLogMock(section) });

            _addDataParams.Setup(m => m.PsnLogsToRead).
                Returns(new List<IPsnLog>() { new PsnLogMock(section, PsnLogType.PowerDepended, new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())), 
                    new PsnLogMock(section, PsnLogType.FixedLength, new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid())) });

            _messenger = new Messenger();
        }

        /// <summary>
        /// Инициализирует мок метода IAddDataParameters.SaveDataAsync
        /// </summary>
        /// <param name="completed">Если true, то будет вызван колбэк onComplete</param>
        /// <param name="completeResult">Результат для onComplete</param>
        private void InitializeSaveDataAsyncMock(bool completed, OnCompleteEventArgs.CompleteResult completeResult)
        {
            _repository.Setup(m => m.SaveDataAsync(It.IsAny<IRepository>(),
                                     It.IsAny<IList<IPsnLog>>(),
                                     It.IsAny<IList<IFaultLog>>(),
                                     It.IsAny<Action<OnCompleteEventArgs>>(),
                                     It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Callback<IRepository, IEnumerable<IPsnLog>, IEnumerable<IFaultLog>,
                            Action<OnCompleteEventArgs>, Action<OnProgressChangeEventArgs>>(
                    (r, list1, list2, onComplete, onProgress) =>
                    {
                        onProgress(new OnProgressChangeEventArgs(100));
                        if (completed)
                            onComplete(new OnCompleteEventArgs(completeResult, "ok"));
                    }).
                Verifiable();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(_viewModel);
        }

        [Test]
        public void MessengerPropertySet_OneLocomotivePassed_HeaderSet()
        {
            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            Assert.That(_viewModel.Header, Is.EqualTo("Bobo"));
        }

        [Test]
        public void MessengerPropertySet_MultipleLocomotivesPassed_HeaderSet()
        {
            _addDataParams.Setup(m => m.ReadLocomotives).
                Returns(new List<ILocomotive>()
                            {
                                new LocomotiveMock() { Name = "Bobo" },
                                new LocomotiveMock() { Name = "Lulu"},
                                new LocomotiveMock() { Name = "Momo"}
                            });

            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            Assert.That(_viewModel.Header, Is.EqualTo("Bobo, Lulu, Momo"));
        }

        [Test]
        public void MessengerPropertySet_FaultsAndPsnLogsStringSet()
        {
            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            Assert.That(_viewModel.FaultsString, Is.EqualTo("Копируется аварий: 2"));
            Assert.That(_viewModel.PsnLogString, Is.EqualTo("Копируется дампов магистрали ПСН: 2"));
        }

        [Test]
        public void MessengerPropertySet_SaveDataAsyncCalledAndCanNotClose()
        {
            bool closeMessageSent = false;
            _messenger.Register<ViewMessage>(this, Views.Views.CopyProggress, message => { closeMessageSent = true; });

            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            _viewModel.Close.Execute(null);

            _repository.Verify();

            Assert.That(closeMessageSent, Is.False); // должно быть false, т.к процесс копирования не закончен
        }

        [Test]
        public void MessengerPropertySet_CompleteResultIsError_ErrorDialogMessageSent()
        {
            InitializeSaveDataAsyncMock(true, OnCompleteEventArgs.CompleteResult.Error);

            bool dialogShown = false;
            bool closeMessageSent = false;
            _messenger.Register<DialogMessage>(this, AppMessages.CopyProgressError, message => { dialogShown = true; });
            _messenger.Register<ViewMessage>(this, Views.Views.CopyProggress, message => { closeMessageSent = true; });

            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            _viewModel.Close.Execute(null);

            Assert.That(dialogShown, Is.True);
            Assert.That(closeMessageSent, Is.True);
        }

        [Test]
        public void MessengerPropertySet_CompleteResultIsOk_FaultsStringSetAndCanClose()
        {
            InitializeSaveDataAsyncMock(true, OnCompleteEventArgs.CompleteResult.Ok);

            bool dialogShown = false;
            bool closeMessageSent = false;
            _messenger.Register<DialogMessage>(this, AppMessages.CopyProgressError, message => { dialogShown = true; });
            _messenger.Register<ViewMessage>(this, Views.Views.CopyProggress, message => { closeMessageSent = true; });

            _viewModel = new CopyProgressViewModel(_repository.Object);
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(_addDataParams.Object));

            _viewModel.Close.Execute(null);

            Assert.That(dialogShown, Is.False);
            Assert.That(closeMessageSent, Is.True);
            Assert.That(_viewModel.Progress, Is.EqualTo(100));
            Assert.That(_viewModel.FaultsString, Is.EqualTo("Скопировано аварий: 2"));
            Assert.That(_viewModel.PsnLogString, Is.EqualTo("Скопировано дампов магистрали ПСН: 2"));

        }
    }
}
