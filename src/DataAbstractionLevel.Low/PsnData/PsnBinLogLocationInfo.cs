namespace DataAbstractionLevel.Low.PsnData {
	internal struct PsnBinLogLocationInfo
	{
		private readonly string _psnBinFileName;

		public string PsnBinFileName
		{
			get { return _psnBinFileName; }
		}

		private readonly int _firstPageIndex;

		public int FirstPageIndex
		{
			get { return _firstPageIndex; }
		}

		private readonly int _lastPageIndex;

		public int LastPageIndex
		{
			get { return _lastPageIndex; }
		}

		public PsnBinLogLocationInfo(string fileName, int firstPageIndex, int lastPageIndex)
		{
			_psnBinFileName = fileName;
			_firstPageIndex = firstPageIndex;
			_lastPageIndex = lastPageIndex;
		}
	}
}