namespace RPD.Presentation.Contracts.ViewModels.DalViewModels {
	public interface ILogDataViewModel {
		/// <summary>
		/// Данные лога уже сохранены в репозиторий
		/// </summary>
		bool IsSavedToRepository { get; set; }

		/// <summary>
		/// Признак того что данные лога новые в списке (только что добавили).
		/// </summary>
		bool IsNew { get; set; }
	}
}