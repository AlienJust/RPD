using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DAL;
using RPD.DalRelease.Rpd.Contracts;

namespace RPD.DalRelease.Rpd {
	internal class DumpConditionBuilderFromRpdDumpRule : IDumpConditionBuilder {
		private readonly IRpdDumpRule _rpdDumpRule;
		private readonly int _conditionPointIndex;
		public DumpConditionBuilderFromRpdDumpRule(IRpdDumpRule rpdDumpRule, int conditionPointIndex) {
			_rpdDumpRule = rpdDumpRule;
			_conditionPointIndex = conditionPointIndex;
		}
		public IDumpCondition Build() {
			bool isUsed = _conditionPointIndex > 0;

			var controlValue = _rpdDumpRule.ControlValue;
			var highLimit = _rpdDumpRule.MaxValue;
			var lowLimit = _rpdDumpRule.MinValue;

			var useValueAbs = (_rpdDumpRule.Type & 0x01) == 0x01;
			var useHighLimit = (_rpdDumpRule.Type & 0x02) == 0x02;
			var useLowLimit = (_rpdDumpRule.Type & 0x04) == 0x04;
			var useControlValue = (_rpdDumpRule.Type & 0x08) == 0x08;

			const bool useLogicalAnd = false;
			return new RpdChannellDumpCondition(_conditionPointIndex, highLimit, useHighLimit, lowLimit, useLowLimit, useValueAbs, useLogicalAnd, controlValue, useControlValue, isUsed);
		}
	}
}