using System;
using System.Linq;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using NLog;

namespace RPD.Presentation.Extensions {
	public static class RepositoryViewModelExtension {
		private static Logger _logger = LogManager.GetCurrentClassLogger();

		public static ITrendViewModel FindTrend(this IRepositoryViewModel repository, Guid uid) {
			foreach (var loco in repository.Locomotives) {
				foreach (var sec in loco.Sections) {
					foreach (var psnLogViewModel in sec.PsnLogs) {
						foreach (var psnMeterViewModel in psnLogViewModel.PsnMeters) {
							var result = psnMeterViewModel.PsnChannels.FirstOrDefault(e => e.Uid == uid);
							if (result != null)
								return result;

						}
					}

					foreach (var psnLogViewModel in sec.PsnPowerOnLogs) {
						foreach (var psnMeterViewModel in psnLogViewModel.PsnMeters) {
							var result = psnMeterViewModel.PsnChannels.FirstOrDefault(e => e.Uid == uid);
							if (result != null)
								return result;
						}
					}

					foreach (var fault in sec.Faults) {
						foreach (var meter in fault.RpdMeters) {
							var result = meter.Channels.FirstOrDefault(e => e.Uid == uid);
							if (result != null)
								return result;
						}
					}
				}
			}

			return null;
		}
	}
}