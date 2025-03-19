using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Models;

namespace CourseManagement.Interface
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        bool AddItem(TEntity entity);
        bool UpdateItem(TEntity entity);
        bool RemoveItem(int id);

    }
}