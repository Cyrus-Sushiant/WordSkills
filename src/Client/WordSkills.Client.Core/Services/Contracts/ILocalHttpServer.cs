namespace WordSkills.Client.Core.Services.Contracts;

public interface ILocalHttpServer
{
    Task<int> Start();

    int Port { get; }
}
