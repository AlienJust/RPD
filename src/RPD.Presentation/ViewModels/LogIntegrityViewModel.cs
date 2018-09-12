using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;

namespace RPD.Presentation.ViewModels {
	class LogIntegrityViewModel : ViewModelBase, ILogIntegrityViewModel {
		private readonly IMessenger _messenger;
		private bool _isLoading;
		private string _windowTitle;

		public LogIntegrityViewModel(IMessenger messenger) {
			Text = new ObservableCollection<string>();

			IsLoading = true;
			_messenger = messenger;
			_messenger.Register<SetViewModelParametersMessage<IPsnLogViewModel>>(this,
					p => {
						WindowTitle = "Информация о целостности лога " + p.Parameter.Name;
						p.Parameter.PsnLog.GetStatisticAsync(OnGetStatisticsAsync);
					});
		}

		public string WindowTitle {
			get => _windowTitle;
			set { Set(() => WindowTitle, ref _windowTitle, value); }
		}

		public bool IsLoading {
			get => _isLoading;
			set { Set(() => IsLoading, ref _isLoading, value); }
		}

		public ObservableCollection<string> Text { get; }

		private void OnGetStatisticsAsync(Exception ex, IEnumerable<string> result) {
			IsLoading = false;
			Text.Clear();
			if (ex != null) {
				Text.Add("При подсчете статистики возникла исключительная ситуация: ");
				Text.Add(ex.Message);
				Text.Add(ex.StackTrace);
				return;
			}

			foreach (var str in result)
				Text.Add(str);
		}
	}
}
