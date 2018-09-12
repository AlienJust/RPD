using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using RPD.DAL;
using RPD.EventArgs;

namespace RPD.Presentation.Mocks.DAL {
	class RepositoryMock : IRepository {
		public RepositoryMock() {
			Locomotives = new ObservableCollection<ILocomotive>();
		}

		public void Open(Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange) {
			onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, ""));
		}

		public void Remove(ISection section, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void Remove(IEnumerable<IPsnLog> psnLogs, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void SaveDataAsync(IRepository sourceRepo, IEnumerable<IPsnLog> psnLogs, IEnumerable<IFaultLog> rpdLogs, Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange) {
			var bg = new BackgroundWorker();
			bg.DoWork += BgSaveData;
			bg.ProgressChanged += (sender, args) => onProgressChange(new OnProgressChangeEventArgs(args.ProgressPercentage));
			bg.RunWorkerCompleted += (sender, args) => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, ""));
			bg.WorkerReportsProgress = true;

			bg.RunWorkerAsync();
		}

		private void BgSaveData(object sender, DoWorkEventArgs doWorkEventArgs) {
			var bg = (BackgroundWorker)sender;

			for (int i = 0; i < 10; i++) {
				Thread.Sleep(500);
				bg.ReportProgress(10 * (i + 1));
			}
		}

		public bool IsExist(IFaultLog rpdLog) {
			return false;
		}

		public bool IsExist(IPsnLog psnLog) {
			return false;
		}

		public ObservableCollection<ILocomotive> Locomotives { get; }
	}
}
