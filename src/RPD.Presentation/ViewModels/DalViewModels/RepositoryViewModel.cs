using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using Dnv.Utils.Collections;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления хранилища.
	/// </summary>
	internal sealed class RepositoryViewModel : ViewModelBase, IRepositoryViewModel {
		ObservableCollectionsConnector<ILocomotive, ILocomotiveViewModel> _locomotivesLinker;
		bool _captureNewFaults = false;
		string _header;
		private ObservableCollection<ILocomotiveViewModel> _locomotives = new ObservableCollection<ILocomotiveViewModel>();

		public RepositoryViewModel(IRepository repository) {
			Repository = repository ?? throw new ArgumentNullException(nameof(repository), " must be not null");
			FillLocomotives(repository);
			Messenger.Default.Register<CaptureNewFaultsMessage>(this, m => CaptureNewFaults = m.Start);
		}


		#region Private Methods

		private void FillLocomotives(IRepository repository) {
			foreach (var locomotive in repository.Locomotives)
				Locomotives.Add(new LocomotiveViewModel(locomotive));

			// Связываем модель представления локомотивов с локомотивами из модели данных.
			_locomotivesLinker = new ObservableCollectionsConnector<ILocomotive, ILocomotiveViewModel>
					(repository.Locomotives, Locomotives, OnNewLocomotiveAdded,
					locomotive => Locomotives.FirstOrDefault(e => e.Locomotive == locomotive));
		}


		/// <summary>
		/// Вызывается автоматически при добавлении нового локомотива в список локомотивов в модели данных (IRepository.Locomotives).
		/// </summary>
		/// <param name="locomotive">Локомотив.</param>
		/// <returns>Модель представления локомотива.</returns>
		private LocomotiveViewModel OnNewLocomotiveAdded(ILocomotive locomotive) {
			var locomotiveViewModel = new LocomotiveViewModel(locomotive) { CaptureNewFaults = CaptureNewFaults };
			if (CaptureNewFaults)
				locomotiveViewModel.AllFaultsNew = true;
			return locomotiveViewModel;
		}

		#endregion

		#region Presentation Members

		public IRepository Repository { get; set; }

		/// <summary>
		/// Если true, то у всех аварий, которые добавляются в 
		/// списки аварий на локомотивах будет выставлен флаг IsNew.
		/// </summary>
		public bool CaptureNewFaults {
			get => _captureNewFaults;
			set {
				_captureNewFaults = value;

				foreach (var locomotive in Locomotives)
					locomotive.CaptureNewFaults = value;
			}
		}

		public string Header {
			get => _header;
			set { Set(() => Header, ref _header, value); }
		}

		/// <summary>
		/// Список локомотивов.
		/// </summary>
		public ObservableCollection<ILocomotiveViewModel> Locomotives {
			get => _locomotives;
			set { Set(() => Locomotives, ref _locomotives, value); }
		}

		#endregion
	}
}
