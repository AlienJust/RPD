using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.Contracts
{
	public interface ICustomConfiguration
	{
		string LocomotiveName { get; set; }
		int SectionNumber { get; set; }
		int PsnVersion { get; set; }
		IList<IRpdMeterCustomInfo> RpdMetersCustomInfos { get; }
	}
}
