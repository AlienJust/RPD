using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Tests.ViewModelTests
{
    [TestFixture]
    public class ExportProgressViewModelTest
    {
        private Mock<ILoader> _loader;
        private Mock<IRepository> _repository;
        private ExportProgressViewModel _viewModel;
        private IMessenger _messenger;
        private const string FileName = "bobo";
        private bool _closeExecuted;

        [SetUp]
        public void SetUp()
        {
            _closeExecuted = false;

            _repository = new Mock<IRepository>();

            InitializeSaveDataAsync(false, OnCompleteEventArgs.CompleteResult.Ok, 50);

            _loader = new Mock<ILoader>();
            _loader.Setup(m => m.GetZippedRepository(It.Is<string>(f => f == FileName))).
                Returns(_repository.Object).
                Verifiable();

            _viewModel = new ExportProgressViewModel(_loader.Object);

            _messenger = new Messenger();
            _messenger.Register<ViewMessage>(this, Views.Views.ExportProgress, message =>
            {
                if (message.Action == ViewAction.Close)
                    _closeExecuted = true;
            });
        }

        private void InitializeSaveDataAsync(bool callOnComplete, OnCompleteEventArgs.CompleteResult completeResult, int progress)
        {
            _repository.Setup(
                m => m.SaveDataAsync(It.IsAny<IRepository>(),
                                     It.IsAny<IEnumerable<IPsnLog>>(),
                                     It.IsAny<IEnumerable<IFaultLog>>(),
                                     It.IsAny<Action<OnCompleteEventArgs>>(),
                                     It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Callback<IRepository, IEnumerable<IPsnLog>, IEnumerable<IFaultLog>,
                            Action<OnCompleteEventArgs>, Action<OnProgressChangeEventArgs>>(
                    (r, list1, list2, onComplete, onProgress) =>
                    {
                        if (callOnComplete)
                            onComplete(new OnCompleteEventArgs(completeResult, ""));

                        onProgress(new OnProgressChangeEventArgs(progress));
                    }).
                Verifiable();
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        private void SendMessageToViewModel()
        {
            _viewModel.StartMessaging(_messenger);
            _messenger.Send(new SetViewModelParametersMessage<ExportLogsParametersBase>(new ExportLogsParametersBase() { FileName = FileName }));
        }

        [Test]
        public void StartMessaging_ViewModelParametersMessageSent_GetDataWriterCalledAndWriteAsyncCalled()
        {
            SendMessageToViewModel();

            _loader.Verify();
            _repository.Verify();
        }

        [Test]
        public void StartMessaging_CopyInProgress_CloseCanNotExecute()
        {
            SendMessageToViewModel();
            _viewModel.Close.Execute(null);

            Assert.That(_closeExecuted, Is.False);
        }

        [Test]
        public void StartMessaging_WriteAsyncOnCompleteCalledResultError_ErrorDialogMessageSentAndCanClose()
        {
            InitializeSaveDataAsync(true, OnCompleteEventArgs.CompleteResult.Error, 50);

            bool dialogShown = false;
            _messenger.Register<DialogMessage>(this, Views.Views.ExportProgress, message => dialogShown = true);

            SendMessageToViewModel();
            _viewModel.Close.Execute(null);

            Assert.That(dialogShown, Is.True);
            Assert.That(_closeExecuted, Is.True);
        }

        [Test]
        public void StartMessaging_WriteAsyncCompleteResultOk_CloseCanExecute()
        {
            bool closeExecuted = false;
            _messenger.Register<ViewMessage>(this, Views.Views.ExportProgress, message =>
            {
                if (message.Action == ViewAction.Close)
                    closeExecuted = true;
            });

            InitializeSaveDataAsync(true, OnCompleteEventArgs.CompleteResult.Ok, 100);
            SendMessageToViewModel();

            _viewModel.Close.Execute(null);

            Assert.That(_viewModel.Progress, Is.EqualTo(100));
            Assert.That(_viewModel.Header, Is.EqualTo("Данные успешно записаны в файл " + FileName));
            Assert.That(closeExecuted, Is.True);
        }
    }
}

