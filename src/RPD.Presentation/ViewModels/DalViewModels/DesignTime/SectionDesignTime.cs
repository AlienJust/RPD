using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime
{
    class SectionDesignTime : ViewModelBase, ISectionViewModel
    {
        private readonly ILocomotiveViewModel _ownerLocomotive;

        public bool IsTreeViewExpanded { get; set; }

        public SectionDesignTime(ILocomotiveViewModel ownerLocomotive)
        {
            _ownerLocomotive = ownerLocomotive;
            Name = "Секция 16";

            Faults = new ObservableCollection<IFaultViewModel>();

            /*
            Faults.Add(new FaultDesignTime());
            Faults.Add(new FaultDesignTime());
            Faults.Add(new FaultDesignTime());
            */
            IsTreeViewExpanded = true;
            PsnLogs = new ObservableCollection<IPsnLogViewModel>();
            PsnPowerOnLogs = new ObservableCollection<IPsnLogViewModel>();
        }

        #region Implementation of ISectionViewModel

        public ISection Section
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool AllFaultsNew { set; private get; }
        public bool CaptureNewFaults { get; set; }
        public string Name { get; private set; }

        public ILocomotiveViewModel OwnerLocomotive
        {
            get { return _ownerLocomotive; }
        }

        public ObservableCollection<IFaultViewModel> Faults { get; set; }
        public ObservableCollection<IPsnLogViewModel> PsnLogs { get; set; }
        public ObservableCollection<IPsnLogViewModel> PsnPowerOnLogs { get; set; }

        #endregion
    }
}
