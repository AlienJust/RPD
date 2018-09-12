using System.Collections.ObjectModel;

namespace RPD.DAL {
	/// <summary>
	/// Представляет собой локомотив
	/// </summary>
	public interface ILocomotive {
		/// <summary>
		/// Список секций
		/// </summary>
		ObservableCollection<ISection> Sections { get; }

		/// <summary>
		/// Название локомотива
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Устанавливает название локомотива
		/// </summary>
		/// <param name="name"></param>
		void SetName(string name);
	}
}
