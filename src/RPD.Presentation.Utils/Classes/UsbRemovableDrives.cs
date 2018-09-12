using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace RPD.Presentation.Utils.Classes
{
    public interface IUsbRemovableDrives
    {
        /// <summary>
        /// Списко подключаемых накопителей.
        /// </summary>
        ObservableCollection<DriveInfo> Items { get; }
    }

    /// <summary>
    /// Список подключаемых накопителей.
    /// </summary>
    public class UsbRemovableDrives : IUsbRemovableDrives
    {
        readonly Timer _timer = new Timer();
        private readonly ObservableCollection<DriveInfo> _items = new ObservableCollection<DriveInfo>();

        public UsbRemovableDrives()
        {
            FillItems(GetRemovableDrives());

            InitializeTimer();
        }

        /// <summary>
        /// Списко подключаемых накопителей.
        /// </summary>
        
        public ObservableCollection<DriveInfo> Items
        {
            get
            {
                return _items;
            }
        }

        private void InitializeTimer()
        {
            _timer.Interval = 3000;
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void FillItems(IEnumerable<DriveInfo> drives)
        {
            Items.Clear();
            foreach (var drive in drives)
                Items.Add(drive);            
        }


        /// <summary>
        /// Тик таймера.
        /// </summary>
        private void TimerTick(object sender, System.EventArgs e)
        {
            CheckIfDrivesListsDiffers(GetRemovableDrives());
        }

        private void CheckIfDrivesListsDiffers(List<DriveInfo> drives)
        {
            if (drives.Count != Items.Count)
                FillItems(drives);
            else
            {
                for (int i = 0; i < drives.Count; i++)
                {
                    if (drives[i].Name != Items[i].Name)
                    {
                        FillItems(drives);
                        return;
                    }
                }
            }
        }

        private static List<DriveInfo> GetRemovableDrives()
        {
            var drives = new List<DriveInfo>(DriveInfo.GetDrives());

            drives.RemoveAll((drive) => drive.DriveType != DriveType.Removable || drive.Name.ToLower().Contains("a:"));

            return drives;
        }
    }
}
