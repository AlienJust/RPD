using System;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.Shared {
	internal sealed class PsnDataInformationSimple : IPsnDataInformation
	{
		private readonly IIdentifier _id;
		private readonly DateTime? _beginTime;
		private readonly DateTime? _endTime;
		private readonly DateTime? _saveTime;
		private readonly PsnDataFragmentType _dataDataFragmentType;
		private readonly bool _isLastDeviceLog;
		private readonly IIdentifier _deviceInformationId;

		public PsnDataInformationSimple(string id, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType dataDataFragmentType, bool isLastDeviceLog, string deviceInformationId)
		{
			_id = new IdentifierStringToLowerBased(id);
			_beginTime = beginTime;
			_endTime = endTime;
			_saveTime = saveTime;
			_dataDataFragmentType = dataDataFragmentType;
			_isLastDeviceLog = isLastDeviceLog;
			_deviceInformationId = new IdentifierStringToLowerBased(deviceInformationId);
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public DateTime? BeginTime {
			get { return _beginTime; }
		}

		public DateTime? EndTime {
			get { return _endTime; }
		}

		public DateTime? SaveTime {
			get { return _saveTime; }
		}

		public PsnDataFragmentType DataFragmentType
		{
			get { return _dataDataFragmentType; }
		}

		public bool IsLastDeviceLog {
			get { return _isLastDeviceLog; }
		}

		public IIdentifier DeviceInformationId
		{
			get { return _deviceInformationId; }
		}
	}
}