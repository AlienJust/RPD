using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IRepositoryViewModel
    {
        IRepository Repository { get; set; }

        /// <summary>
        /// ���� true, �� � ���� ������, ������� ���������� � 
        /// ������ ������ �� ����������� ����� ��������� ���� IsNew.
        /// </summary>
        bool CaptureNewFaults { get; set; }

        string Header { get; set; }
        ObservableCollection<ILocomotiveViewModel> Locomotives { get; set; }
    }
}