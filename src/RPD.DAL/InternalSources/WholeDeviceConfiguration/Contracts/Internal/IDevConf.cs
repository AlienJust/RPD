using System;
using System.Collections.Generic;

namespace RPD.DAL.WholeDeviceConfiguration.Contracts.Internal {
	internal interface IDevConf
	{
		string LocomotiveName { get; set; }
		int SectionNumber { get; set; }
		int Address { get; set; }
		int NetAddress { get; set; }
		bool UseHammingForNand { get; set; }
		bool LogPsn { get; set; }
		bool SaveByteInterval { get; set; }
		bool ResetFaultsDump { get; set; }
		IDeviceDiagnostic Diagnostic { get; }
		DateTime? CurrentTime { get; }
		IEnumerable<ILineMeter> LineMeters { get; }
	}
}