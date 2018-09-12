using System.Collections.ObjectModel;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface ILogIntegrityViewModel
    {
        string WindowTitle { get; }
        bool IsLoading { get; set; }
        ObservableCollection<string> Text { get; }
    }
}