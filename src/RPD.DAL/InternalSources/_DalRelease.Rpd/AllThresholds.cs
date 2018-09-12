using System;
using System.IO;

namespace RPD.DAL {
	internal class AllThresholds
	{
		public const int MaxCountPorog = 16;
		public Threshold[] Threshold { get; set; } //16 порогов для анализа
		public AllThresholds()
		{
			Threshold = new Threshold[MaxCountPorog];
			for (int i = 0; i < Threshold.Length; i++)
			{
				Threshold[i] = new Threshold();
			}
		}

		public AllThresholds(BinaryReader br)
		{
			Threshold = new Threshold[MaxCountPorog];
			for (int i = 0; i < Threshold.Length; i++)
			{
				Threshold[i] = new Threshold(br);
			}
		}

		public override string ToString()
		{
			string result = "";
			for (int i = 0; i < Threshold.Length; i++)
			{
				result += "  Threshold#:" + i + Environment.NewLine;
				result += Threshold[i].ToString();
			}
			return result;
		}
	}
}