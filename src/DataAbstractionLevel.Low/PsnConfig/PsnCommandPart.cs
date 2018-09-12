using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnCommandPartInfo : IPsnProtocolCommandPartConfiguration {

		public string PartName { get; set; }
		public PsnProtocolCommandPartType PartType { get; set; }

		public IPsnProtocolParameterDefinedConfiguration[] DefParams { get; set; }
		public IPsnProtocolParameterConfigurationVariable[] VarParams { get; set; }

		public byte Length { get; set; }
		public int Offset { get; set; }

		public IIdentifier CommandId { get; set; }
		public IPsnProtocolParameterDefinedConfiguration Address { get; set; }
		public IPsnProtocolParameterDefinedConfiguration CommandCode { get; set; }
		public IPsnProtocolParameterConfigurationVariable CrcLow { get; set; }
		public IPsnProtocolParameterConfigurationVariable CrcHigh { get; set; }
		public IIdentifier Id { get; set; }
	}
}
