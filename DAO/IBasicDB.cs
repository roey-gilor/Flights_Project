using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IBasicDB<T> where T : IPoco
    {
        public T Get(long id);
        public IList<T> GetAll();
        public long Add(T t);
        public void Remove(T t);
        public void Update(T t);
    }
}
