using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.Shared {
	internal sealed class DeviceInformationSimple : IDeviceInformationWritable {
		string _name;
		string _description;
		readonly IIdentifier _id;

		public DeviceInformationSimple(string name, string description, IIdentifier id)
		{
			_name = name;
			_description = description;
			_id = new IdentifierStringToLowerBased(id.IdentyString);
		}

		public string Name {
			get { return _name; }
		}

		public string Description {
			get { return _description; }
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public void SetName(string name) {
			_name = name;
		}

		public void SetDescription(string description) {
			_description = description;
		}

		public override string ToString() {
			return _name + "/" + _description + "[" + _id + "]";
		}
	}
}