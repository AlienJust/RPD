using System;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	internal struct PsnCommandPartLocation
	{
		public readonly int Position;
		public readonly DateTime Time;

		public PsnCommandPartLocation(int position, DateTime time)
		{
			Position = position;
			Time = time;
		}
		/*
		public override string ToString()
		{
			return TicksOffset + " > " + Position + " > " + Validation.ToCustomString();
		}*/
	}
}