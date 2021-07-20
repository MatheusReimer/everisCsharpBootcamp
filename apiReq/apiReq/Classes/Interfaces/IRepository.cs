using System.Collections.Generic;

namespace projectDIO.Interfaces
{
    public interface IRepository<T>
    {   

        List<T> List();

        T ReturnById(int id);
        
        void Insert(T entity);

        void Exclude(int id);

        void Update(int id,T entity);

        int NextId();
    }    
}