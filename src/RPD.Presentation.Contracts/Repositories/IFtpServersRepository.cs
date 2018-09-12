using System.Collections.Generic;
using RPD.Presentation.Contracts.Model;

namespace RPD.Presentation.Contracts.Repositories
{
    /// <summary>
    /// Доступные сервера FTP.
    /// </summary>
    public interface IFtpServersRepository
    {
        IList<IFtpServer> ListAll();
        void Add(IFtpServer ftpServer);
        void Update(IFtpServer ftpServer);
        void Delete(int id);
    }
}