using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	interface IPsnLogSaver {
		void SaveLogToRepository(IPsnData data);
	}
}