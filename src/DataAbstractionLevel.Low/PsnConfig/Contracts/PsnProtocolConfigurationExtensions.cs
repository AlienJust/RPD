using System;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public static class PsnProtocolConfigurationExtensions
	{
		/// <summary>
		/// ��� ���� ��������, ����������� � ������������� ����������
		/// </summary>
		/// <param name="psnConfig">������������ ���</param>
		/// <param name="meterAddress">����� ����������</param>
		/// <param name="progressAction">�������� ��� ��������� ���������� ������������ �������, ���� ������ ������ - �� ����� �����������</param>
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
		/// ��� ���� �������� ������������
		/// </summary>
		/// <param name="psnConfig">������������ ���</param>
		/// <param name="progressAction">�������� ��� ��������� ���������� ������������ �������, ���� ������ ������ - �� ����� �����������</param>
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