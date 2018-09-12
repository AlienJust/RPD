namespace RPD.DAL {
	/// <summary>
	/// Тип измерителя
	/// </summary>
	public enum RpdMeterType
	{
		/// <summary>
		/// Тип измерителя не определен
		/// </summary>
		Undefined = 0xFF,

		/// <summary>
		/// УРАН
		/// </summary>
		Uran = 0,

		/// <summary>
		/// ИРВИ
		/// </summary>
		Irvi = 1,

		/// <summary>
		/// Радар
		/// </summary>
		Radar = 2
	}
}