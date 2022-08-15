using System.Collections.Generic;

namespace MyDi.DI.Interfaces
{
    public interface IDIContainer
    {
        T GetService<T>(string name = null);
        IEnumerable<T> GetAll<T>(string name = null);
    }
}
