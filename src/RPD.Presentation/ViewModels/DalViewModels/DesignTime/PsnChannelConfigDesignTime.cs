using System;
using System.Windows.Media;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime
{
    public class PsnChannelConfigDesignTime : IPsnChannelConfigViewModel
    {
        public PsnChannelConfigDesignTime(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; }
        public Color Color { get; set; }
    }
}