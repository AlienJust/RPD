using System;
using System.IO;
using AlienJust.Support.IO;

namespace RPD.DalRelease.Configuration.System {
	internal class FaultLogTableRecord //19
	{
		public byte Number { get; private set; } //0
		public byte Status { get; private set; } //1
		public byte Year { get; private set; } //2
		public byte Month { get; private set; } //3
		public byte Day { get; private set; } //4
		public byte Hour { get; private set; } //5
		public byte Minute { get; private set; } //6
		public byte Second { get; private set; } //7
		public int DescriptionPageAddress { get; private set; } //8 9 10 11
		public int LastWrittenPageAddress { get; private set; } //12 13 14 15
		public byte FaultWasReaded { get; private set; } //16
		public short BadPageCounter { get; private set; } //17 18

		public FaultLogTableRecord() {
			Number = 0;
			Status = 0;
			Year = 0;
			Month = 0;
			Day = 0;
			Hour = 0;
			Minute = 0;
			Second = 0;
			DescriptionPageAddress = 0;
			LastWrittenPageAddress = 0;
			BadPageCounter = 0;
		}

		public void ReadFromStream(Stream stream) {
			var br = new AdvancedBinaryReader(stream, false);
			Number = br.ReadByte();
			Status = br.ReadByte();
			Day = br.ReadByte();
			Month = br.ReadByte();
			Year = br.ReadByte();
			Hour = br.ReadByte();
			Minute = br.ReadByte();
			Second = br.ReadByte();

			DescriptionPageAddress = br.ReadInt32();
			LastWrittenPageAddress = br.ReadInt32();

			FaultWasReaded = br.ReadByte();
			BadPageCounter = br.ReadInt16();
		}

		public override string ToString() {
			string result;
			var splitter = Environment.NewLine;
			try {
				result =
					"PageNumber=" + Number + splitter +
					"Status=" + Status + splitter +
					"Year=" + Year + splitter +
					"Month=" + Month + splitter +

					"Day=" + Day + splitter +
					"Hour=" + Hour + splitter +
					"Minute=" + Minute + splitter +
					"Second=" + Second + splitter +

					"DescriptionPageAddress=" + DescriptionPageAddress + splitter +
					"LastWrittenPageAddress=" + LastWrittenPageAddress + splitter +
					"FaultWasReaded=" + FaultWasReaded + splitter +
					"BadPageCounter=" + BadPageCounter;
			}
			catch (Exception ex) {
				result = ex.ToString();
			}
			return result;
		}
	}
}