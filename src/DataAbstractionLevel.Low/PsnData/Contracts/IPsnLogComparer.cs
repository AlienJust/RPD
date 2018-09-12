using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	interface IPsnLogComparer {
		bool AreLogsTheSame(IPsnData dataInfo1, IPsnData dataInfo2);
	}
}