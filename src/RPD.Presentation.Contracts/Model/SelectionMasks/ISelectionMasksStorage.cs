using System.Collections.Generic;

namespace RPD.Presentation.Contracts.Model.SelectionMasks
{
    public interface ISelectionMasksStorage
    {
        /// <summary>
        /// Таблица коллекция шаблонов выделений.
        /// string - название коллекции.
        /// </summary>
        Dictionary<string, ISelectionMasksCollection> SelectionMasks { get; set; }

        void Load(string fileName);
        void Save(string fileName);
    }
}