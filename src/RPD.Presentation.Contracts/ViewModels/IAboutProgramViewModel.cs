namespace RPD.Presentation.Contracts.ViewModels
{
    public interface IAboutProgramViewModel
    {
        string InstallerVersion { get; }
        string InstallerDate { get; }

        string PresentationVersion { get; }
        string PresentationDate { get; }

        string DalVersion { get; }
        string DalDate { get; }
    }
}