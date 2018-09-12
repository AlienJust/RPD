using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace RPD.DAL.RepostirorySystem.Relay {
	internal class Section : ISection {
		private readonly IUid _deviceInformationId;
		private readonly IWorker<Action> _backWorker;
		private readonly IDeviceInformationStorage _backStorage;

		public ObservableCollection<IFaultLog> Faults { get; set; }
		public ObservableCollection<IPsnLog> Psns { get; set; }

		private string _name;
		

		public Section(IUid deviceInformationId, string name, IWorker<Action> backWorker, IDeviceInformationStorage backStorage)
		{
			_deviceInformationId = deviceInformationId;
			_name = name;

			_backWorker = backWorker;
			_backStorage = backStorage;

			Faults = new ObservableCollection<IFaultLog>();
			Psns = new ObservableCollection<IPsnLog>();
		}

		public void SetName(string name) {
			_name = name;
			Exception exc = null;
			_backWorker.AddToQueueAndWaitExecution(
				() => {
					try {
						// Сперва получаем данные из хранилища, т.к. не знаем названия локомотива
						var devInfo = _backStorage.DeviceInformations.First(di => di.Id.ToString() == _deviceInformationId.ToString());
						_backStorage.Update(new IdentifierStringToLowerBased(_deviceInformationId.UnicString), devInfo.Name, name);
					}
					catch (Exception ex) {
						exc = ex;
					}
				});
			if (exc != null) throw exc;
		}


		public string Name { get { return _name; } }
		

		public IUid DeviceInformationId
		{
			get { return _deviceInformationId; }
		}
	}
}
