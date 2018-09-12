using System.Collections.Generic;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class CmdFormatRpd: IRpdControlCommand
	{
		private readonly IList<byte> _request = new byte[0];

		public int Code
		{
			get { return 0x52; }
		}

		public string Name
		{
			get { return "Переформатировать РПД"; }
		}

		public IList<byte> Info
		{
			get { return _request; }
		}
	}
}