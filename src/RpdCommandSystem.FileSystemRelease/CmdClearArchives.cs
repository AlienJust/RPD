using System.Collections.Generic;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class CmdClearArchives : IRpdControlCommand
	{
		private readonly IList<byte> _request = new byte[0];

		public int Code {
			get { return 0x57; }
		}

		public string Name {
			get { return "Очистка архивов"; }
		}

		public IList<byte> Info
		{
			get { return _request; }
		}
	}
}