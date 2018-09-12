using System.IO;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace RPD.DAL {
	class StreamReadableDataBasedOnObject : IStreamedData
	{
		private readonly IStreamReadableObject _obj;
		public StreamReadableDataBasedOnObject(IStreamReadableObject obj)
		{
			_obj = obj;
		}

		public Stream GetStreamForReading()
		{
			return _obj.GetStreamForReading();
		}
	}
}