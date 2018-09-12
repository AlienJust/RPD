using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.Model.SelectionMasks;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;
using RPD.Presentation.ViewModels.DalViewModels;
using PsnChannelDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnChannelDesignTime;
using PsnMeterDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnMeterDesignTime;

namespace RPD.Presentation.Tests.ViewModelTests
{
    [TestFixture]
    public class MainViewModelTest
    {
        private Mock<IRepository> _repository;
        private Mock<ILoader> _loader;
        private Mock<IApplicationSettings> _appSettings;
        private Mock<ISelectionMasksStorage> _masks;
        private Mock<IRepositoryViewModelFactory> _repositoryViewModelFactory;
        private Mock<IRepositoryViewModel> _repositoryViewModel;
        private Mock<ISciChartViewModelFactory> _sciChartViewModelFactory;
        private Mock<ISciChartViewModel> _sciChartViewModel;
        private IUnityContainer _container;
        private IMessenger _messenger;
        private string _masksFileName;

        private MainViewModel _target;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepository>();
            _loader = new Mock<ILoader>();
            _container = new UnityContainer();

            _appSettings = new Mock<IApplicationSettings>();
            _appSettings.SetupProperty(m => m.LastExportFolder);

            _masks = new Mock<ISelectionMasksStorage>();

            // просто файл, который существует
            _masksFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            _appSettings.Setup(m => m.SelectionMasksFullFilePath).
                Returns(_masksFileName);

            _messenger = new Messenger();

            _repositoryViewModel = new Mock<IRepositoryViewModel>();
            _repositoryViewModel.SetupProperty(m => m.Locomotives);
            _repositoryViewModel.Object.Locomotives = new ObservableCollection<ILocomotiveViewModel>();
            GenerateLocomotives(_repositoryViewModel.Object.Locomotives);

            _repositoryViewModelFactory = new Mock<IRepositoryViewModelFactory>();
            _repositoryViewModelFactory.Setup(m => m.Create(It.IsAny<IRepository>())).
                Returns(_repositoryViewModel.Object).
                Verifiable();

            _sciChartViewModel = new Mock<ISciChartViewModel>();

            _sciChartViewModelFactory = new Mock<ISciChartViewModelFactory>();
            _sciChartViewModelFactory.Setup(m => m.Create()).
                Returns(_sciChartViewModel.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _messenger.Unregister(this);
        }

        private void GenerateLocomotives(ObservableCollection<ILocomotiveViewModel> locomotives)
        {
            var loco = new LocomotiveMock() { Name = "1" };
            loco.Sections.Add(GenerateSection(loco, "1"));
            loco.Sections.Add(GenerateSection(loco, "2"));
            locomotives.Add(new LocomotiveViewModel(loco));

            var loco2 = new LocomotiveMock() { Name = "2" };
            loco2.Sections.Add(GenerateSection(loco2, "3"));
            loco2.Sections.Add(GenerateSection(loco2, "4"));
            locomotives.Add(new LocomotiveViewModel(loco));
        }

