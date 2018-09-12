using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData
{
	sealed class PsnBinPageExtractor : IPsnPageExtractor {
		
		/// <summary>
		/// Длинна страницы
		/// </summary>
		public int PsnPageSize => 2048;

		/// <summary>
		/// Длина заголовка страницы
		/// </summary>
		public int PsnPageHeaderLength => 10;


		/// <summary>
		/// Старт байт страницы
		/// </summary>
		public byte StartByteValue => 0x80;


		/// <summary>
		/// Смещение лет добавлять даты лога (то есть в бинарнике хранится не 2010 год, например, а 0010 год)
		/// </summary>
		private const int PageYearsOffset = 2000;

		public PsnPageHeader GetHeaderFromRealDevice(byte[] headerRaw)
		{
			// TODO: get page real number
			if (headerRaw.Length < PsnPageHeaderLength) throw new Exception("Невозможно создать заголовок страницы ПСН. Недостаточное количество байт");
			//if (!StartByteIsOk(headerRaw[0])) throw new Exception("Невозможно создать заголовок страницы ПСН. Страница имеет неверный стартбайт");

			DateTime? time = GetTimeFromRawHeader(headerRaw);
			var pageInfo = GetPageInfoFromHeaderBytes(headerRaw);
			if (pageInfo == PsnPageInfo.NormalPage) {
				return new PsnPageHeader(time, TimeSpan.FromMilliseconds(((headerRaw[8] << 8) | headerRaw[9]) * 100), pageInfo, 0); // 1 дискрет = 0.1 сек. согласно ТЗ
				//return new PsnPageHeader(time, TimeSpan.FromMilliseconds(((headerRaw[8] << 8) | headerRaw[9])), pageInfo); // 1 дискрет = 0.1 сек. согласно ТЗ
			}
			else {
				return new PsnPageHeader(time, TimeSpan.FromMilliseconds(0), pageInfo, 0);
			}
		}

		public bool StartByteIsOk(byte startByte)
		{
			return startByte == StartByteValue;
		}

		public PsnPageInfo GetPageInfoFromHeaderBytes(byte[] headerRaw) {
			if (headerRaw[0] != StartByteValue) return PsnPageInfo.BadPage;
			return PsnPageInfo.NormalPage;
		}

		private DateTime? GetTimeFromRawHeader(IList<byte> headerRaw) {
			DateTime? result;
			if (headerRaw[0] != StartByteValue) result = null;
			else if (headerRaw[1] == 0x01 &&
						headerRaw[2] == 0x01 &&
						headerRaw[3] == 0x01 &&
						headerRaw[4] == 0x01 &&
						headerRaw[5] == 0x01 &&
						headerRaw[6] == 0x01)
				result = null;
			else if (headerRaw[1] == 0xFF &&
						headerRaw[2] == 0xFF &&
						headerRaw[3] == 0xFF &&
						headerRaw[4] == 0xFF &&
						headerRaw[5] == 0xFF &&
						headerRaw[6] == 0xFF)
				result = null;

			else if (headerRaw[1] == 0x00 &&
						headerRaw[2] == 0x00 &&
						headerRaw[3] == 0x00 &&
						headerRaw[4] == 0x00 &&
						headerRaw[5] == 0x00 &&
						headerRaw[6] == 0x00)
				result = null;
			else {
				try {
					byte day = headerRaw[1];
					byte month = headerRaw[2];
					byte year = headerRaw[3];

					byte hour = headerRaw[4];
					byte minute = headerRaw[5];
					byte second = headerRaw[6];
					result = new DateTime(PageYearsOffset + year, month, day, hour, minute, second);
				}
				catch {
					result = null;
				}
			}
			return result;
		}

		public PsnPageHeader GetHeaderFromRealDevice(byte[] data, int offset) {
			throw new NotImplementedException();
		}
	}
}