using System;
using Abt.Controls.SciChart;
using GalaSoft.MvvmLight;

namespace RPD.SciChartControl.ViewModel
{
    internal class ChartBookmarkViewModel: ViewModelBase
    {
        private string _name;
        private bool _isCurrent;


        public DateRange XVisibleRange { get; set; }

        public bool IsCurrent 
        {
            get { return _isCurrent; }
            set { Set(() => IsCurrent, ref _isCurrent, value); }
        }

        public DateTime Center 
        {
            get
            {
                var diff = (XVisibleRange.Max - XVisibleRange.Min).Ticks / 2;
                return XVisibleRange.Min + TimeSpan.FromTicks(diff);
            } 
        }

        public string Name
        {
            get { return _name; }
            set { Set(()=>Name, ref _name, value); }
        }

        public string TimeString 
        {
            get { return Center.ToString("HH:mm:ss.ff"); }
        }

        public string Title {
            get { return Name + " (" + TimeString + ")"; }
        }
    }
}