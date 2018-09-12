using System.Collections.Generic;

namespace RpdCommandSystem.Contracts
{
	public interface IRpdControlCommand {
		int Code { get; }
		string Name { get; }
		IList<byte> Info { get; }
	}
}
