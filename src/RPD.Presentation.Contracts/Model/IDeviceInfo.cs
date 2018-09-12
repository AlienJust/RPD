namespace RPD.Presentation.Contracts.Model
{
    public interface IDeviceInfo
    {
        /// <summary>
        /// Номер устройства.
        /// </summary>
        int DeviceNumber { get; set; }

        /// <summary>
        /// Номер секции.
        /// </summary>
        int SectionNumber { get; set; }

        /// <summary>
        /// Название локомотива.
        /// </summary>
        string LocomotiveName { get; set; }
    }
}