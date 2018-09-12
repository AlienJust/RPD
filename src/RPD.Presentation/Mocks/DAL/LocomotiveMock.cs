using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	internal class LocomotiveMock : ILocomotive {
		public LocomotiveMock() {
			Sections = new ObservableCollection<ISection>();
			Name = "Локомотив 65ПН74";
		}

		#region Implementation of ILocomotive

		public void SetName(string name) {
			throw new System.NotImplementedException();
		}

		public ObservableCollection<ISection> Sections { get; }
		public string Name { get; set; }

		#endregion
	}
}
