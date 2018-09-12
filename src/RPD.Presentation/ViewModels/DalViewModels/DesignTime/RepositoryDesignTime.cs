using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime
{
    class RepositoryDesignTime : ViewModelBase, IRepositoryViewModel
    {
        public RepositoryDesignTime()
        {
            Locomotives = new ObservableCollection<ILocomotiveViewModel>();
        }

        #region Implementation of IRepositoryViewModel

        public IRepository Repository { get; set; }
        public bool CaptureNewFaults { get; set; }
        public string Header { get; set; }
        public ObservableCollection<ILocomotiveViewModel> Locomotives { get; set; }

        #endregion
    }
}
