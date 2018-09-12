using System.Collections.Generic;

namespace RPD.Presentation.Contracts.Repositories
{
    /// <summary>
    /// ������ ���� ������������ (����� ����������) - (������������ ���).
    /// </summary>
    public interface IDeviceNumberToPsnConfigurationRepository
    {
        IDictionary<int, string> Configurtions { get; }

        void Save();
    }
}