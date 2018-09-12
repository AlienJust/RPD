using System;
using System.IO;

namespace RPD.DAL {
	internal class Threshold //настройка порогов
	{
		public ushort SetupCh1 { get; set; } //:4;
		public ushort SetupCh2 { get; set; } //:4;
		public ushort SetupCalc { get; set; } //:2;//0,1,2,3 - мгновенное/эффективное/спектр/частота
		public ushort SetupCh2Enable { get; set; } //:1;//есть второй канал
		public ushort SetupPlav { get; set; } //:1; //плавающий порог
		public ushort SetupMin { get; set; } //:1;//анализировать по миним
		public ushort SetupMax { get; set; } //:1;//...по макс
		public ushort SetupPri { get; set; } //:1;//...по приращению
		public ushort SetupInv { get; set; } //:1;//норма в диапазоне

		public ushort Reserv1 { get; set; }
		public float Min { get; set; }
		public float Max { get; set; }
		public float Pri { get; set; }
		public ushort Freq { get; set; } //основная (или ожидаемая) частота
		public ushort Reserv2 { get; set; }

		public Threshold()
		{
			SetupCh1 = 0;
			SetupCh2 = 0;

			SetupCalc = 0;
			SetupCh2Enable = 0;
			SetupPlav = 0;
			SetupMin = 0;
			SetupMax = 0;
			SetupPri = 0;
			SetupInv = 0;
			//------------------------------------------------------- 2 bytes
			Reserv1 = 0;
			Min = 0;
			Max = 0;
			Pri = 0;

			Freq = 0;
			Reserv2 = 0;
		}

		public Threshold(BinaryReader br)
		{
			byte firstByte = br.ReadByte();
			SetupCh1 = (ushort) (firstByte & 0xF);
			SetupCh2 = (ushort) ((firstByte & 0x0F) >> 4);

			byte secondByte = br.ReadByte();
			SetupCalc = (ushort) (secondByte & 0x3);
			SetupCh2Enable = (ushort) ((secondByte & 0x4) >> 2);
			SetupPlav = (ushort) ((secondByte & 0x8) >> 3);
			SetupMin = (ushort) ((secondByte & 0x10) >> 4);
			SetupMax = (ushort) ((secondByte & 0x20) >> 5);
			SetupPri = (ushort) ((secondByte & 0x40) >> 6);
			SetupInv = (ushort) ((secondByte & 0x80) >> 7);
			//------------------------------------------------------- 2 bytes
			Reserv1 = br.ReadUInt16();
			Min = br.ReadSingle();
			Max = br.ReadSingle();
			Pri = br.ReadSingle();

			Freq = br.ReadUInt16();
			Reserv2 = br.ReadUInt16();
		}

		public override string ToString()
		{
			string result = "";
			result += "    SetupCh1 = " + SetupCh1 + Environment.NewLine;
			result += "    SetupCh2 = " + SetupCh2 + Environment.NewLine;
			result += "    SetupCalc = " + SetupCalc + Environment.NewLine;
			result += "    SetupCh2Enable = " + SetupCh2Enable + Environment.NewLine;

			result += "    SetupPlav = " + SetupPlav + Environment.NewLine;
			result += "    SetupMin = " + SetupMin + Environment.NewLine;
			result += "    SetupMax = " + SetupMax + Environment.NewLine;
			result += "    SetupPri = " + SetupPri + Environment.NewLine;
			result += "    SetupInv = " + SetupInv + Environment.NewLine;
			//-------------------------------------------------------
			result += "    Reserv1 = " + Reserv1 + Environment.NewLine;
			result += "    Min = " + Min + Environment.NewLine;
			result += "    Max = " + Max + Environment.NewLine;
			result += "    Pri = " + Pri + Environment.NewLine;

			result += "    Freq = " + Freq + Environment.NewLine;
			result += "    Reserv2 = " + Reserv2 + Environment.NewLine;

			return result;
		}

	}
}