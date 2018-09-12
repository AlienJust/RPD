using System;
using System.IO;

namespace RPD.DAL {
	internal class Calibrations
	{
		public const int MaxCountChannels = 16;
		public ChannelCalibration[] Channels;

		public Calibrations()
		{
			Channels = new ChannelCalibration[MaxCountChannels];
			for (int i = 0; i < Channels.Length; i++)
			{
				Channels[i] = new ChannelCalibration();
			}
		}

		public Calibrations(BinaryReader br)
		{
			Channels = new ChannelCalibration[MaxCountChannels];
			for (int i = 0; i < Channels.Length; i++)
			{
				Channels[i] = new ChannelCalibration(br);
			}
		}

		public override string ToString()
		{
			string result = "";
			for (int i = 0; i < Channels.Length; i++)
			{
				result += "  Channel#:" + i + Environment.NewLine;
				result += Channels[i].ToString();
			}
			return result;
		}
	}
}