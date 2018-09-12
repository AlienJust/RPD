using System.Collections.Generic;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class CmdTestLinkWithMeters : IRpdControlCommand
	{
		private readonly IList<byte> _request = new byte[0];

		public int Code
		{
			get { return 0x58; }
		}

		public string Name
		{
			get { return "Проверка связи с измерителями"; }
		}

		public IList<byte> Info
		{
			get { return _request; }
		}
	}
}