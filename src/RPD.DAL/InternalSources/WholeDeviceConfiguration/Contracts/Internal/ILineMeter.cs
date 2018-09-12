using System.Collections.Generic;

namespace RPD.DAL.WholeDeviceConfiguration.Contracts.Internal {
	internal interface ILineMeter
	{
		string Name { get; set; }
		int Address { get; set; }
		IEnumerable<ILineMeterChannel> Channels { get; }
	}
}