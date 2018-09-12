using RPD.Presentation.Contracts.Model;

namespace RPD.Presentation.Model {
	internal class FtpServer : IFtpServer {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
	}
}
