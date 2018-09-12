using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DAL;
using RPD.DalRelease.Configuration.System.Contracts;

namespace RPD.DalRelease.Configuration.System {
	class RpdDumpRuleBuilderFromDumpCondition : IRpdDumpRuleBuilder
	{
		private readonly IDumpCondition _condition;

		public RpdDumpRuleBuilderFromDumpCondition(IDumpCondition condition)
		{
			_condition = condition;
		}

		public IRpdDumpRule Build()
		{
			var rule = new RpdDumpRuleSimple {Type = 0};
			if (_condition.UseValueAbs) rule.Type += 0x01;
			if (_condition.UseHighLimit) rule.Type += 0x02;
			if (_condition.UseLowLimit) rule.Type += 0x04;
			if (_condition.UseControlValue) rule.Type += 0x08;

			rule.MaxValue = (short)_condition.HighLimit;
			rule.MinValue = (short)_condition.LowLimit;
			rule.ControlValue = (short)_condition.ControlValue;
			return rule;
		}
	}
}