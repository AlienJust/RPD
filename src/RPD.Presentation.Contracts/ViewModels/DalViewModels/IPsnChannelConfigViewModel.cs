using System;
using System.Windows.Media;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IPsnChannelConfigViewModel
    {
        Guid Id { get; }
        string Name { get; }

        Color Color { get; set; } 
    }
}