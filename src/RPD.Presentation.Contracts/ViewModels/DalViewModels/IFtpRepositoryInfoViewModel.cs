using System.ComponentModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IFtpRepositoryInfoViewModel
    {
        IFtpRepositoryInfo Model { get; }

        string DeviceNumber { get; }
        bool IsNameSet { get; set; }
        string LocomotiveName { get; set; }
        string SectionName { get; set; }
        string LocomotiveAndSectionString { get; }
    }
}