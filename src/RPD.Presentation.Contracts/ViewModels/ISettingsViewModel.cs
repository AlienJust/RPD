using GalaSoft.MvvmLight.Command;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface ISettingsViewModel
    {
        /// <summary>
        /// Путь к хранилищу
        /// </summary>
        string RepositoryPath { get; set; }

        /// <summary>
        /// Установлена ли директория хранилища.
        /// </summary>
        bool IsRepositoryPathSetted { get; set; }

        /// <summary>
        /// Текст строки статуса.
        /// </summary>
        string StatusString { get; set; }

        /// <summary>
        /// Видимость строки статуса.
        /// </summary>
        bool IsStatusStringVisible { get; }

        RelayCommand BrowseRepositoryPath { get; set; }
        RelayCommand OK { get; set; }
        RelayCommand Cancel { get; set; }

        bool CanClose();
    }
}