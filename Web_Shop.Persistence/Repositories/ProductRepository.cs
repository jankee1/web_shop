using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Shop.Persistence.Repositories.Interfaces;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WwsishopContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product?> GetOneByIdWithRelationAsync(ulong id, params Expression<Func<Product, object>>[] includes)
        {
            IQueryable<Product> query = Entities;

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return await query.FirstOrDefaultAsync(p => p.IdProduct == id);
        }
    }
}
