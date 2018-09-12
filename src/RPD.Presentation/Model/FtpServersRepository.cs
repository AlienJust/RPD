using System;
using System.Collections.Generic;

namespace RPD.Presentation.Mocks
{
    internal interface IFtpServersRepository
    {
        IList<FtpServer> ListAll();
        void Add(FtpServer ftpServer);
        void Update(FtpServer ftpServer);
        void Delete(int id);
    }

    internal class FtpServersRepository: IFtpServersRepository
    {
        private IList<FtpServer> _servers = new List<FtpServer>();

        public FtpServersRepository()
        {
            _servers.Add(new FtpServer()
                             {
                                 Id = 1,
                                 Host = "212.220.187.50",
                                 Port = 21,
                                 Name = "Горизонт",
                                 User = "admin",
                                 Password = "eSCADA"
                             });
        }

        public IList<FtpServer> ListAll()
        {
            return _servers;
        }

        public void Add(FtpServer ftpServer)
        {
            _servers.Add(ftpServer);
        }

        public void Update(FtpServer ftpServer)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
