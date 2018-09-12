using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using RPD.DAL;
using RPD.EventArgs;

namespace SampleApp {
	internal class PsnSignalViewModel : ViewModelBase {
		private readonly IPsnChannel _model;
		private readonly DependedCommand _cmdLoadTrend;
		private readonly DependedCommand _cmdUnloadTrend;

		private bool _isTrendLoadInProgress;

		public PsnSignalViewModel(IPsnChannel model) {
			_model = model;
			_isTrendLoadInProgress = false;

			_cmdLoadTrend = new DependedCommand(() =>
			{
				IsTrendLoadInProgress = true;
				
				_model.LoadTrendAsync(ea => {
					MessageSystem.ShowMessage(ea.Message);

					switch (ea.ResultCode) {
						case OnCompleteEventArgs.CompleteResult.Ok:
							MessageSystem.ShowMessage("DataPoints count: " + _model.Trend.Count);
							IsTrendLoadInProgress = false;
							return;
						case OnCompleteEventArgs.CompleteResult.Error:
							IsTrendLoadInProgress = false;
							return;
					}
				});
			}, () => !IsTrendLoadInProgress);
			_cmdLoadTrend.AddDependOnProp(this, "IsTrendLoadInProgress");

			
			_cmdUnloadTrend = new DependedCommand(() => _model.FreeTrend(), () => true);
		}

		public string Name => _model.Name;

		public bool IsTrendLoadInProgress
		{
			get => _isTrendLoadInProgress;
			set
			{
				if (_isTrendLoadInProgress != value)
				{
					_isTrendLoadInProgress = value;
					RaisePropertyChanged(() => IsTrendLoadInProgress);
				}
			}
		}

		public ICommand CmdLoadTrend => _cmdLoadTrend;

		public ICommand CmdUnloadTrend => _cmdUnloadTrend;
	}
}