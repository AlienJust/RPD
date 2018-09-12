using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataCustomConfigStorage
{
	/// <summary>
	/// Хранилище данных для пользовательской конфигурации данных ПСН на основе XML
	/// </summary>
	public class PsnDataCustomConfigurationsStorageXDocument : IPsnDataCustomConfigurationsStorage
	{
		private readonly IStreamedData _streamReadableObject;
		private readonly IStreamedDataWritable _streamWritableObject;

		/// <summary>
		/// Создает новый экземпляр хранилища
		/// </summary>
		/// <param name="streamReadableObj">Объект, читающий данные из потока (для чтения данных из хранилища)</param>
		/// <param name="streamWritableObj">Объект, записывающий данные в поток (для записи данных в хранилище)</param>
		public PsnDataCustomConfigurationsStorageXDocument(IStreamedData streamReadableObj, IStreamedDataWritable streamWritableObj) {
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
				doc = new XDocument(new XElement("PsnLogCustomConfigurations"));
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
		public IEnumerable<IPsnDataCustomConfigration> Configurations {
			get {
				var doc = ReadXDocFromStream();
				return doc.Element("PsnLogCustomConfigurations").
					Elements("PsnLogCustomConfiguration").
					Select(
						e => new PsnDataCustomConfigurationSimple(
							new IdentifierStringToLowerBased(e.Attribute("Id").Value),
							new IdentifierStringToLowerBased(e.Attribute("PsnConfigurationId").Value),
							e.Attribute("CustomLogName").Value));
			}
		}

		/// <summary>
		/// Добавляет новые пользовательские данные в хранилище
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор данных</param>
		/// <param name="psnConfigruationId">Идентификатор конфигурации ПСН</param>
		/// <param name="customLogName">Пользоваетльское название данных ПСН</param>
		public void Add(IIdentifier psnDataCustomConfigurationId, IIdentifier psnConfigruationId, string customLogName)
		{
			var doc = ReadXDocFromStream();
			doc.Element("PsnLogCustomConfigurations").
				Add(
				new XElement(
					"PsnLogCustomConfiguration", 
					new XAttribute("Id", psnDataCustomConfigurationId.ToString()),
					new XAttribute("PsnConfigurationId", psnConfigruationId.IdentyString),
					new XAttribute("CustomLogName", customLogName)));
			WriteOutXml(doc);
		}

		/// <summary>
		/// Обновляет пользовательские данные в хранилище
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор записи, которую необходимо обновить</param>
		/// <param name="setPsnConfigruationId">Новое значение идентификатора конфигурации ПСН</param>
		/// <param name="setCustomLogName">Новое значение пользоваетльского названия данных ПСН</param>
		public void Update(IIdentifier psnDataCustomConfigurationId, IIdentifier setPsnConfigruationId, string setCustomLogName)
		{
			var doc = ReadXDocFromStream();
			var customConfigElements = doc.Element("PsnLogCustomConfigurations").
				Elements("PsnLogCustomConfiguration").
				Where(e => e.Attribute("Id").Value == psnDataCustomConfigurationId.IdentyString);
			foreach (var customConfigElement in customConfigElements) {
				customConfigElement.Attribute("PsnConfigurationId").SetValue(setPsnConfigruationId.IdentyString);
				customConfigElement.Attribute("CustomLogName").SetValue(setCustomLogName);
			}
			WriteOutXml(doc);
		}

		/// <summary>
		/// Удаляет данные их хранилища
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор записи для удаления</param>
		public void Remove(IIdentifier psnDataCustomConfigurationId) {
			var doc = ReadXDocFromStream();
			var customConfigElements = doc.Element("PsnLogCustomConfigurations").
				Elements("PsnLogCustomConfiguration").
				Where(e => e.Attribute("Id").Value == psnDataCustomConfigurationId.IdentyString);
			foreach (var customConfigElement in customConfigElements) {
				customConfigElement.Remove();
			}
			WriteOutXml(doc);
		}
	}
}
