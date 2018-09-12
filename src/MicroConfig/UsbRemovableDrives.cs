using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace MicroConfig {
    /// <summary>
    /// Список подключаемых накопителей.
    /// </summary>
    public class UsbRemovableDrives {
        private readonly Timer _timer = new Timer();

        public UsbRemovableDrives() {
            FillItems(GetRemovableDrives());

            InitializeTimer();
        }

        private void InitializeTimer() {
            _timer.Interval = 10000;
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void FillItems(IEnumerable<DriveInfo> drives) {
            Items.Clear();
            foreach (var drive in drives)
                Items.Add(drive);
        }

        /// <summary>
        /// Списко подключаемых накопителей.
        /// </summary>
        private readonly ObservableCollection<DriveInfo> _items = new ObservableCollection<DriveInfo>();

        public ObservableCollection<DriveInfo> Items {
            get { return _items; }
        }

        /// <summary>
        /// Тик таймера.
        /// </summary>
        private void TimerTick(object sender, System.EventArgs e) {
            CheckIfDrivesListsDiffers(GetRemovableDrives());
        }

        private void CheckIfDrivesListsDiffers(List<DriveInfo> drives) {
            if (drives.Count != Items.Count)
                FillItems(drives);
            else {
                for (int i = 0; i < drives.Count; i++) {
                    if (drives[i].Name != Items[i].Name)
                        FillItems(drives);
                }
            }
        }

        private static List<DriveInfo> GetRemovableDrives() {
            var drives = new List<DriveInfo>(DriveInfo.GetDrives());

            drives.RemoveAll(
                (drive) => drive.DriveType != DriveType.Removable || drive.Name.ToLower().Contains("a:"));

            return drives;
        }
    }
}
