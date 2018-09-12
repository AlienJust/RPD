using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal static class PsnCommandPartInfoExtensions {
		public static bool IsEqualTo(this IPsnProtocolCommandPartConfiguration part1, IPsnProtocolCommandPartConfiguration part2)
		{
			if (!DefinedPsnParameterInfoExtensions.IsEqualTo(part1.Address, part2.Address)) return false;
			if (!DefinedPsnParameterInfoExtensions.IsEqualTo(part1.CommandCode, part2.CommandCode)) return false;
			if (!part1.CrcHigh.IsEqualTo(part2.CrcHigh)) return false;
			if (!part1.CrcLow.IsEqualTo(part2.CrcLow)) return false;
			if (part1.DefParams.Length != part2.DefParams.Length) return false;
			for (int i = 0; i < part1.DefParams.Length; ++i) {
				if (!DefinedPsnParameterInfoExtensions.IsEqualTo(part1.DefParams[i], part2.DefParams[i])) return false;
			}
			if (part1.Length != part2.Length) return false;
			if (part1.Offset != part2.Offset) return false;
			if (part1.PartName != part2.PartName) return false;
			if (part1.VarParams.Length != part2.VarParams.Length) return false;
			for (int i = 0; i < part1.VarParams.Length; ++i) {
				if (!part1.VarParams[i].IsEqualTo(part2.VarParams[i])) return false;
			}
            
			if (part1.PartType != part2.PartType) return false;
			if (part1.CommandId.ToString() != part2.CommandId.ToString()) return false;
			return true;
		}
	}
}