using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig
{
	internal class PsnCommandInfo : IPsnProtocolCommandConfiguration {
		public string Name { get; set; }
		public IIdentifier Id { get; set; }
	}
}
