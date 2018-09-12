using System.Collections.Generic;
using System.Linq;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal static class PsnCmdPartExt {
		/// <summary>
		/// Производит поиск предопределенного параметра по названию
		/// </summary>
		/// <param name="prms">Список предопределенных параметров</param>
		/// <param name="name">Название параметра</param>
		/// <returns>Найденный параметр или null</returns>
		public static IPsnProtocolParameterDefinedConfiguration GetDefParamInfo(IEnumerable<IPsnProtocolParameterDefinedConfiguration> prms, string name)
		{
			return prms.FirstOrDefault(caValue => caValue.Name == name);
		}

		/// <summary>
		/// Производит поиск параметра неизвестного значения по названию
		/// </summary>
		/// <param name="prms">Список переменных параметров</param>
		/// <param name="name">Название параметра</param>
		/// <returns>Найденный параметр или null</returns>
		public static IPsnProtocolParameterConfigurationVariable GetVarParamInfo(IEnumerable<IPsnProtocolParameterConfigurationVariable> prms, string name)
		{
			return prms.FirstOrDefault(caValue => caValue.Name == name);
		}
	}
}