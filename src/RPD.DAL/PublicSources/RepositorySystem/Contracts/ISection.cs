using System.Collections.ObjectModel;

namespace RPD.DAL
{
	/// <summary>
	/// Описывает секцию локомотива
	/// </summary>
	public interface ISection
	{
		/// <summary>
		/// Идентификатор связанного устройства
		/// </summary>
		IUid DeviceInformationId { get; }
		
		/// <summary>
		/// Список аварий
		/// </summary>
		ObservableCollection<IFaultLog> Faults { get; }

		/// <summary>
		/// Список ПСНов
		/// </summary>
		ObservableCollection<IPsnLog> Psns { get; }

		/// <summary>
		/// Название секции (в простейшем варианте - № секции)
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Устанавливает название секции
		/// </summary>
		/// <param name="name">новое название</param>
		void SetName(string name);
	}
}
