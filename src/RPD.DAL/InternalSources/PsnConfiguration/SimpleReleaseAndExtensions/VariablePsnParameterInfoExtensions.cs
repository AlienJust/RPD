namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	/// <summary>
	/// Вспомогательный класс расширений для ПСН параметра
	/// </summary>
	public static class VariablePsnParameterInfoExtensions
	{
		/// <summary>
		/// Сравнивает 2 параметра ПСН между собой
		/// </summary>
		/// <param name="info1">Первый параметр для сравнения</param>
		/// <param name="info2">Второй параметр для сравнения</param>
		/// <returns>Истина, если параметры одинаковы</returns>
		public static bool IsEqualTo(this IVariablePsnParameterInfo info1, IVariablePsnParameterInfo info2)
		{
			if (!((IPsnParameterConfiguration)info1).IsEqualTo(info2)) return false;
			return true;
		}
	}
}