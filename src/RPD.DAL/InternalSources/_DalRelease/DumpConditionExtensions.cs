namespace RPD.DAL {
	internal static class DumpConditionExtensions {
		public static bool IsEqualTo(this IDumpCondition c1, IDumpCondition c2) {
			if (c1 == null && c2 == null) return true;
			if (c1 == null || c2 == null) return false; // предыдущий return делает код правильным

			if (c1.ConnectionPointIndex != c2.ConnectionPointIndex) return false;
			if (c1.ControlValue != c2.ControlValue) return false;
			if (c1.HighLimit != c2.HighLimit) return false;
			if (c1.IsUsed != c2.IsUsed) return false;
			if (c1.LowLimit != c2.LowLimit) return false;
			if (c1.UseControlValue != c2.UseControlValue) return false;
			if (c1.UseHighLimit != c2.UseHighLimit) return false;
			if (c1.UseLowLimit != c2.UseLowLimit) return false;
			if (c1.UseLogicalAnd != c2.UseLogicalAnd) return false;
			if (c1.UseValueAbs != c2.UseValueAbs) return false;
			return true;
		}
	}
}