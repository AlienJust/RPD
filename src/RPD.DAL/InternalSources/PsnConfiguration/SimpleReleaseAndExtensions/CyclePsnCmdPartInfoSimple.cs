using System;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class CyclePsnCmdPartInfoSimple : PsnCommandPartInfoSimple, ICyclePsnCmdPartInfo {
		private readonly TimeSpan _cyclePeriod;
		public CyclePsnCmdPartInfoSimple(IPsnCommandPartInfo psnCmdPart, TimeSpan cyclePeriod)
			: base(
			psnCmdPart.Id, 
			psnCmdPart.PartName, 
			psnCmdPart.PartType,
			psnCmdPart.DefParams, 
			psnCmdPart.VarParams, 
			psnCmdPart.Length, 
			psnCmdPart.Offset, 
			psnCmdPart.Address, 
			psnCmdPart.CommandCode,
			psnCmdPart.CrcLow, 
			psnCmdPart.CrcHigh, 
			psnCmdPart.CommandId)
		{
			_cyclePeriod = cyclePeriod;
		}

		public TimeSpan CyclePeriod {
			get { return _cyclePeriod; }
		}
	}
}