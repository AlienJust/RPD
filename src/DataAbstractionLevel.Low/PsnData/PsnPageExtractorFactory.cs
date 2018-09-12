using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	internal static class PsnPageExtractorFactory {
		//private static readonly IPsnPageExtractor CurrentExtractor = new PsnBinPageExtractor();
		public static IPsnPageExtractor Extractor { get; } = new PsnBinPageExtractorFifteen();
	}
}