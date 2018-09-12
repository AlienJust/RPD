using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.InternalKitchen.Extensions;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage
{
	/// <summary>
	/// Хранилище информации о ПСН логах в формате XML
	/// </summary>
	public class PsnDataInformationStorageXDocument : IPsnDataInformationStorage
	{
		private readonly IStreamedData _streamReadableObject;
		private readonly IStreamedDataWritable _streamWritableObject;

		/// <summary>
		/// Создает новый экземпляр хранилища
		/// </summary>
		/// <param name="streamReadableObj">Объект для чтения данных из хранилища</param>
		/// <param name="streamWritableObj">Объект для записи данных в хранилище</param>
		public PsnDataInformationStorageXDocument(IStreamedData streamReadableObj, IStreamedDataWritable streamWritableObj)
		{
			_streamReadableObject = streamReadableObj;
			_streamWritableObject = streamWritableObj;
		}


		private XDocument ReadXDocFromStream()
		{
			try
			{
				using (var stream = _streamReadableObject.GetStreamForReading())
				{
					return XDocument.Load(stream);
				}
			}
			catch {
				// Бывает так, что файл не удается прочитать документ из потока (например, если файл был только что создан)
				return new XDocument(new XElement("PsnDataInformations"));
			}
		}

		private void WriteOutXml(XDocument doc)
		{
			using (var stream = _streamWritableObject.GetStreamForWriting())
			{
				doc.Save(stream);
			}
		}


		/// <summary>
		/// Перечисление даных хранилища (не кэшируемое)
		/// </summary>
		public IEnumerable<IPsnDataInformation> PsnDataInformations
		{
			get {
				//Console.WriteLine("Warning, enumerating PsnDataInformations");
				var doc = ReadXDocFromStream();
				return doc.Element("PsnDataInformations").
					Elements("PsnDataInformation").
					Select(
						e => {
							var logId = e.Attribute("Id").Value;
							var beginTime = e.Attribute("BeginTime").Value.FromFileMsString();
							var endTime = e.Attribute("EndTime").Value.FromFileMsString();
							var saveTime = e.Attribute("SaveTime").Value.FromFileMsString();
							var logType = e.Attribute("LogType").Value.ToPsnLogType();

							var deviceInformationId = e.Attribute("DeviceInformationId").Value;
							return new PsnDataInformationSimple(logId, beginTime, endTime, saveTime, logType, false, deviceInformationId);
						});
			}
		}

		/// <summary>
		/// Добавляет новые данные в хранилище
		/// </summary>
		/// <param name="psnLogInformationId">Идентификатор записи данных</param>
		/// <param name="beginTime">Время начала лога ПСН</param>
		/// <param name="endTime">Время окончания лога ПСН</param>
		/// <param name="saveTime">Время сохранения лога ПСН</param>
		/// <param name="psnDataType">Тип лога ПСН</param>
		/// <param name="isLastDeviceLog">Флаг, является ли лог последним на устройстве</param>
		/// <param name="deviceInformationId">Информация об устройстве</param>
		public void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId)
		{
			var doc = ReadXDocFromStream();
			doc.Element("PsnDataInformations").
				Add(
				new XElement(
					"PsnDataInformation",
					new XAttribute("Id", psnLogInformationId.IdentyString),
				             new XAttribute("SaveTime", saveTime.ToFileMsString()),
								 new XAttribute("LogType", psnDataType.ToSimpleString()),
				             new XAttribute("BeginTime", beginTime.ToFileMsString()),
				             new XAttribute("EndTime", endTime.ToFileMsString()),
				             new XAttribute("DeviceInformationId", deviceInformationId.IdentyString)));
			WriteOutXml(doc);
		}

		/// <summary>
		/// Удаляет данные ПСН из хранилища
		/// </summary>
		/// <param name="psnDataInformationId">Идентификатор записи в хранилище</param>
		public void Remove(IIdentifier psnDataInformationId)
		{
			var doc = ReadXDocFromStream();
			var customConfigElements = doc.Element("PsnDataInformations").Elements("PsnDataInformation").Where(e => e.Attribute("Id").Value == psnDataInformationId.IdentyString);
			foreach (var customConfigElement in customConfigElements) {
				customConfigElement.Remove();
			}
			WriteOutXml(doc);
		}
	}
}
