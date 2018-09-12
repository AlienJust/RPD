using System.Collections.Generic;
using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnCommandPartInfoSimple : IPsnCommandPartInfo {
		private readonly IUid _id;
		private readonly string _partName;
		private readonly PsnCommandPartType _partType;
		private readonly List<IDefinedPsnParameterInfo> _defParams;
		private readonly List<IVariablePsnParameterInfo> _varParams;
		private readonly int _length;
		private readonly int _offset;
		private readonly IDefinedPsnParameterInfo _address;
		private readonly IDefinedPsnParameterInfo _commandCode;
		private readonly IVariablePsnParameterInfo _crcLow;
		private readonly IVariablePsnParameterInfo _crcHigh;
		private readonly IUid _commandId;
		public PsnCommandPartInfoSimple(IUid id, string partName, PsnCommandPartType partType, List<IDefinedPsnParameterInfo> defParams, List<IVariablePsnParameterInfo> varParams, int length, int offset, IDefinedPsnParameterInfo address, IDefinedPsnParameterInfo commandCode, IVariablePsnParameterInfo crcLow, IVariablePsnParameterInfo crcHigh, IUid commandId) {
			_id = id;
			_partName = partName;
			_partType = partType;
			_defParams = defParams;
			_varParams = varParams;
			_length = length;
			_offset = offset;
			_address = address;
			_commandCode = commandCode;
			_crcLow = crcLow;
			_crcHigh = crcHigh;
			_commandId = commandId;
		}

		public IUid Id {
			get { return _id; }
		}

		public string PartName {
			get { return _partName; }
		}

		public PsnCommandPartType PartType {
			get { return _partType; }
		}

		public List<IDefinedPsnParameterInfo> DefParams {
			get { return _defParams; }
		}

		public List<IVariablePsnParameterInfo> VarParams {
			get { return _varParams; }
		}

		public int Length {
			get { return _length; }
		}

		public int Offset {
			get { return _offset; }
		}

		public IDefinedPsnParameterInfo Address {
			get { return _address; }
		}

		public IDefinedPsnParameterInfo CommandCode {
			get { return _commandCode; }
		}

		public IVariablePsnParameterInfo CrcLow {
			get { return _crcLow; }
		}

		public IVariablePsnParameterInfo CrcHigh {
			get { return _crcHigh; }
		}

		public IUid CommandId {
			get { return _commandId; }
		}
	}
}