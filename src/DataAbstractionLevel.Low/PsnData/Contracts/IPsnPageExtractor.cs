using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	interface IPsnPageExtractor {
		int PsnPageSize { get; }

		int PsnPageHeaderLength { get; }

		byte StartByteValue { get; }

		PsnPageHeader GetHeaderFromRealDevice(byte[] headerRaw);
		PsnPageHeader GetHeaderFromRealDevice(byte[] data, int offset);

		bool StartByteIsOk(byte startByte);

		PsnPageInfo GetPageInfoFromHeaderBytes(byte[] headerRaw);
	}
}