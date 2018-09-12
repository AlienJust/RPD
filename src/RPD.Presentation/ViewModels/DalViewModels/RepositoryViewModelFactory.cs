using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	internal sealed class RepositoryViewModelFactory : IRepositoryViewModelFactory {
		public IRepositoryViewModel Create(IRepository repository) {
			return new RepositoryViewModel(repository);
		}
	}
}
