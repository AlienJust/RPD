using System;

namespace DataAbstractionLevel.Low.PsnData {
	internal struct PsnBinLogAdvancedLocationInfo {
		public readonly string PsnBinFileName;
		private PsnPageNumberAndTime _firstPageInfo;
		private PsnPageNumberAndTime _lastPageInfo;
		private PsnPageNumberAndTime? _firstDatedPageInfo;

		public PsnBinLogAdvancedLocationInfo(string fileName, PsnPageNumberAndTime firstPageInfo, PsnPageNumberAndTime lastPageInfo) {
			PsnBinFileName = fileName;
			IsLastPsnLogOnDevice = false;

			_firstPageInfo = firstPageInfo;
			_lastPageInfo = lastPageInfo;
	
			_firstDatedPageInfo = null;
		}

		public void UpdateFirstDatedPageInfo(PsnPageNumberAndTime? info) {
			_firstDatedPageInfo = info;
		}

		public bool IsLastPsnLogOnDevice { get; set; }

		public PsnPageNumberAndTime FirstPageInfo => _firstPageInfo;

		public PsnPageNumberAndTime? FirstDatedPageInfo => _firstDatedPageInfo;

		public PsnPageNumberAndTime LastPageInfo => _lastPageInfo;

		public void UpdateFirstPageInfoTime(DateTime? time) {
			_firstPageInfo.Time = time;
		}

		public void UpdateLastPageInfoTime(DateTime? time)
		{
			_lastPageInfo.Time = time;
		}
	}
}