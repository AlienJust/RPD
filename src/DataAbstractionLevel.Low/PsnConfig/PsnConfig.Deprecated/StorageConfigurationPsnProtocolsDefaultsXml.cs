using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class StorageConfigurationPsnProtocolsDefaultsXml : IStorageConfigurationPsnProtocols {
		public const string RootNodeName = "PsnConfiguration";
		private readonly string _pathToDefatulsDirecotry;
		public StorageConfigurationPsnProtocolsDefaultsXml(string pathToDefatulsDirecotry) {
			_pathToDefatulsDirecotry = pathToDefatulsDirecotry;
		}

		public IEnumerable<IConfigurationPsnProtocol> ConfigurationPsnProtocols {
			get {
				var defaultsDirInfo = new DirectoryInfo(_pathToDefatulsDirecotry);
				var psnConfigFiles = defaultsDirInfo.GetFiles("psn.*.xml");
				foreach (var psnConfigFile in psnConfigFiles) {
					string configId;
					string configName;
					string configVersion;
					string configDescription;
					int configRpdId;
					try {
						var xdoc = XDocument.Load(psnConfigFile.FullName);
						var rootNode = xdoc.Root;
						if (rootNode == null) throw new Exception("Не удалось найти начальный узел XML");
						if (rootNode.Name != RootNodeName) throw new Exception("Название начального узла XML должно быть " + RootNodeName);

						configId = rootNode.Attribute("Id").Value;
						configName = rootNode.Attribute("Name").Value;
						configVersion = rootNode.Attribute("Version").Value;
						configDescription = rootNode.Attribute("Description").Value;
						configRpdId = int.Parse(rootNode.Attribute("RpdId").Value);
					}
					catch {
						continue;
					}
					yield return new ConfigurationPsnProtocolSimple(new IdString(configId), configName, configVersion, configDescription, configRpdId);
				}
			}
		}
	}
}