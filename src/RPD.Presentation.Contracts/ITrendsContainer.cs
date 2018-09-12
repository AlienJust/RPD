using System.Collections.Generic;
using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.Contracts
{
    public interface ITrendsContainer
    {
        string Name { get; }
        IEnumerable<ITrendViewModel> Trends { get; }
    }
}