using GalaSoft.MvvmLight.Command;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface ICopyProgressViewModel
    {
        /// <summary>
        /// Прогресс копирования.
        /// </summary>
        int Progress { get; set; }

        int Minimum { get; }
        int Maximum { get; }

        string WindowTitle { get; set; }

        string Header { get;set; }

        string Status { get; set; }

        /// <summary>
        /// Описание количества аварий, которые копируются
        /// </summary>
        string FaultsString { get; set; }

        string PsnLogString { get; set; }

        RelayCommand Close { get; set; }
    }
}