using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPD.DAL;
using RPD.DalRelease.RPD;

namespace RPD {
	/// <summary>
	/// Страница архива аварии РПД
	/// </summary>
	internal class FaultArchivePage {
		/// <summary>
		/// Представление страницы ввиде набора байт
		/// </summary>
		public byte[] RawView { get; private set; }

		/// <summary>
		/// Время создания страницы
		/// </summary>
		public DateTime TimeCreated { get; set; }

		/// <summary>
		/// Индекс страницы в дампе AVRXX.bin
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Авария, к которой относится данная страница
		/// </summary>
		public FaultLog OwnerFault { get; set; }

		/// <summary>
		/// Номер измерителя, к которому принадлежит данная страница
		/// </summary>
		public int MeterNumber { get; set; }

		/// <summary>
		/// Список строк страницы
		/// </summary>
		public List<VariableLengthPageLine> Lines { get; set; }

		/// <summary>
		/// Инициализирует новый экземпляр класса
		/// </summary>
		/// <param name="rawView">Массив байт, прочитанных из файла вида AVRXX.bin</param>
		/// <param name="ownerFault">Авария, к которой относится данная страница</param>
		public FaultArchivePage(byte[] rawView, FaultLog ownerFault) {
			RawView = rawView;
			OwnerFault = ownerFault;

			//первые 6 байт - дата и время создания страницы
			TimeCreated = new DateTime(rawView[3], rawView[2], rawView[1], rawView[4], rawView[5], rawView[6]);

			//индекс (порядковый номер страницы (7-ой и 8-ой байты)
			Index = rawView[7]*256 + rawView[8];

			MeterNumber = rawView[0];

			Lines = new List<VariableLengthPageLine>();
			for (int i = 11; i < 11 + 21*97; i += 21) {
				var lineRaw = new byte[21];
				for (int j = 0; j < 21; ++j) {
					lineRaw[j] = rawView[i + j];
				}
				var pl = new VariableLengthPageLine(lineRaw);
				Lines.Add(pl);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------
	internal class VariableLengthPageLine {
		private readonly List<int> _lineValues = new List<int>();
		public const int LineHeaderBytesCount = 6;
		public byte[] LineRaw { get; private set; }

		/// <summary>
		/// Номер измерителя, к которому принадлежит данная строка
		/// </summary>
		public int MeterNumber { get; private set; }

		/// <summary>
		/// Номер канала измерителя, к которому принадлежит данная строка
		/// </summary>
		public int ChannelNumber { get; private set; }

		/// <summary>
		/// Номер аварии, к которой относится данная строка
		/// </summary>
		public int FaultNumber { get; private set; }

		/// <summary>
		/// Номер данной строки
		/// </summary>
		public int LineNumber { get; private set; }

		public VariableLengthPageLine(byte[] lineRaw) {
			LineRaw = lineRaw;

			MeterNumber = lineRaw[0];
			ChannelNumber = lineRaw[1];
			FaultNumber = lineRaw[2];
			LineNumber = lineRaw[3]*256*256 + lineRaw[4]*256 + lineRaw[5];

			//Значения строки, из каждых трех байт извлекается 2 значения
			_lineValues = new List<int>();
			for (int i = 6; i < lineRaw.Length; i += 3) { 
				//int x = ((lineRaw[1 + i] & 0x0F));
				int v1 = (((lineRaw[1 + i] & 0x0F)) << 8) + lineRaw[0 + i];
				int v2 = ((lineRaw[2 + i]) << 4) + ((lineRaw[1 + i] & 0xF0) >> 4);

				_lineValues.Add(v1);
				_lineValues.Add(v2);
			}
		}

		public ReadOnlyCollection<int> Values {
			get { return Array.AsReadOnly(_lineValues.ToArray()); }
		}
	}
}
