using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using System.Linq;
using RPD.EventArgs;
using System.Windows.Input;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.ViewModels {
	/// <summary>
	/// Модель представления окна копирования аварий.
	/// Sends messages: 
	/// AppMessages.CloseCopyProgressWindow,
	/// CaptureNewFaultsMessage
	/// Receives messages:
	/// IAddDataParameters.
	/// </summary>
	class CopyProgressViewModel : ViewModelBase, ICopyProgressViewModel, IMessageable {
		private IAddDataParameters _addDataParameters;
		private readonly IRepository _destRepository;
		private IMessenger _messenger;
		private bool _canClose = false;

		#region Private properties Fields

		private int _progress = 0;
		private string _faultsString = "";
		private string _psnLogString = "";
		private string _header;
		private string _windowTitle;
		private string _status;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destRepository">Репозиторий, в который нужно скопировать данные.</param>
		public CopyProgressViewModel(IRepository destRepository) {
			_destRepository = destRepository;
			WindowTitle = "Копирование данных РПД";

			Close = new RelayCommand(CloseExecute, () => _canClose);
			Status = "Копирование завершено успешно";
		}

		#region Implementation of IMessageable

		public void StartMessaging(IMessenger messenger) {
			_messenger = messenger;

			_messenger.Register<SetViewModelParametersMessage<IAddDataParameters>>(this,
					(msg) => {
						_addDataParameters = msg.Parameter;

						SetHeader();

						StartReadingFaults();
					});
		}

		#endregion

		private void SetHeader() {
			if (_addDataParameters.ReadLocomotives.Count == 1)
				Header = _addDataParameters.ReadLocomotives[0].Name;
			else {
				var name = _addDataParameters.ReadLocomotives.Aggregate("", (current, loco) => current + (loco.Name + ", "));
				Header = name.Trim(' ', ',');
			}
		}

		void StartReadingFaults() {
			FaultsString = "Копируется аварий: " + _addDataParameters.FaultsToRead.Count;
			PsnLogString = "Копируется дампов магистрали ПСН: " + _addDataParameters.PsnLogsToRead.Count;

			// Посылаем глобальное сообщение
			Messenger.Default.Send(new CaptureNewFaultsMessage(true));

			_destRepository.SaveDataAsync(_addDataParameters.SourceRepository,
					_addDataParameters.PsnLogsToRead,
					_addDataParameters.FaultsToRead,
					OnReadFaultsComplete,
					OnReadFaultsProgress);
		}

		private void OnReadFaultsComplete(OnCompleteEventArgs onCompleteEventArgs) {
			_canClose = true;

			if (onCompleteEventArgs.ResultCode == OnCompleteEventArgs.CompleteResult.Error) {
				_messenger.Send(
						new DialogMessage(this, onCompleteEventArgs.Message, (p) => { }) {
							Button = MessageBoxButton.OK,
							Caption = "Ошибка",
							Icon = MessageBoxImage.Error,
						},
				AppMessages.CopyProgressError);

				Progress = 100;
				FaultsString = "";
				PsnLogString = "";

				Status = "При копировании возникла ошибка";
				CommandManager.InvalidateRequerySuggested();
				_messenger.Send(new CaptureNewFaultsMessage(false));
				return;
			}

			Progress = 100;
			CommandManager.InvalidateRequerySuggested();//заставляем вид обновить Command.CanExecute

			FaultsString = "Скопировано аварий: " + _addDataParameters.FaultsToRead.Count;
			PsnLogString = "Скопировано дампов магистрали ПСН: " + _addDataParameters.PsnLogsToRead.Count;

			_messenger.Send(new CaptureNewFaultsMessage(false));
		}

		void OnReadFaultsProgress(OnProgressChangeEventArgs args) {
			Progress = args.ProgressPercent;
		}

		#region Presentation Members

		/// <summary>
		/// Прогресс копирования.
		/// </summary>
		public int Progress {
			get => _progress;
			set { Set(() => Progress, ref _progress, value); }
		}

		public int Minimum => 0;

		public int Maximum => 100;

		public string WindowTitle {
			get => _windowTitle;
			set { Set(() => WindowTitle, ref _windowTitle, value); }
		}

		public string Header {
			get => _header;
			set { Set(() => Header, ref _header, value); }
		}

		public string Status {
			get => _status;
			set { Set(() => Status, ref _status, value); }
		}


		/// <summary>
		/// Описание количества аварий, которые копируются
		/// </summary>
		public string FaultsString {
			get => _faultsString;
			set { Set(() => FaultsString, ref _faultsString, value); }
		}

		public string PsnLogString {
			get => _psnLogString;
			set { Set(() => PsnLogString, ref _psnLogString, value); }
		}

		#endregion // Presentation Members


		#region Commands

		public RelayCommand Close { get; set; }

		private void CloseExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Close), Views.Views.CopyProggress);
		}

		#endregion //Commands

		public override void Cleanup() {
			_messenger.Unregister(this);
			base.Cleanup();
		}
	}
}