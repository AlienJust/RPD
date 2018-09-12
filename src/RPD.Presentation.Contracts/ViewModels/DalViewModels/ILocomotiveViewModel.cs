using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface ILocomotiveViewModel : ITreeViewItemViewModel
    {
        ILocomotive Locomotive { get; set; }

        /// <summary>
        /// Если true, то у всех аварий на этом локомотиве будет высталвен флаг IsNew.
        /// </summary>
        bool AllFaultsNew { set; }

        /// <summary>
        /// Если true, то у всех аварий, которые добаляются в 
        /// списки аварий на секциях будет выставлен флаг IsNew.
        /// </summary>
        bool CaptureNewFaults { get; set; }

        /// <summary>
        /// Название локомотива.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Список секций.
        /// </summary>
        ObservableCollection<ISectionViewModel> Sections { get; set; }
    }
}