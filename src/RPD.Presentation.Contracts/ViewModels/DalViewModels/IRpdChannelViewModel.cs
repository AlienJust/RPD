namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IRpdChannelViewModel : ITrendViewModel
    {
        IRpdMeterViewModel Meter { get; set; }
    }
}