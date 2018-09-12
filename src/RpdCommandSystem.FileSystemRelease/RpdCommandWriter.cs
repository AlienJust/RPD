using System;
using System.IO;
using System.Linq;
using System.Threading;
using AlienJust.Support.IO;
using AlienJust.Support.Numeric;
using RpdCommandSystem.Contracts;

namespace RpdCommandSystem.FileSystemRelease {
	public sealed class RpdCommandWriter : IRpdCommandWriter {
		private readonly string _cmdBinPath;

		public RpdCommandWriter(string driveLetter) {
			_cmdBinPath = driveLetter.Substring(0, 1) + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar + "COMMAND.BIN";
			Console.WriteLine("CmdBinPath = " + _cmdBinPath);
		}

		public void WriteCommandSync(IRpdControlCommand cmd, int pauseSeconds) {
			using (var bw = new AdvancedBinaryWriter(new FileStream(_cmdBinPath, FileMode.Open, FileAccess.ReadWrite), false)) {
				var code = (byte) cmd.Code;
				var info = cmd.Info;
				var lenH = (byte) ((info.Count & 0xFF00) >> 8);
				var lenL = (byte)(info.Count & 0xFF);
				var cmdRequest = new byte[1 + 2 + info.Count];
				cmdRequest[0] = code;
				cmdRequest[1] = lenH;
				cmdRequest[2] = lenL;
				info.CopyTo(cmdRequest, 3);
				var crc = MathExtensions.GetCrc16FromList(cmdRequest.ToList());
				
				var resultRequest = new byte[cmdRequest.Length + 2];
				cmdRequest.CopyTo(resultRequest, 0);
				resultRequest[resultRequest.Length - 2] = crc.High;
				resultRequest[resultRequest.Length - 1] = crc.Low;
				bw.Write(resultRequest);
				bw.BaseStream.Close();
			}
			Thread.Sleep(pauseSeconds*1000);
		}
	}
}