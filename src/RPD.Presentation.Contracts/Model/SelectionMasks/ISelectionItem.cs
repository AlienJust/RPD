namespace RPD.Presentation.Contracts.Model.SelectionMasks
{
    public interface ISelectionItem
    {
        /// <summary>
        /// Номер эелемента в списке.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Название элемента.
        /// </summary>
        string Name { get; set; }
    }
}