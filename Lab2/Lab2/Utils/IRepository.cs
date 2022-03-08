using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab2.Utils
{
    internal interface IRepository<BaseEntity>
    {
        void Get();
        void Update(BaseEntity entity);
        void Remove(int id);
        void Insert(BaseEntity entity);
    }
}
