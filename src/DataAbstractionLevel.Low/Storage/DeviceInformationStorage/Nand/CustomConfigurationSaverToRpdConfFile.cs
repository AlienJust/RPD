using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public sealed class CustomConfigurationSaverToRpdConfFile : ICustomConfigurationSaver {
		private readonly string _fileName;
		private const int FileSize = 16384;

		public CustomConfigurationSaverToRpdConfFile(string binaryFileName) {
			_fileName = binaryFileName;
		}

		public void SaveConfiguration(ICustomConfiguration configuration) {
			// 1. Build 2 XML files (with attributes and with rpd meters)
			// 1.1 Build attributes
			var atrDoc = new XDocument(
				new XDeclaration("1.0", "utf-8", "no"),
				new XElement(
					"Attributes",
					new XElement("Locomotive", new XAttribute("value", configuration.LocomotiveName)),
					new XElement("Section", new XAttribute("value", configuration.SectionNumber))));
		
			// 1.2 Build RPD config
			var rpdDoc = new XDocument(
				new XDeclaration("1.0", "utf-8", "no"),
				new XElement("Configuration",
				             new XElement("Meters",
				                          configuration.RpdMetersCustomInfos.Select(
				                          	ci => new XElement("Meter",
				                          	                   new XAttribute("address", ci.Address),
				                          	                   new XAttribute("name", ci.Name),
				                          	                   new XAttribute("type", ci.MeterType.ToXmlString()))))));


			byte[] gzippedAtrDocument;
			using (var atrMs = new MemoryStream()) {
				using (var atrGz = new GZipStream(atrMs, CompressionMode.Compress)) {
					atrDoc.Save(atrGz);
				}
				gzippedAtrDocument = atrMs.ToArray();
			}

			byte[] gzippedRpdDocument;
			using (var rpdMs = new MemoryStream()) {
				using (var rpdGz = new GZipStream(rpdMs, CompressionMode.Compress)) {
					rpdDoc.Save(rpdGz);
				}
				gzippedRpdDocument = rpdMs.ToArray();
			}
			

			using (var sw = new BinaryWriter(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write))) {
				// Clearing or creating file
				for  (int i = 0; i < FileSize; ++i) {
					sw.Write((byte)0);
				}
				sw.Seek(0, SeekOrigin.Begin);

				// 2. Save XML To GZIP (len1-xml1-len2-xml2)
				sw.Write((byte)configuration.PsnVersion);
				sw.Write(gzippedAtrDocument.Length);
				sw.Write(gzippedAtrDocument);

				sw.Write(gzippedRpdDocument.Length);
				sw.Write(gzippedRpdDocument);
			}
			// TODO: using and disposing streams
		}
	}
}