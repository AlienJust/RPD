using System.Collections.Generic;
using System.Linq;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal static class PsnCmdPartExt {
		/// <summary>
		/// ���������� ����� ����������������� ��������� �� ��������
		/// </summary>
		/// <param name="prms">������ ���������������� ����������</param>
		/// <param name="name">�������� ���������</param>
		/// <returns>��������� �������� ��� null</returns>
		public static IPsnProtocolParameterDefinedConfiguration GetDefParamInfo(IEnumerable<IPsnProtocolParameterDefinedConfiguration> prms, string name)
		{
			return prms.FirstOrDefault(caValue => caValue.Name == name);
		}

		/// <summary>
		/// ���������� ����� ��������� ������������ �������� �� ��������
		/// </summary>
		/// <param name="prms">������ ���������� ����������</param>
		/// <param name="name">�������� ���������</param>
		/// <returns>��������� �������� ��� null</returns>
		public static IPsnProtocolParameterConfigurationVariable GetVarParamInfo(IEnumerable<IPsnProtocolParameterConfigurationVariable> prms, string name)
		{
			return prms.FirstOrDefault(caValue => caValue.Name == name);
		}
	}
}