        private static SectionMock GenerateSection(LocomotiveMock loco, string name)
        {
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

        private void CreateViewModel()
        {
            _target = new MainViewModel(_container, _loader.Object, _appSettings.Object, _masks.Object,
                _messenger, _repositoryViewModelFactory.Object, _sciChartViewModelFactory.Object, null);
        }

        private void CreateAndInitializeViewModel()
        {
            PrepareInitializeMocks(OnCompleteEventArgs.CompleteResult.Ok);
            CreateViewModel();
            _target.Initialize.Execute(null);
        }

        [Test]
        public void Constructor_LoadsSelectionMasksIfFileExists()
        {
            _masks.Setup(m => m.Load(It.Is<string>(p => p == _masksFileName))).
                Verifiable();

            CreateViewModel();

            _masks.Verify();
        }

        private void PrepareInitializeMocks(OnCompleteEventArgs.CompleteResult completeResult)
        {
            _appSettings.Setup(m => m.IsRepositoryPathSetted).Returns(true);
            _appSettings.Setup(m => m.RepositoryPath).Returns("c:\\");

            _repository.Setup(
                m => m.Open(It.IsAny<Action<OnCompleteEventArgs>>(),
                            It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Callback<Action<OnCompleteEventArgs>, Action<OnProgressChangeEventArgs>>(
                    (c, p) => c(new OnCompleteEventArgs(completeResult, ""))).
                Verifiable();

            _loader = new Mock<ILoader>();
            _loader.Setup(m => m.GetLocalDirectoryRepository(It.IsAny<string>())).Returns(_repository.Object);
        }

        [Test]
        public void InitializeCommand_AppSettingsRepositoryPathNotSet_ShowSettingsMessageSentAndRepositoryOpenCalled()
        {
            PrepareInitializeMocks(OnCompleteEventArgs.CompleteResult.Ok);
            _appSettings.Setup(m => m.IsRepositoryPathSetted).Returns(false);

            _repository.Setup(
                m => m.Open(It.IsAny<Action<OnCompleteEventArgs>>(),
                            It.IsAny<Action<OnProgressChangeEventArgs>>())).
                Verifiable();

            bool wasExecuted = false;
            _messenger.Register<MessageWithCallback>(this, Views.Views.Settings,
                                                     callback =>
                                                     {
                                                         wasExecuted = true;
                                                         callback.ProcessCallback();
                                                     });

            CreateViewModel();
            _target.Initialize.Execute(null);

            Assert.That(wasExecuted, Is.True);
            _repository.Verify();
        }

        [Test]
        public void InitializeCommand_AppSettingsRepositoryPathSet_IsRepositoryLoadedIsTrueAndRepositoryViewModelCreated()
        {
            CreateAndInitializeViewModel();

            _repository.Verify();
            _repositoryViewModelFactory.Verify(); // проверяем что был вызван Create

            Assert.That(_target.IsRepositoryLoaded, Is.True);
            Assert.That(_target.Repository, Is.Not.Null);
        }

        [Test]
        public void InitializeCommand_RepositoryOpenError_ErrorDialogMessageSent()
        {
            bool wasExecuted = false;
            _messenger.Register<DialogMessage>(this, AppMessages.ErrorDialogMessage,
                                               callback =>
                                               {
                                                   wasExecuted = true;
                                               });

            PrepareInitializeMocks(OnCompleteEventArgs.CompleteResult.Error);
            CreateViewModel();
            _target.Initialize.Execute(null);

            Assert.That(wasExecuted, Is.True);
        }

        [Test]
        public void ExportDataCommand_SomeLogsChecked_SaveDialogShownAndExportProgressShown()
        {
            var saveDialogExecuted = false;
            _messenger.Register<DialogMessage<SaveFileDialog>>(this,
                                                               message =>
                                                               {
                                                                   saveDialogExecuted = true;
                                                                   Assert.That(message.Dialog.CheckFileExists, Is.False);
                                                                   Assert.That(message.Dialog.CheckPathExists, Is.True);
                                                                   message.Result.DialogResult = DialogResult.OK;
                                                                   message.Dialog.FileName = "c:\\bobo\\1.rpd";
                                                                   message.ProcessResult();
                                                               });

            var progressExecuted = false;
            _messenger.Register<ViewMessage>(this, Views.Views.ExportProgress, message => progressExecuted = true);

            _repositoryViewModel.Object.Locomotives[0].Sections[0].PsnLogs[0].IsChecked = true;
            _repositoryViewModel.Object.Locomotives[0].Sections[0].PsnPowerOnLogs[0].IsChecked = true;
            _repositoryViewModel.Object.Locomotives[0].Sections[0].Faults[0].IsChecked = true;

            ExportLogsParametersBase exportLogsParams = null;
            _messenger.Register<SetViewModelParametersMessage<ExportLogsParametersBase>>(this,
                                                                                 message =>
                                                                                 exportLogsParams = message.Parameter);

            CreateAndInitializeViewModel();

            _target.ExportDataCommand.Execute(null);

            Assert.That(saveDialogExecuted, Is.True);
            Assert.That(progressExecuted, Is.True);
            Assert.That(exportLogsParams, Is.Not.Null);
            Assert.That(exportLogsParams.FileName, Is.EqualTo("c:\\bobo\\1.rpd"));
            Assert.That(exportLogsParams.PsnLogs.Count, Is.EqualTo(2));
            Assert.That(exportLogsParams.RpdLogs.Count, Is.EqualTo(1));
            Assert.That(_appSettings.Object.LastExportFolder, Is.EqualTo("c:\\bobo"));
        }

        private static Mock<ITrendViewModel> CreateTrendViewModel()
        {
            var trend = new Mock<ITrendViewModel>();
            trend.SetupProperty(m => m.IsTrendLoading);
            trend.SetupProperty(m => m.IsOnPlot);
            return trend;
        }

        [Test]
        public void IsTrendOnPlotChangedMessageReceived_IsOnPlotTrue_TrendAddedToPlot()
        {
            var trend = CreateTrendViewModel();

            CreateViewModel();

            _sciChartViewModel.Setup(m => m.AddTrend(It.IsAny<ITrendViewModel>(), It.IsAny<Action<OnCompleteEventArgs>>())).
                Callback<ITrendViewModel, Action<OnCompleteEventArgs>>(
                    (t, onComplete) =>
                    {
                        Assert.That(_target.IsTrendLoading, Is.True);
                        Assert.That(t.IsTrendLoading, Is.True);
                        onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, ""));
                    }).
                Verifiable();

            //Act
            trend.Object.IsOnPlot = true;
            _messenger.Send(new IsTrendOnPlotChangedMessage(trend.Object));

            //Assert
            _sciChartViewModel.Verify();
            Assert.That(_target.IsTrendLoading, Is.False);
            Assert.That(trend.Object.IsTrendLoading, Is.False);
        }

        [Test]
        public void IsTrendOnPlotChangedMessageReceived_IsOnPlotFalse_TrendAddedToPlot()
        {
            var trend = CreateTrendViewModel();

            _sciChartViewModel.Setup(m => m.RemoveTrend(It.IsAny<ITrendViewModel>())).
                Verifiable();

            CreateViewModel();

            //Act
            trend.Object.IsOnPlot = false;
            _messenger.Send(new IsTrendOnPlotChangedMessage(trend.Object));

            //Assert
            _sciChartViewModel.Verify();
            Assert.That(_target.IsTrendLoading, Is.False);
        }

        [Test]
        public void Can_Save_Selection_Masks()
        {
            _masks.SetupProperty(m => m.SelectionMasks);
            _masks.Object.SelectionMasks = new Dictionary<string, ISelectionMasksCollection>();

            var psnMeter = new PsnMeterDesignTime();
            psnMeter.PsnChannels.Add(new PsnChannelDesignTime("1") { IsOnPlot = false});
            psnMeter.PsnChannels.Add(new PsnChannelDesignTime("2") { IsOnPlot = true });
            psnMeter.PsnChannels.Add(new PsnChannelDesignTime("3") { IsOnPlot = false });
            psnMeter.PsnChannels.Add(new PsnChannelDesignTime("4") { IsOnPlot = true });

            CreateViewModel();

            //Act
            _target.SetContextMenuTrendsContainerCommand.Execute(psnMeter);
            _target.SaveSelectionMaskCommand.Execute(null);

            //Assert
            Assert.That(_target.SelectionMasksStorage.SelectionMasks.Count, Is.EqualTo(1));
            Assert.That(_target.SelectionMasksStorage.SelectionMasks[psnMeter.Name].Items.Count, Is.EqualTo(1));
            Assert.That(_target.SelectionMasksStorage.SelectionMasks[psnMeter.Name].Items[0].Items.Count, Is.EqualTo(2));
            Assert.That(_target.SelectionMasksStorage.SelectionMasks[psnMeter.Name].Items[0].Items[0].Name, Is.EqualTo("2"));
            Assert.That(_target.SelectionMasksStorage.SelectionMasks[psnMeter.Name].Items[0].Items[1].Name, Is.EqualTo("4"));
        }
    }
}
