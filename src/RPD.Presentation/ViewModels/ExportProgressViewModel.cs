using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Utils.Messages;

namespace RPD.Presentation.ViewModels {
	internal class ExportProgressViewModel : ViewModelBase, ICopyProgressViewModel, IMessageable {
		private readonly ILoader _loader;
		private string _fileName;
		private int _progress;
		private string _header;
		private IMessenger _messenger;
		private bool _canClose;
		private string _title;

		public ExportProgressViewModel(ILoader loader) {
			_loader = loader;

			Minimum = 0;
			Maximum = 100;
			Close = new RelayCommand(CloseExecute, () => _canClose);
		}

		#region Implementation of IMessageable

		public void StartMessaging(IMessenger messenger) {
			_messenger = messenger;

			_messenger.Register<SetViewModelParametersMessage<LogsParameters>>(this, RemoveLogs);
			_messenger.Register<SetViewModelParametersMessage<ExportLogsParametersBase>>(this, ExportLogs);
		}

		#endregion

		private void ExportLogs(SetViewModelParametersMessage<ExportLogsParametersBase> p) {
			WindowTitle = "Экспорт данных РПД";
			Status = "Копирование завершено успешно";
			_fileName = p.Parameter.FileName;

			var repo = _loader.GetZippedRepository(_fileName);
			repo.Open(args => { }, args => { });
			_repository = p.Parameter.Repository;
			repo.SaveDataAsync(p.Parameter.Repository, p.Parameter.PsnLogs, p.Parameter.RpdLogs, OnExportComplete, OnProgressChange);
		}

		private void RemoveLogs(SetViewModelParametersMessage<LogsParameters> p) {
			_repository = p.Parameter.Repository;

			WindowTitle = "Удаление данных РПД";
			Status = "Удаление завершено успешно";

			p.Parameter.Repository.Remove(p.Parameter.PsnLogs, OnRemoveComplete);
		}



		private IRepository _repository;


		private void CloseExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Close), Views.Views.ExportProgress);
		}

		private void OnProgressChange(OnProgressChangeEventArgs onProgressChangeEventArgs) {
			Progress = onProgressChangeEventArgs.ProgressPercent;
		}

		private void OnExportComplete(OnCompleteEventArgs onCompleteEventArgs) {
			_canClose = true;

			if (onCompleteEventArgs.ResultCode == OnCompleteEventArgs.CompleteResult.Error) {
				ShowError(onCompleteEventArgs.Message, "При копировании возникла ошибка");
				return;
			}

			Header = "Данные успешно записаны в файл " + _fileName;
			Progress = 100;
			CommandManager.InvalidateRequerySuggested();//заставляем вид обновить Command.CanExecute
		}

		private void OnRemoveComplete(OnCompleteEventArgs onCompleteEventArgs) {
			_canClose = true;

			if (onCompleteEventArgs.ResultCode == OnCompleteEventArgs.CompleteResult.Error) {
				ShowError(onCompleteEventArgs.Message, "При удалении возникла ошибка");
				return;
			}

			Header = "Данные удалены";
			Progress = 100;
			CommandManager.InvalidateRequerySuggested();//заставляем вид обновить Command.CanExecute
		}

		private void ShowError(string errorMessage, string headerText) {
			_messenger.Send(
					new DialogMessage(this, errorMessage, (p) => { }) {
						Button = MessageBoxButton.OK,
						Caption = "Ошибка",
						Icon = MessageBoxImage.Error,
					},
					Views.Views.ExportProgress);

			Progress = 100;
			FaultsString = "";
			PsnLogString = "";
			Status = headerText;
			CommandManager.InvalidateRequerySuggested();
		}

		#region Implementation of ICopyProgressViewModel

		public int Progress {
			get => _progress;
			set { Set(() => Progress, ref _progress, value); }
		}


		public int Minimum { get; }
		public int Maximum { get; }

		public string WindowTitle {
			get => _title;
			set { Set(() => WindowTitle, ref _title, value); }
		}

		public string Header {
			get => _header;
			set { Set(() => Header, ref _header, value); }
		}

		public string Status { get; set; }

		public string FaultsString { get; set; }
		public string PsnLogString { get; set; }
		public RelayCommand Close { get; set; }

		#endregion

		public override void Cleanup() {
			_messenger.Unregister(this);
			base.Cleanup();
		}
	}
}
