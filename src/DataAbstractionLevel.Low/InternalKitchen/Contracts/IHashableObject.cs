using System.Collections.Generic;
using System.Linq;

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

	/// <summary>
	/// Расширения для работы с IHashableObject
	/// </summary>
	public static class HashableObjectExt {
		/// <summary>
		/// Проверяет равенство по хэшу
		/// </summary>
		/// <param name="source">Объект 1</param>
		/// <param name="secondObject">Объект 2</param>
		/// <returns>Истина при совпадении хэшей</returns>
		public static bool IsEqualByHash(this IHashableObject source, IHashableObject secondObject) {
			if (source.Hash.Count != secondObject.Hash.Count) {
				return false;
			}
			return !source.Hash.Where((t, i) => t != secondObject.Hash[i]).Any();
		}

		/// <summary>
		/// Отображает хэш в привычном виде
		/// </summary>
		/// <param name="bytes">Хэш</param>
		/// <param name="seporator">Разделитель байтов</param>
		/// <returns>Строка-предстваление хэша</returns>
		public static string ToHumanString(this IList<byte> bytes, string seporator) {
			var result = bytes.Aggregate(string.Empty, (current, b) => current + (b.ToString("X2") + seporator));
			
			// remove last seporator and returns:
			return result.Substring(0, result.Length - seporator.Length);
		}
	}
}