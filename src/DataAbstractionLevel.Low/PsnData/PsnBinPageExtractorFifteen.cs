using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData
{
	/// <summary>
	/// named in da memory of psn page header length
	/// </summary>
	sealed class PsnBinPageExtractorFifteen : IPsnPageExtractor {
		
		/// <summary>
		/// ƒлинна страницы
		/// </summary>
		public int PsnPageSize => 2048;

		/// <summary>
		/// ƒлина заголовка страницы
		/// </summary>
		public int PsnPageHeaderLength => 15;


		/// <summary>
		/// —тарт байт страницы
		/// </summary>
		public byte StartByteValue => 0x80;


		/// <summary>
		/// —мещение лет добавл€ть даты лога (то есть в бинарнике хранитс€ не 2010 год, например, а 0010 год)
		/// </summary>
		private const int PageYearsOffset = 2000;

		public PsnPageHeader GetHeaderFromRealDevice(byte[] headerRaw)
		{
			if (headerRaw.Length < PsnPageHeaderLength) throw new Exception("Ќевозможно создать заголовок страницы ѕ—Ќ. Ќедостаточное количество байт");

			var time = GetTimeFromHeaderBytes(headerRaw);
			var pageInfo = GetPageInfoFromHeaderBytes(headerRaw);
			var number = GetNumberFromHeaderBytes(headerRaw);
			if (pageInfo == PsnPageInfo.NormalPage) {
				// ¬ этом случае учитываетс€ и врем€, прошедшее с момента записи предыдущей страницы до момента записи этой страницы
				return new PsnPageHeader(time, TimeSpan.FromMilliseconds(((headerRaw[8] << 8) | headerRaw[9]) * 100), pageInfo, number);
			}
			// —траница сломана - нет смысла вычисл€ть врем€ с момента записи предыдущей страницы до момента записи этой страницы
			return new PsnPageHeader(time, TimeSpan.FromMilliseconds(0), pageInfo, number);
		}

		public bool StartByteIsOk(byte startByte)
		{
			return startByte == StartByteValue;
		}

		public PsnPageInfo GetPageInfoFromHeaderBytes(byte[] headerRaw, int offset) {
			if (headerRaw[offset + 0] != StartByteValue) return PsnPageInfo.BadPage;
			return PsnPageInfo.NormalPage;
		}
		public PsnPageInfo GetPageInfoFromHeaderBytes(byte[] headerRaw) {
			if (headerRaw[0] != StartByteValue) return PsnPageInfo.BadPage;
			return PsnPageInfo.NormalPage;
		}

		private DateTime? GetTimeFromHeaderBytes(byte[] headerRaw, int offset) {
			DateTime? result;
			if (headerRaw[offset + 0] != StartByteValue) result = null;
			else if (headerRaw[offset + 10] == 0xFF &&
						headerRaw[offset + 11] == 0xFF &&
						headerRaw[offset + 12] == 0xFF &&
						headerRaw[offset + 13] == 0xFF &&
						headerRaw[offset + 14] == 0xFF)
				result = null;

			else if (headerRaw[offset + 10] == 0x00 &&
						headerRaw[offset + 11] == 0x00 &&
						headerRaw[offset + 12] == 0x00 &&
						headerRaw[offset + 13] == 0x00 &&
						headerRaw[offset + 14] == 0x00)
				result = null;

			else {
				try {
					var millisecond = headerRaw[offset + 10] * 4;
					var second = (headerRaw[offset + 11] & 0xFC) >> 2;
					var minute = ((headerRaw[offset + 11] & 0x03) << 4) + ((headerRaw[offset + 12] & 0xF0) >> 4);
					var hour = ((headerRaw[offset + 12] & 0xF) << 1 ) + ((headerRaw[offset + 13] & 0x80) >> 7);
					var day = (headerRaw[offset + 13] & 0x7C) >> 2;
					
					var month = ((headerRaw[offset + 13] & 0x03) << 2) + ((headerRaw[offset + 14] & 0xC0) >> 6);
					var year = headerRaw[offset + 14] & 0x3F;
					
					result = new DateTime(PageYearsOffset + year, month, day, hour, minute, second, millisecond);
				}
				catch {
					result = null;
				}
			}
			return result;
		}
		private DateTime? GetTimeFromHeaderBytes(byte[] headerRaw) {
			DateTime? result;
			if (headerRaw[0] != StartByteValue) result = null;
			else if (headerRaw[10] == 0xFF &&
						headerRaw[11] == 0xFF &&
						headerRaw[12] == 0xFF &&
						headerRaw[13] == 0xFF &&
						headerRaw[14] == 0xFF)
				result = null;

			else if (headerRaw[10] == 0x00 &&
						headerRaw[11] == 0x00 &&
						headerRaw[12] == 0x00 &&
						headerRaw[13] == 0x00 &&
						headerRaw[14] == 0x00)
				result = null;

			else {
				try {
					var millisecond = headerRaw[10] * 4;
					var second = (headerRaw[11] & 0xFC) >> 2;
					var minute = ((headerRaw[11] & 0x03) << 4) + ((headerRaw[12] & 0xF0) >> 4);
					var hour = ((headerRaw[12] & 0xF) << 1) + ((headerRaw[13] & 0x80) >> 7);
					var day = (headerRaw[13] & 0x7C) >> 2;

					var month = ((headerRaw[13] & 0x03) << 2) + ((headerRaw[14] & 0xC0) >> 6);
					var year = headerRaw[14] & 0x3F;

					result = new DateTime(PageYearsOffset + year, month, day, hour, minute, second, millisecond);
				}
				catch {
					result = null;
				}
			}
			return result;
		}

		private int GetNumberFromHeaderBytes(IList<byte> headerBytes, int offset) {
			return headerBytes[offset + 7];
		}
		private int GetNumberFromHeaderBytes(IList<byte> headerBytes) {
			return headerBytes[7];
		}

		public PsnPageHeader GetHeaderFromRealDevice(byte[] data, int offset) {
			var time = GetTimeFromHeaderBytes(data, offset);
			var pageInfo = GetPageInfoFromHeaderBytes(data, offset);
			var number = GetNumberFromHeaderBytes(data, offset);
			if (pageInfo == PsnPageInfo.NormalPage) {
				// ¬ этом случае учитываетс€ и врем€, прошедшее с момента записи предыдущей страницы до момента записи этой страницы
				return new PsnPageHeader(time, TimeSpan.FromMilliseconds(((data[offset + 8] << 8) | data[offset + 9]) * 100), pageInfo, number);
			}
			// —траница сломана - нет смысла вычисл€ть врем€ с момента записи предыдущей страницы до момента записи этой страницы
			return new PsnPageHeader(time, TimeSpan.FromMilliseconds(0), pageInfo, number);
		}
	}
}