using System;

namespace DataAbstractionLevel.Low.PsnData {
	internal struct PsnPageNumberAndTime
	{
		public DateTime? Time;
		public int Number;

		public PsnPageNumberAndTime(int number, DateTime? time)
		{
			Time = time;
			Number = number;
		}
	}
}