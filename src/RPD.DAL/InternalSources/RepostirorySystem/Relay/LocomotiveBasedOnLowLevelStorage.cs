using System;
using System.Collections.ObjectModel;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace RPD.DAL.RepostirorySystem.Relay {
	internal class Locomotive : ILocomotive {
		public string Name { get; set; }
		private readonly IWorker<Action> _backWorker;
		private readonly IDeviceInformationStorage _backSotrage;

		public Locomotive(string name, IWorker<Action> backWorker, IDeviceInformationStorage backSotrage) {
			Name = name;
			_backWorker = backWorker;
			_backSotrage = backSotrage;

			Sections = new ObservableCollection<ISection>();
		}

		public void SetName(string name) {
			Name = name;
			foreach (var section in Sections) {
				var sectionName = section.Name;
				var devId = section.DeviceInformationId;
				Exception exc = null;
				_backWorker.AddToQueueAndWaitExecution(() => {
					try {
						_backSotrage.Update(new IdentifierStringToLowerBased(devId.UnicString), name, sectionName);
					}
					catch (Exception ex) {
						exc = ex;
					}
				});
				if (exc != null) throw exc;
			}
		}

		public ObservableCollection<ISection> Sections { get; set; }
	}
}
