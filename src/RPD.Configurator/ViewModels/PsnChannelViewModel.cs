using GalaSoft.MvvmLight;
using RPD.DAL;


namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представление канала ПСН.
    /// </summary>
    public class PsnChannelViewModel : ViewModelBase
    {
        IPsnChannel psnChannel;

        public PsnChannelViewModel(IPsnChannel psnChannel, PsnMeterViewModel parentMeter)
        {
            this.psnChannel = psnChannel;
            ParentMeter = parentMeter;
        }


        #region Presentation Members

        public const string IsFaultSignPropertyName = "IsFaultSign";

        public bool IsFaultSign
        {
            get
            {
                return psnChannel.IsFaultSign;
            }

            set { ; }
        }

        public string Name
        {
            get { return psnChannel.Name; }
        }

        public bool CanBeFaultSign
        {
            get { return psnChannel.CanBeFaultSign; }
        }

        public string MeterName
        {
            get { return ParentMeter.Name; }
        }

        #endregion // Presentation Members

        public PsnMeterViewModel ParentMeter { get; set; }
    }
}