using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataFaultReason
{
	/// <summary>
	/// Хранилище данных для причин аварий дампов ПСН на основе XML
	/// </summary>
	public class PsnDataFaultReasonStorageXDocument : IStorage<IPsnDataFaultReason> {
		private const string RootXmlElementName = "PsnLogFaultReasons";
		private const string FaultReasonXmlElementName = "PsnLogFaultReason";

		private const string IdAttributeName = "Id";
		private const string FaultReasonAttributeName = "FaultReason";

		private readonly IStreamedData _streamReadableObject;
		private readonly IStreamedDataWritable _streamWritableObject;
		private IEnumerable<IPsnDataFaultReason> _storedItems;

		/// <summary>
		/// Создает новый экземпляр хранилища
		/// </summary>
		/// <param name="streamReadableObj">Объект, читающий данные из потока (для чтения данных из хранилища)</param>
		/// <param name="streamWritableObj">Объект, записывающий данные в поток (для записи данных в хранилище)</param>
		public PsnDataFaultReasonStorageXDocument(IStreamedData streamReadableObj, IStreamedDataWritable streamWritableObj) {
			_streamReadableObject = streamReadableObj;
			_streamWritableObject = streamWritableObj;
		}


		private XDocument ReadXDocFromStream()
		{
			XDocument doc;
			try
			{
				var stream = _streamReadableObject.GetStreamForReading();
				using (stream)
				{
					doc = XDocument.Load(stream);
				}
			}
			catch {
				// Бывает так, что файл не удается прочитать документ из потока (например, если файл был только что создан)
				doc = new XDocument(new XElement(RootXmlElementName));
			}
			return doc;
		}

		private void WriteOutXml(XDocument doc)
		{
			var stream = _streamWritableObject.GetStreamForWriting();
			using (stream)
			{
				doc.Save(stream);
			}
		}


		/// <summary>
		/// No cache, loads directly from XML, do not use without really needs
		/// Не кэширует данные, подгружает напрямую их XML, лучше не использовать без реальной необходимости (лучше работать через кэширующее хранилище)
		/// </summary>
		public IEnumerable<IPsnDataFaultReason> Configurations {
			get {
				var doc = ReadXDocFromStream();
				return doc.Element(RootXmlElementName).
					Elements(FaultReasonXmlElementName).
					Select(
						e => new PsnDataFaultReasonSimple(
							new IdentifierStringToLowerBased(e.Attribute(IdAttributeName).Value),
							e.Attribute(FaultReasonAttributeName).Value));
			}
		}

		public void Add(IPsnDataFaultReason item) {
			var doc = ReadXDocFromStream();
			doc.Element(RootXmlElementName).
				Add(
				new XElement(
					FaultReasonXmlElementName,
					new XAttribute(IdAttributeName, item.Id.IdentyString),
					new XAttribute(FaultReasonAttributeName, item.FaultReason)));
			WriteOutXml(doc);
		}

		public void Remove(IPsnDataFaultReason item) {
			var doc = ReadXDocFromStream();
			var faultReasonElements = doc.Element(RootXmlElementName).
				Elements(FaultReasonXmlElementName).
				Where(e => e.Attribute(IdAttributeName).Value == item.Id.IdentyString);
			foreach (var faultReasonElement in faultReasonElements) {
				faultReasonElement.Remove();
			}
			WriteOutXml(doc);
		}

		public void Update(IIdentifier id, IPsnDataFaultReason item) {
			// TODO: check id and item for null
			
			var doc = ReadXDocFromStream();
			var faultReasonElements = doc.Element(RootXmlElementName).
				Elements(FaultReasonXmlElementName).
				Where(e => e.Attribute(IdAttributeName).Value == id.IdentyString);
			foreach (var faultReasonElement in faultReasonElements) {
				faultReasonElement.Attribute(FaultReasonAttributeName).Value = item.FaultReason;
			}
			WriteOutXml(doc);
		}

		public IEnumerable<IPsnDataFaultReason> StoredItems {
			get { return _storedItems; }
		}
	}
}
