using System;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace RPD.DAL.DataExtraction.SimpleRelease {
	internal sealed class DataPointSimple : IDataPoint
	{
		private readonly IBackSearchData _backSearch;

		public DataPointSimple(double value, DateTime time, bool valid, int dataPosition, IBackSearchData backSearch)
		{
			_backSearch = backSearch;
			Value = value;
			Time = time;
			Valid = valid;
			DataPosition = dataPosition;
		}

		public double Value { get; }

		public DateTime Time { get; }

		public bool Valid { get; }

		public int DataPosition { get; }
		
		public byte[] GetCommandBytes()
		{
			return _backSearch.GetCommandPartBytes(DataPosition);
		}
	}
}