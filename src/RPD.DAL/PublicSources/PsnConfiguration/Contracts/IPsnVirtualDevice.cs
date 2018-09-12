using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// Виртуальное устройство ПСН
	/// </summary>
	public interface IPsnVirtualDevice {
		/// <summary>
		/// Название устройства
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Параметры устройства (тоже виртуальные)
		/// </summary>
		IEnumerable<IPsnVirtualParameter> Parameters { get; }
	}
}