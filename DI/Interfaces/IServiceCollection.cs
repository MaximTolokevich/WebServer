namespace MyDi.DI.Interfaces
{
    public interface IServiceCollection
    {
        void Add(object obj, string name = null);
        void Add<TInterface>(object obj, string name = null);
        void AddTransient<T, V>(string name = null);
        void AddTransient<T>(string name = null);
        void AddTransient<TInterface>(object obj, string name = null);
        void AddSingleton<T, V>(string name = null);
        void AddSingleton<T>(string name = null);
        IDIContainer BuildContainer();
    }
}