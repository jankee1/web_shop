using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetOneByIdWithRelationAsync(ulong id, params Expression<Func<Product, object>>[] includes);
    }
}
