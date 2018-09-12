using System;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal static class PsnParamValueExtractor {
		private static readonly Random Rnd = new Random();
		public static byte GetByteValueFromBytes(byte[] cmdPartContext, int startByte, int posByte)
		{
			return cmdPartContext[startByte + posByte];
		}

		public static sbyte GetSByteValueFromBytes(byte[] cmdPartContext, int startByte, int posByte)
		{
			return unchecked ((sbyte)cmdPartContext[startByte + posByte]);
		}

		public static bool GetBitValueFromBytes(byte[] cmdPartContext, int startByte, int posByte, int posBit)
		{
			return (cmdPartContext[startByte + posByte] & (0x01 << posBit)) > 0;
		}

		public static int GetMultibitUnsignedValueFromBytes(byte[] cmdPartContext, int startByte, int posByte, int posBit, int bitsCount)
		{
			int result = 0;
			int curBytePos = posByte + startByte;
			int curBitPos = posBit;

			int bits = bitsCount; // ����� ������� �� ���� �������� CmdValue (If IsValueSigned { PsnPageHeaderLength--; } (�����������)

			for (int i = 0; i < bits; ++i) // ������� ��� ���� � ������ ��������:
			{
				result = result | (((cmdPartContext[curBytePos] & (0x01 << curBitPos)) >> curBitPos) << i);

				curBitPos++;
				if (curBitPos == 8) //���� �������� ����� �����, �� ��������� � ���������� �����:
				{
					curBitPos = 0;
					++curBytePos;
				}
			}

			return result; // �������� �� ���������
		}

		public static int GetMultibitSignedValueFromBytes(byte[] cmdPartContext, int startByte, int posByte, int posBit, int bitsCount)
		{
			int result = 0;
			int curBytePos = posByte + startByte;
			int curBitPos = posBit;

			int bits = bitsCount - 1;


			for (int i = 0; i < bits; ++i) // ������� ��� ���� � ������ ��������:
			{
				result = result | (((cmdPartContext[curBytePos] & (0x01 << curBitPos)) >> curBitPos) << i);

				curBitPos++;
				if (curBitPos == 8) //���� �������� ����� �����, �� ��������� � ���������� �����:
				{
					curBitPos = 0;
					curBytePos++;
				}
			}

			if ((cmdPartContext[curBytePos] & (0x01 << curBitPos)) != 0) // ���� �������� �������� � ���� �����:
			{
				result = 0 - result;
			}
			
			return result;
		}

		public static int GetRandomIntValue(int maxValue) {
			return Rnd.Next(maxValue);
		}
	}
}