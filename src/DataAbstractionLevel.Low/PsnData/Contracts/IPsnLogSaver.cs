using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	interface IPsnLogSaver {
		void SaveLogToRepository(IPsnData data);
	}
}