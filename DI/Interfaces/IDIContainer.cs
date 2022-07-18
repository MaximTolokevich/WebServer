namespace WebServer.DI.Interfaces
{
    public interface IDIContainer
    {
        T GetService<T>(string name);
    }
}
