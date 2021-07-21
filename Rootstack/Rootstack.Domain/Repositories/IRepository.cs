using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        bool Add(TEntity obj);

        bool AddMasive(List<TEntity> lstObj);
        TEntity GetById(long id, int range);

        Task<List<TEntity>> GetAll();

        bool Update(TEntity obj);

        bool Remove(TEntity obj);

        int SaveChanges();
    }
}
