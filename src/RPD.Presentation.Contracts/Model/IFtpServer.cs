namespace RPD.Presentation.Contracts.Model
{
    public interface IFtpServer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string User { get; set; }
        string Password { get; set; }
    }
}