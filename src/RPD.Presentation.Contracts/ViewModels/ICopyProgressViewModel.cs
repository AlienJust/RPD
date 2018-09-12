using GalaSoft.MvvmLight.Command;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface ICopyProgressViewModel
    {
        /// <summary>
        /// �������� �����������.
        /// </summary>
        int Progress { get; set; }

        int Minimum { get; }
        int Maximum { get; }

        string WindowTitle { get; set; }

        string Header { get;set; }

        string Status { get; set; }

        /// <summary>
        /// �������� ���������� ������, ������� ����������
        /// </summary>
        string FaultsString { get; set; }

        string PsnLogString { get; set; }

        RelayCommand Close { get; set; }
    }
}