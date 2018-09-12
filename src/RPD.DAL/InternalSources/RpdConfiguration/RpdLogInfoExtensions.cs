namespace RPD.DAL {
	internal static class RpdLogInfoExtensions
	{
		public static bool IsEqualTo(this IRpdLogInfo info1, IRpdLogInfo info2)
		{
			if (info1.BadPageCounter != info2.BadPageCounter) return false;
			if (info1.Day != info2.Day) return false;
			if (info1.DescriptionPageAddress != info2.DescriptionPageAddress) return false;
			if (info1.FaultWasReaded != info2.FaultWasReaded) return false;
			if (info1.Hour != info2.Hour) return false;
			if (info1.LastWrittenPageAddress != info2.LastWrittenPageAddress) return false;
			if (info1.Minute != info2.Minute) return false;
			if (info1.Month != info2.Month) return false;
			if (info1.Number != info2.Number) return false;
			if (info1.Second != info2.Second) return false;
			if (info1.Status != info2.Status) return false;
			if (info1.Year != info2.Year) return false;
			return true;
		}
	}
}