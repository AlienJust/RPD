using System.Collections.Generic;

namespace DataAbstractionLevel.Low.InternalKitchen.Contracts {
	/// <summary>
	/// Объект с хэшем
	/// </summary>
	public interface IHashableObject {
		/// <summary>
		/// Хэш объекта
		/// </summary>
		IList<byte> Hash { get; }
	}
}