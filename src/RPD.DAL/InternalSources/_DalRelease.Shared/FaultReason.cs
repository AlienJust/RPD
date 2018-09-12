using System;
using System.Collections.Generic;
using System.Linq;

namespace RPD.DalRelease.Shared
{
	/// <summary>
	/// Причина аварии
	/// </summary>
	class FaultReason
	{
		#region Consts
		/// <summary>
		/// Максимальная длина ответа ПСН
		/// </summary>
		public const int PsnReplyMaxLength = 20;

		/// <summary>
		/// Число ответов ПСН устройств с т.з. РПД
		/// </summary>
		public const int PsnGaugesCount = 13; 
		#endregion

		/// <summary>
		/// Текущие данные каналов РПД
		/// </summary>
		public List<RpdChannelCurrentData> CurrentRpdData { get; private set; }

		/// <summary>
		/// Текущие данные каналов ПСН
		/// </summary>
		public List<PsnChannelCurrentData> CurrentPsnData { get; private set; }

		/// <summary>
		/// Список сигналов ПСН, предконфигурированых в РПД как определяющие аварию
		/// </summary>
		public List<PsnFaultReasonSignal> FaultConfiguredPsnSignals { get; private set; }

		/// <summary>
		/// Список сигналов ПСН, поддающихся предконфигурации в РПД как определяющие аварию
		/// </summary>
		public List<PsnFaultReasonSignal> PsnSignals { get; private set; }

		/// <summary>
		/// Сигналы, которые заданы в массиве правил
		/// </summary>
		public List<PsnFaultReasonSignal> RuledPsnSignals { get; private set; }

		public FaultReason(List<RpdChannelCurrentData> currentRpdData, List<PsnChannelCurrentData> currentPsnData)
		{
			FaultConfiguredPsnSignals = new List<PsnFaultReasonSignal>();
			PsnSignals = new List<PsnFaultReasonSignal>();
			RuledPsnSignals = new List<PsnFaultReasonSignal>();
			CurrentRpdData = currentRpdData ?? new List<RpdChannelCurrentData>();
			CurrentPsnData = currentPsnData ?? new List<PsnChannelCurrentData>();
		}

		public override string ToString()
		{
			var result = string.Empty;

			result += Environment.NewLine + "Список сигналов ПСН, предконфигурированых в РПД как определяющие аварию:" + Environment.NewLine;
			foreach (var psnCurData in CurrentPsnData)
			{
				if (psnCurData.Channel.IsFaultSign)
				{
					result += /*psnCurData.Channel.Configuration.PsnCmdPartInfoOwner.CommandInfoOwner.Name + */" > " + psnCurData.Channel.Name + " = " + psnCurData.Data + Environment.NewLine;
				}
			}

			foreach (var rpdCurData in CurrentRpdData)
			{
				if (rpdCurData.Channel.DumpCondition.IsUsed)
				{
					if (rpdCurData.Channel.DumpCondition.UseHighLimit)
					{
						if (rpdCurData.Data > rpdCurData.Channel.DumpCondition.HighLimit)
						{
							result += Environment.NewLine + rpdCurData.Channel.OwnerMeter.Name + " > канал " + rpdCurData.Channel.Name + " = " + rpdCurData.Data + " (превышение порога " + rpdCurData.Channel.DumpCondition.HighLimit + ")";
						}
					}
					if (rpdCurData.Channel.DumpCondition.UseLowLimit)
					{
						if (rpdCurData.Data < rpdCurData.Channel.DumpCondition.LowLimit)
						{
							result += Environment.NewLine + rpdCurData.Channel.OwnerMeter.Name + " > канал " + rpdCurData.Channel.Name + " = " + rpdCurData.Data + " (значение ниже порога " + rpdCurData.Channel.DumpCondition.LowLimit + ")";
						}
					}
				}
			}
			//result += FaultConfiguredPsnSignals.Aggregate(string.Empty, (current, signal) => current + (signal.PsnCmdName + " > " + signal.Name + " = " + signal.Value + Environment.NewLine));
			//result += Environment.NewLine + "Список сигналов ПСН, поддающихся предконфигурации в РПД как определяющие аварию:" + Environment.NewLine;
			//result += PsnSignals.Aggregate(string.Empty, (current, signal) => current + (signal.PsnCmdName + " > " + signal.Name + " = " + signal.Value + Environment.NewLine));
			
			return result;
		}
	}

	/// <summary>
	/// Причинный сигнал ПСН
	/// </summary>
	class PsnFaultReasonSignal
	{
		public string PsnCmdName { get; set; }
		public string Name { get; set; }
		public double Value { get; set; }
		public string Description { get; set; }
	}
}
