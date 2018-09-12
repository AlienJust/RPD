using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IRepositoryViewModelFactory
    {
        IRepositoryViewModel Create(IRepository repository);
    }
}