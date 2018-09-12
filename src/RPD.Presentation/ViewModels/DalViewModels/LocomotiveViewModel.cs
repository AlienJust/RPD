using System.Collections.ObjectModel;
using System.Linq;
using Dnv.Utils.Collections;
using RPD.DAL;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления локомотива.
	/// </summary>
	class LocomotiveViewModel : ViewModelBase, ILocomotiveViewModel {
		ObservableCollectionsConnector<ISection, ISectionViewModel> _sectionsLinker;

		public ILocomotive Locomotive { get; set; }

		public bool IsTreeViewExpanded { get; set; }

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="locomotive">Локомотив.</param>
		public LocomotiveViewModel(ILocomotive locomotive) {
			Locomotive = locomotive;

			FillSections(locomotive);
		}

		private void FillSections(ILocomotive locomotive) {
			for (int i = 0; i < locomotive.Sections.Count; i++)
				Sections.Add(new SectionViewModel(locomotive.Sections[i], i + 1, this));

			// Связываем модель представления секций с секциями из модели данных.
			_sectionsLinker = new ObservableCollectionsConnector<ISection, ISectionViewModel>
					(locomotive.Sections, Sections, OnNewSectionAdded,
					section => Sections.FirstOrDefault(e => e.Section == section));
		}



		/// <summary>
		/// Вызывается при добавленнии новой секции в список секции в модели данных (ILocomotive.Sections).
		/// </summary>
		/// <param name="section">Новая секция.</param>
		/// <returns>Модель представления секции.</returns>
		SectionViewModel OnNewSectionAdded(ISection section) {
			var sectionViewModel =
				new SectionViewModel(section, Locomotive.Sections.Count, this) { CaptureNewFaults = CaptureNewFaults };

			if (CaptureNewFaults)
				sectionViewModel.AllFaultsNew = true;

			return sectionViewModel;
		}

		/// <summary>
		/// Если true, то у всех аварий на этом локомотиве будет высталвен флаг IsNew.
		/// </summary>
		public bool AllFaultsNew {
			set {
				foreach (var section in Sections) {
					foreach (var fault in section.Faults)
						fault.IsNew = value;
				}
			}
		}

		bool _captureNewFaults = false;

		/// <summary>
		/// Если true, то у всех аварий, которые добаляются в 
		/// списки аварий на секциях будет выставлен флаг IsNew.
		/// </summary>
		public bool CaptureNewFaults {
			get => _captureNewFaults;
			set {
				_captureNewFaults = value;
				foreach (var section in Sections)
					section.CaptureNewFaults = value;
			}
		}


		#region Presentation Members


		string _name = "";

		/// <summary>
		/// Название локомотива.
		/// </summary>
		public string Name {
			get {
				if (_name.Length == 0)
					return Locomotive.Name;
				return _name;
			}

			set { Set(() => Name, ref _name, value); }
		}


		private ObservableCollection<ISectionViewModel> _sections = new ObservableCollection<ISectionViewModel>();

		/// <summary>
		/// Список секций.
		/// </summary>
		public ObservableCollection<ISectionViewModel> Sections {
			get => _sections;
			set { Set(() => Sections, ref _sections, value); }
		}

		#endregion //Presentation Members
	}
}
