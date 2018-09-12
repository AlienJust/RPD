using System.Collections.ObjectModel;

namespace RPD.Presentation.Contracts.Model.SelectionMasks
{
    public interface ISelectionMasksCollection
    {
        /// <summary>
        /// Название коллекции.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Коллекция шаблонов.
        /// </summary>
        ObservableCollection<ISelectionMask> Items { get; set; }

        bool Contains(ISelectionMask mask);
    }
}