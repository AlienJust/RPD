namespace RPD.Presentation.Contracts.ViewModels
{
    public interface IProgressViewModel
    {
        string MessageText { get; set; }
        bool ProgressVisible { get; set; }
        int Progress { get; }
    }
}
