using System.Xml.Linq;
using RPD.DAL;
using RPD.DalRelease.ConfigurationSaver.Contracts;

namespace RPD.DalRelease.ConfigurationSaver.BasicRelease
{
	class DeviceConfigurationSaverToXml : IDeviceConfigurationSaver {
		private readonly string _filename;

		public DeviceConfigurationSaverToXml(string filename) {
			_filename = filename;
		}

		public void SaveConfiguration(IDeviceConfiguration configuration) {
			var xdoc = new XDocument();
			xdoc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
		}
	}
}
