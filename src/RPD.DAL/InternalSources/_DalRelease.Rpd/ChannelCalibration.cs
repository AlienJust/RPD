using System;
using System.IO;

namespace RPD.DAL {
	internal class ChannelCalibration
	{
		public ChannelCalibration()
		{
			Kkor = 0;
			Kper = 0;
			Zero = 0;
			Ed = new ushort[3];
			Ed[0] = 0;
			Ed[1] = 0;
			Ed[2] = 0;
			//System.Single.
		}

		public ChannelCalibration(BinaryReader br)
		{
			Kkor = br.ReadSingle();
			Kper = br.ReadSingle();
			Zero = br.ReadUInt16();
			Ed = new ushort[3];
			Ed[0] = br.ReadUInt16();
			Ed[1] = br.ReadUInt16();
			Ed[2] = br.ReadUInt16();
			//System.Single.
		}

		public override string ToString()
		{
			string result = "";
			result += "    Kkor = " + Kkor + Environment.NewLine;
			result += "    Kper = " + Kper + Environment.NewLine;
			result += "    Zero = " + Zero + Environment.NewLine;

			result += "    Ed[0] = " + Ed[0] + Environment.NewLine;
			result += "    Ed[1] = " + Ed[1] + Environment.NewLine;
			result += "    Ed[2] = " + Ed[2] + Environment.NewLine;
			return result;
		}

		//тарировка одного канала АЦП,  size=8 word       
		public float Kkor { get; set; } //2-word   коэффициент корректирующий
		public float Kper { get; set; } //2-word   коэффициент передачи  
		public ushort Zero { get; set; } //1-word  нуль       
		public ushort[] Ed { get; set; } //3-word     единицы измерения            
	}
}