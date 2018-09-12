using System;

namespace RPD.DAL {
	/// <summary>
	/// Конфигурация канала ПСН
	/// </summary>
	public interface IPsnChannelConfig {
		/// <summary>
		/// Уникальный идентификатор канала
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// Название канала
		/// </summary>
		String Name { get; }
	}
}