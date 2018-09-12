using System.IO;
using AlienJust.Support.IO;
using RPD.DAL;
using RPD.DalRelease.Configuration.System.Contracts;

namespace RPD.DalRelease.Configuration.System {
	class PsnLogFragmentInfoSaverToStream : IPsnLogFragmentInfoSaver
	{
		private readonly Stream _stream;

		public PsnLogFragmentInfoSaverToStream(Stream stream)
		{
			_stream = stream;
		}

		public void Save(IPsnLogFragmentInfo psnLogFragmentInfo)
		{
			var bw = new AdvancedBinaryWriter(_stream, false);
			bw.Write((uint)psnLogFragmentInfo.StartOffset);

			var day = (byte) (psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Day : 0);
			var month = (byte)(psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Month : 0);
			var year = (byte)(psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Year - 2000 : 0);
			var hour = (byte)(psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Hour : 0);
			var minute = (byte)(psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Minute : 0);
			var second = (byte)(psnLogFragmentInfo.StartTime.HasValue ? psnLogFragmentInfo.StartTime.Value.Second : 0);
			bw.Write(day);
			bw.Write(month);
			bw.Write(year);
			bw.Write(hour);
			bw.Write(minute);
			bw.Write(second);
		}
	}
}