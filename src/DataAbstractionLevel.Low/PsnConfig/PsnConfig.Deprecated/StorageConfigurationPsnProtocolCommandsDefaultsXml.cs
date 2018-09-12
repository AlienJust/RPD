using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class StorageConfigurationPsnProtocolCommandsDefaultsXml : IStorageConfigurationPsnProtocolCommands
	{
		public const string RootNodeName = "PsnConfiguration";
		private readonly string _pathToDefatulsDirecotry;
		public StorageConfigurationPsnProtocolCommandsDefaultsXml(string pathToDefatulsDirecotry)
		{
			_pathToDefatulsDirecotry = pathToDefatulsDirecotry;
		}

		public IEnumerable<IConfigurationPsnProtocolCommand> ConfigurationPsnProtocolCommands
		{
			get
			{
				var result = new List<IConfigurationPsnProtocolCommand>();
				var defaultsDirInfo = new DirectoryInfo(_pathToDefatulsDirecotry);
				var psnConfigFiles = defaultsDirInfo.GetFiles("psn.*.xml");
				foreach (var psnConfigFile in psnConfigFiles)
				{
					try
					{
						var xdoc = XDocument.Load(psnConfigFile.FullName);
						var rootNode = xdoc.Root;
						if (rootNode == null) throw new Exception("Не удалось найти начальный узел XML");
						if (rootNode.Name != RootNodeName) throw new Exception("Название начального узла XML должно быть " + RootNodeName);
						var configId = rootNode.Attribute("Id").Value;

						var commandsElement = rootNode.Element("Commands");
						if (commandsElement == null) throw new Exception("Не удалось найти элемент XML <Commands/>");
						var commandElements = commandsElement.Elements("CmdMask");
						{

							foreach (var commandElement in commandElements) {
								try {
									var commandName = commandElement.Attribute("Name").Value;
									var commandId = commandElement.Attribute("Id").Value;
									result.Add(new ConfigurationPsnProtocolCommandSimple(new IdString(commandId), commandName, new IdString(configId)));
								}
								catch {
									continue;
								}
							}
						}
					}
					catch
					{
						continue;
					}
				}
				return result;
			}
		}
	}

	class StorageConfigurationPsnProtocolCommandPartsDefaultsXml : IStorageConfigurationPsnProtocolCommandParts
	{
		public const string RootNodeName = "PsnConfiguration";
		private readonly string _pathToDefatulsDirecotry;
		public StorageConfigurationPsnProtocolCommandPartsDefaultsXml(string pathToDefatulsDirecotry)
		{
			_pathToDefatulsDirecotry = pathToDefatulsDirecotry;
		}

		public IEnumerable<IConfigurationPsnProtocolCommandPart> ConfigurationPsnProtocolCommandParts
		{
			get
			{
				var result = new List<IConfigurationPsnProtocolCommandPart>();
				var defaultsDirInfo = new DirectoryInfo(_pathToDefatulsDirecotry);
				var psnConfigFiles = defaultsDirInfo.GetFiles("psn.*.xml");
				foreach (var psnConfigFile in psnConfigFiles)
				{
					try
					{
						var xdoc = XDocument.Load(psnConfigFile.FullName);
						var rootNode = xdoc.Root;
						if (rootNode == null) throw new Exception("Не удалось найти начальный узел XML");
						if (rootNode.Name != RootNodeName) throw new Exception("Название начального узла XML должно быть " + RootNodeName);
						//var configId = rootNode.Attribute("Id").Value;

						var commandsElement = rootNode.Element("Commands");
						if (commandsElement == null) throw new Exception("Не удалось найти элемент XML <Commands/>");
						var commandElements = commandsElement.Elements("CmdMask");
						{

							foreach (var commandElement in commandElements)
							{
								try
								{
									// TODO: request node
									// TODO: reply node
								}
								catch
								{
									continue;
								}
							}
						}
					}
					catch
					{
						continue;
					}
				}
				return result;
			}
		}
	}
}