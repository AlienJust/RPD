using System;

namespace DataAbstractionLevel.Low.InternalKitchen.Extensions {
	internal struct ByteAndBitPosition {
		public int Byte;
		public int Bit;

		public ByteAndBitPosition(int byteNumber, int bitNumber) {
			if (bitNumber > 7 || bitNumber < 0) throw new ArgumentOutOfRangeException("bitNumber", bitNumber, "Bit number must be positive and less than 8");
			Byte = byteNumber;
			Bit = bitNumber;
		}
	}
}