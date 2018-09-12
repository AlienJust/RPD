namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Вспомогательный класс расширений для ПСН параметра
	/// </summary>
	public static class VariablePsnParameterInfoExtensions {
		/// <summary>
		/// Сравнивает 2 параметра ПСН между собой
		/// </summary>
		/// <param name="info1">Первый параметр для сравнения</param>
		/// <param name="info2">Второй параметр для сравнения</param>
		/// <returns>Истина, если параметры одинаковы</returns>
		public static bool IsEqualTo(this IPsnProtocolParameterConfigurationVariable info1, IPsnProtocolParameterConfigurationVariable info2)
		{
			if (!((IPsnProtocolParameterConfiguration)info1).IsEqualTo(info2)) return false;
			return true;
		}
	}
}