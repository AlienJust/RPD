using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.Contracts.ViewModels.AddDataViewModels
{
    /// <summary>
    /// Вы устройства на FTP сервере.
    /// </summary>
    public interface ISelectFtpDeviceViewModel
    {
        bool IsBusy { get; }

        IFtpRepositoryInfoViewModel SelectedItem { get; set; }

        ObservableCollection<IFtpRepositoryInfoViewModel> Items { get; }

        RelayCommand NextCommand { get; }
        RelayCommand BackCommand { get; }
    }
}