using System.Collections.ObjectModel;

namespace RPD.Presentation.Contracts.Model.SelectionMasks
{
    public interface ISelectionMask
    {
        /// <summary>
        /// Название.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Список элементов.
        /// </summary>
        ObservableCollection<ISelectionItem> Items { get; set; }

        bool ContainsItemNumber(int number);
    }
}