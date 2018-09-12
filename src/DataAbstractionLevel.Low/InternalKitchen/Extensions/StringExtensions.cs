using System;
using System.Globalization;

namespace DataAbstractionLevel.Low.InternalKitchen.Extensions {
	internal static class StringExtensions {
		public static int ToInt(this string toParse)
		{
			bool isHex = false;
			if (toParse.Length > 2)
			{
				if (toParse[1] == 'x')
				{
					isHex = true;
				}
			}
			return isHex ? int.Parse(toParse.Substring(2), NumberStyles.HexNumber) : int.Parse(toParse);
		}

		public static ByteAndBitPosition ToPos(this string toParse) {
			var parts = toParse.Split('.');
			if (parts.Length != 2)
				throw new FormatException("String '" + toParse + "' has wrong format");
			return new ByteAndBitPosition(int.Parse(parts[0]), int.Parse(parts[1]));
		}
	}
}