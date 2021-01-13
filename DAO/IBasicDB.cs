using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    interface IBasicDB<T> where T : IPoco
    {
        public T Get(int id);
        public IList<T> GetAll();
        public void Add(T t);
        public void Remove(T t);
        public void Update(T t);
    }
}
