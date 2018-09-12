using System;
using System.Collections.Generic;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class CmdSetTime : IRpdControlCommand {
		private readonly IList<byte> _request;

		public CmdSetTime(DateTime time) {
			_request =
				new[] {
				      	(byte) time.Second,
				      	(byte) time.Minute,
				      	(byte) time.Hour,
				      	(byte) time.Day,
				      	(byte) time.Month,
				      	(byte) (time.Year - 2000)
				      };
		}

		public int Code {
			get { return 0x59; }
		}

		public string Name {
			get { return "Установка времени внутренних часов РПД"; }
		}

		public IList<byte> Info
		{
			get { return _request; }
		}
	}
}