using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage {
	public sealed class DeviceInformationStorageXDocument : IDeviceInformationStorage
	{
		private readonly IStreamedData _streamReadableObject;
		private readonly IStreamedDataWritable _streamWritableObject;

		public DeviceInformationStorageXDocument(IStreamedData streamReadableObj, IStreamedDataWritable streamWritableObj)
		{
			_streamReadableObject = streamReadableObj;
			_streamWritableObject = streamWritableObj;
		}

		private XDocument ReadXDocFromStream()
		{
			XDocument doc;
			try
			{
				using (var stream = _streamReadableObject.GetStreamForReading())
				{
					doc = XDocument.Load(stream);
				}
			}
			catch//(Exception ex)
			{
				// Бывает так, что файл не удается прочитать документ из потока (например, если файл был только что создан)
				doc = new XDocument(new XElement("DeviceInformations"));
			}
			return doc;
		}

		private void WriteOutXml(XDocument doc)
		{
			using (var stream = _streamWritableObject.GetStreamForWriting())
			{
				doc.Save(stream);
			}
		}

		public IEnumerable<IDeviceInformation> DeviceInformations {
			get
			{
				var doc = ReadXDocFromStream();
				return 
					doc.Element("DeviceInformations").
					Elements("DeviceInformation").
					Select(i => new DeviceInformationSimple(i.Attribute("Name").Value, i.Attribute("Description").Value, new IdentifierStringToLowerBased(i.Attribute("Id").Value)));
			}
		}

		public void Add(IIdentifier id, string name, string description)
		{
			var doc = ReadXDocFromStream();
			doc.Element("DeviceInformations").
				Add(
					new XElement("DeviceInformation",
					             new XAttribute("Name", name),
					             new XAttribute("Description", description),
					             new XAttribute("Id", id.IdentyString)));
			WriteOutXml(doc);
		}

		public void Remove(IIdentifier id)
		{
			var doc = ReadXDocFromStream();
			var customConfigElements = doc.Element("DeviceInformations").Elements("DeviceInformation").Where(e => e.Attribute("Id").Value == id.IdentyString);
			foreach (var customConfigElement in customConfigElements)
			{
				customConfigElement.Remove();
			}
			WriteOutXml(doc);
		}

		public void Update(IIdentifier id, string setName, string setDescription)
		{
			var doc = ReadXDocFromStream();
			var devInfoElementsToUpdate = doc.Element("DeviceInformations").Elements("DeviceInformation").Where(i=>i.Attribute("Id").Value == id.IdentyString);
			foreach (var xelement in devInfoElementsToUpdate) {
				xelement.SetAttributeValue("Name", setName);
				xelement.SetAttributeValue("Description", setDescription);
			}
			WriteOutXml(doc);
		}
	}
}