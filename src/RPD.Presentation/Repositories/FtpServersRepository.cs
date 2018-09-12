using System.Collections.Generic;
using System.Linq;
using RPD.Presentation.Contracts.Model;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Model;

namespace RPD.Presentation.Repositories {
	internal class FtpServersRepository : IFtpServersRepository {
		readonly IList<FtpServer> _servers = new List<FtpServer>();

		private int _lastId;

		public FtpServersRepository() {
			_servers.Add(new FtpServer {
				Id = 0,
				Name = "Горизонт локальный",
				Host = "10.10.10.10",
				Port = 21,
				User = "1",
				Password = "1"
			});


			_servers.Add(new FtpServer() {
				Id = 0,
				Name = "Горизонт",
				Host = "212.220.187.50",
				Port = 21,
				User = "1",
				Password = "1"
			});

		}

		public IList<IFtpServer> ListAll() {
			return new List<IFtpServer>(_servers);
		}

		public void Add(IFtpServer ftpServer) {
			var copy = new FtpServer();
			CopyEntity(copy, ftpServer);
			copy.Id = _lastId++;

			_servers.Add(copy);
		}

		public void Update(IFtpServer ftpServer) {
			var val = _servers.First(x => x.Id == ftpServer.Id);

			CopyEntity(val, ftpServer);
		}

		public void Delete(int id) {
			_servers.Remove(_servers.First(x => x.Id == id));
		}

		private void CopyEntity(IFtpServer source, IFtpServer dest) {
			dest.Host = source.Host;
			dest.Name = source.Name;
			dest.Password = source.Password;
			dest.Port = source.Port;
			dest.User = source.User;
		}
	}
}
