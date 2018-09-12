using System;
using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IFaultViewModel : ICheckableViewModel, ILogDataViewModel
    {
        IFaultLog Fault { get; set; }
        DateTime AccuredAt { get; }
        string Name { get; }
        string LogReason { get; }

        ObservableCollection<ISignalViewModel> Signals { get; }
        ObservableCollection<IRpdMeterViewModel> RpdMeters { get; }
        IPsnLogViewModel PsnLog { get; }
    }
}