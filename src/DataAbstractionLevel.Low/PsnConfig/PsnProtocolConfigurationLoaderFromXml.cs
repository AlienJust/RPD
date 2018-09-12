using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	public class PsnProtocolConfigurationLoaderFromXml : IPsnProtocolConfigurationLoader {
		private readonly string _xmlFileName;

		public PsnProtocolConfigurationLoaderFromXml(string xmlFileName) {
			_xmlFileName = xmlFileName;
		}

		public IPsnProtocolConfiguration LoadConfiguration()
		{
			var protocolLoader = new PsnProtocolXmlSerializer(_xmlFileName);
			return protocolLoader.LoadProtocol();
		}
	}
}