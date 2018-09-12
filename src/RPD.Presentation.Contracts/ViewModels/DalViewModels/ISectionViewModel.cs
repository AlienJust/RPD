using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface ISectionViewModel : ITreeViewItemViewModel
    {
        ISection Section { get; }

        /// <summary>
        /// Если true, то у всех аварий на секции будет выставлен флаг IsNew.
        /// </summary>
        bool AllFaultsNew { set; }

        /// <summary>
        /// Если true, то у всех аварий, которые добаляются в 
        /// список аварий будет выставлен флаг IsNew.
        /// </summary>
        bool CaptureNewFaults { get; set; }

        string Name { get; }


        ILocomotiveViewModel OwnerLocomotive { get; }

        /// <summary>
        /// Список аварий на секции.
        /// </summary>
        ObservableCollection<IFaultViewModel> Faults { get; set; }

        /// <summary>
        /// Список логов ПСН (PsnLogType.FixedLength).
        /// </summary>
        ObservableCollection<IPsnLogViewModel> PsnLogs { get; set; }

        /// <summary>
        /// Список логов ПСН по включению (PsnLogType.PowerDepended).
        /// </summary>
        ObservableCollection<IPsnLogViewModel> PsnPowerOnLogs { get; set; }
    }
}