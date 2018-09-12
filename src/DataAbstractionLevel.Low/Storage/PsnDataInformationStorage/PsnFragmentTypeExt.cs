using System;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage {
	internal static class PsnFragmentTypeExt
	{
		public static string ToSimpleString(this PsnDataFragmentType value) {
			switch (value) {
				case PsnDataFragmentType.FixedLength:
					return "F";
				case PsnDataFragmentType.PowerDepended:
					return "P";
				case PsnDataFragmentType.LinkedToFault:
					return "L";
			}
			throw new Exception("Cannot convert value to string");
		}

		public static PsnDataFragmentType ToPsnLogType(this string value)
		{
			var strBegin = value.Substring(0, 1);
			switch (strBegin)
			{
				case "F":
					return PsnDataFragmentType.FixedLength;
				case "P":
					return PsnDataFragmentType.PowerDepended;
				case "L":
					return PsnDataFragmentType.LinkedToFault;
			}
			throw new Exception("Cannot convert string to PsnDataType");
		}
	}
}