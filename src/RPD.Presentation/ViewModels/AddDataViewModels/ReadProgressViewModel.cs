using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Messages;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	/// <summary>
	/// Модель представления процесса вычитки данных с РПД.
	/// Generates Messages: 
	/// ViewMessage(Views.Views.AddData)
	/// AppMessages.AddDataWindow.AvailableFaultsPage 
	/// </summary>
	class ReadProgressViewModel : ViewModelBase, IProgressViewModel {
		private readonly IMessenger _messenger;
		private readonly IRepository _sourceRepo;
		private readonly IAddDataParameters _addDataParameters;

		private string _messageText;

		private bool _progressVisible;
		private int _progress;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="messenger">Мессенджер</param>
		/// <param name="sourceRepo">Репозиторий из которого будет производитья вычитка данных.</param>
		/// <param name="addDataParameters">Сюда будет записан результат вычитки.</param>
		public ReadProgressViewModel(IMessenger messenger, IRepository sourceRepo, IAddDataParameters addDataParameters) {
			_messenger = messenger;
			_sourceRepo = sourceRepo;
			_addDataParameters = addDataParameters;

			MessageText = "Вычитка данных из РПД. Пожалуйста, подождите...";
			ProgressVisible = true;
			Progress = 0;

			StartReading();
		}

		#region Private Methods

		void StartReading() {
			_sourceRepo.Open(OnComplete, OnProgressChange);
		}

		private void OnProgressChange(OnProgressChangeEventArgs onProgressChangeEventArgs) {
			Progress = onProgressChangeEventArgs.ProgressPercent;
		}

		private void OnComplete(OnCompleteEventArgs onCompleteEventArgs) {
			if (onCompleteEventArgs.ResultCode == OnCompleteEventArgs.CompleteResult.Error)
				RaiseOpenDeviceError(onCompleteEventArgs);
			else {
				//_addDataParameters.SourceRepository = _sourceRepo;
				_addDataParameters.ReadLocomotives = _sourceRepo.Locomotives;
				_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
						from: AddDataWindowMessages.ReadProgressPage,
						to: AddDataWindowMessages.AvailableFaultsPage));
				//_messenger.Send(AppMessages.AddDataWindow.AvailableFaultsPage);
			}
		}

		void RaiseOpenDeviceError(OnCompleteEventArgs onCompleteEventArgs) {
			// Сообщаем об ошибке.
			_messenger.Send(
					new DialogMessage(this, onCompleteEventArgs.Message,
							(res) => _messenger.Send(AddDataWindowMessages.SelectDataSourcePage)) {
						Caption = "Ошибка"
					},
					AppMessages.ErrorDialogMessage);
		}

		#endregion

		#region Implementation of IProgressViewModel

		public string MessageText {
			get => _messageText;
			set { Set(() => MessageText, ref _messageText, value); }
		}

		public bool ProgressVisible {
			get => _progressVisible;
			set { Set(() => ProgressVisible, ref _progressVisible, value); }
		}

		public int Progress {
			get => _progress;
			set { Set(() => Progress, ref _progress, value); }
		}

		#endregion
	}
}
