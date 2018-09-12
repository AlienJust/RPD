using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public class CustomConfigurationBuilderFromRpdConfFile : ICustomConfigurationBuilder {
		private readonly string _fileName;
		public CustomConfigurationBuilderFromRpdConfFile(string binaryFileName) {
			_fileName = binaryFileName;
		}

		public ICustomConfiguration BuildConfiguration() {
			if (!File.Exists(_fileName))
				throw new Exception("Cannot find file " + _fileName);
			

			using (var fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read)) {
				int psnProtocol = fs.ReadByte();
				var attributesXml = GetXmlDocFromRpdconf(fs);
				var locomotiveName = attributesXml.Element("Attributes").Element("Locomotive").Attribute("value").Value;
				var sectionNumber = int.Parse(attributesXml.Element("Attributes").Element("Section").Attribute("value").Value);
				
				// TODO: parse meters info

				var hardwareConfigXml = GetXmlDocFromRpdconf(fs);
				var meterNodes = hardwareConfigXml.Element("Configuration").Element("Meters").Elements("Meter");
				var meterInfos = new List<IRpdMeterCustomInfo>();
				foreach (var meterNode in meterNodes) {
					var meterInfo = new RpdMeterCustomInfoSimple { Address = int.Parse(meterNode.Attribute("address").Value), Name = meterNode.Attribute("name").Value };
					var type = meterNode.Attribute("type").Value;
					switch (type) {
						case "URAN":
							meterInfo.MeterType = RpdProtocolMeterType.Uran;
							break;
						case "IRVI":
							meterInfo.MeterType = RpdProtocolMeterType.Irvi;
							break;
						case "RADAR":
							meterInfo.MeterType = RpdProtocolMeterType.Radar;
							break;
						default:
							meterInfo.MeterType = RpdProtocolMeterType.Undefined;
							break;
					}
					meterInfo.ChannelInfos = new List<IRpdChannelCustomInformation>();
					var channelNodes = meterNode.Elements("Channel");
					foreach (var channelNode in channelNodes) {
						var channelInfo = new RpdChannelCustomInformationSimple { Number = int.Parse(channelNode.Attribute("number").Value), Name = channelNode.Attribute("name").Value, IsEnabled = bool.Parse(channelNode.Attribute("isEnabled").Value), IsService = bool.Parse(channelNode.Attribute("isService").Value) };
						meterInfo.ChannelInfos.Add(channelInfo);
					}
					meterInfos.Add(meterInfo);
				}

				fs.Close();
				return new CustomConfigurationSimple { LocomotiveName = locomotiveName, SectionNumber = sectionNumber, PsnVersion = psnProtocol, RpdMetersCustomInfos = meterInfos };
			}
		}

		private XDocument GetXmlDocFromRpdconf(FileStream rpdconfFileStream) {
			var br = new BinaryReader(rpdconfFileStream);

			int len = br.ReadInt32();
			if (len > 0) // может так случиться, что длина XML документа = 0 (если при записи XmlDocument был null)
			{
				var gzippedXml = new byte[len];
				br.Read(gzippedXml, 0, len);
				var gzippedXmlStream = new MemoryStream(gzippedXml);
				var gzStream = new GZipStream(gzippedXmlStream, CompressionMode.Decompress);
				var decompressedStream = new MemoryStream();
				gzStream.CopyTo(decompressedStream);
				gzStream.Close();
				gzippedXmlStream.Close();
				decompressedStream.Seek(0, SeekOrigin.Begin);

				return XDocument.Load(decompressedStream);
			}
			throw new Exception("Cannot create XDocument, size is zerro");
		}
	}
}