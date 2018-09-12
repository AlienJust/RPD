using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	class SectionMock : ISection {
		public SectionMock(ILocomotive owner) {
			OwnerLocomotive = owner;
			Faults = new ObservableCollection<IFaultLog>();
			Psns = new ObservableCollection<IPsnLog>();
		}

		#region Implementation of ISection

		public void SetName(string name) {
			throw new System.NotImplementedException();
		}

		public IUid DeviceInformationId => throw new System.NotImplementedException();

		public ObservableCollection<IFaultLog> Faults { get; }
		public ObservableCollection<IPsnLog> Psns { get; }
		public ILocomotive OwnerLocomotive { get; }
		public string Name { get; set; }

		#endregion
	}
}
