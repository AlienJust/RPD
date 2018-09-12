namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	public interface IRpdDumpRule {
		/// <summary>
		/// Маска условий контроля и регистрации
		/// </summary>
		int Type { get; set; }

		/// <summary>
		/// Параметр выполнения условий контроля (пока один, 400)
		/// </summary>
		int ControlValue { get; set; }

		/// <summary>
		/// Верхний предел
		/// </summary>
		int MaxValue { get; set; }

		/// <summary>
		/// Нижний предел
		/// </summary>
		int MinValue { get; set; }
	}
}
