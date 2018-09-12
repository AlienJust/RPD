using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal abstract class PsnParamBase : IPsnProtocolParameterConfiguration
	{
		protected PsnParamBase(string id, string name, bool isBitSignal) {
			Id = new IdentifierStringToLowerBased(id);
			Name = name;
			IsBitSignal = isBitSignal;
		}

		public IIdentifier Id { get; }

		public string Name { get; }

		public bool IsBitSignal { get; }

		public abstract double GetValue(byte[] cmdPartContext, int startByte);
	}
}