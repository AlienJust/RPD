using System;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public static class PsnProtocolConfigurationExtensions
	{
		/// <summary>
		/// Для всех сигналов, относящихся к определенному измерителю
		/// </summary>
		/// <param name="psnConfig">Конфигурация ПСН</param>
		/// <param name="meterAddress">Адрес измерителя</param>
		/// <param name="progressAction">Действие при очередном нахождении конфигурации сигнала, если вернет истину - то метод остановится</param>
		public static void ForeachPsnMeterSignal(this IPsnProtocolConfiguration psnConfig, int meterAddress, Func<IPsnProtocolCommandPartConfiguration, IPsnProtocolParameterConfiguration, bool> progressAction)
		{
			foreach (var cmdPart in psnConfig.CommandParts)
			{
				if (cmdPart != null)
				{
					if (cmdPart.Address != null)
					{
						if (cmdPart.Address.DefinedValue == meterAddress)
						{
							foreach (var param in cmdPart.VarParams)
							{
								if (!param.Name.StartsWith("#")) {
									if (progressAction(cmdPart, param)) {
										return;
									}
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Для всех сигналов конфигурации
		/// </summary>
		/// <param name="psnConfig">Конфигурация ПСН</param>
		/// <param name="progressAction">Действие при очередном нахождении конфигурации сигнала, если вернет истину - то метод остановится</param>
		public static void ForeachPsnParameterConfig(this IPsnProtocolConfiguration psnConfig, Func<IPsnProtocolCommandPartConfiguration, IPsnProtocolParameterConfiguration, bool> progressAction) {
			foreach (var cmdPart in psnConfig.CommandParts) {
				if (cmdPart != null) {
					if (cmdPart.Address != null) {
						foreach (var param in cmdPart.VarParams) {
							if (progressAction(cmdPart, param)) {
								return;
							}
						}
					}
				}
			}
		}
	}
}