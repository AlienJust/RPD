using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DAL;

namespace RPD.DalRelease.Configuration.System.Contracts {
	internal static class InterfaceRpdDumpRuleExtensions {
		public static bool IsEqual(this IRpdDumpRule rule, IRpdDumpRule anotherRule) {
			//если хоть один параметр не совпадает - считаем что правла не совпали (параметр Value не контролируется):
			if (rule.Type != anotherRule.Type) return false;
			if (rule.MaxValue != anotherRule.MaxValue) return false;
			if (rule.MinValue != anotherRule.MinValue) return false;
			if (rule.ControlValue != anotherRule.ControlValue) return false;
			return true;
		}

		public static void UpdateChannelCondition(IRpdDumpRule rule, IDumpCondition condtitionToUpdate)
		{
			//condtitionToModify.ConnectionPointIndex = connectionPointIndex;//connectionPointIndex определяется при создании RPD канала из записи таблицы измерителей
			condtitionToUpdate.IsUsed = condtitionToUpdate.ConnectionPointIndex > 0;

			condtitionToUpdate.ControlValue = rule.ControlValue;
			condtitionToUpdate.HighLimit = rule.MaxValue;
			condtitionToUpdate.LowLimit = rule.MinValue;

			condtitionToUpdate.UseValueAbs = (rule.Type & 0x01) == 0x01;
			condtitionToUpdate.UseHighLimit = (rule.Type & 0x02) == 0x02;
			condtitionToUpdate.UseLowLimit = (rule.Type & 0x04) == 0x04;
			condtitionToUpdate.UseControlValue = (rule.Type & 0x08) == 0x08;

			condtitionToUpdate.UseLogicalAnd = false;
		}
	}
}