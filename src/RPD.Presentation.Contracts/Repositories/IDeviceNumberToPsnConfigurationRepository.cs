using System.Collections.Generic;

namespace RPD.Presentation.Contracts.Repositories
{
    /// <summary>
    /// Хранит пары соответствий (номер устройства) - (конфигурация ПСН).
    /// </summary>
    public interface IDeviceNumberToPsnConfigurationRepository
    {
        IDictionary<int, string> Configurtions { get; }

        void Save();
    }
}