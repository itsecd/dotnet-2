using System.Collections.Generic;

namespace Server.Repositories
{
    public interface IJSONRepository<T>
    {
        public List<T> entyties { get; }
        public T Get(int id);
        public IEnumerable<T> Get();
        public void Add(T element);
        public void Update(int id, T element);
        public void Delete(int id);
        public void DeleteAll();
        public void Save();
        public void Load();
    }
}
