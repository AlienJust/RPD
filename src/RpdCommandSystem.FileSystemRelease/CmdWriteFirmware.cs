using System.Collections.Generic;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class CmdWriteFirmware : IRpdControlCommand {
		private readonly IList<byte> _request = new byte[0];

		public int Code
		{
			get { return 0x55; }
		}

		public string Name
		{
			get { return "Запись прошивки РПД"; }
		}

		public IList<byte> Info
		{
			get { return _request; }
		}
	}
}