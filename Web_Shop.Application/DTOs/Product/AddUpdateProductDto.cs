using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.DTOs.Product
{
    public class AddUpdateProductDto : GetSingleProductDTO
    {
        public virtual ICollection<String> IdCategories { get; set; } = new List<String>();
    }
}
