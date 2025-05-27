using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.DTOs.Product
{
    public class AddUpdateProductDto : AbstractProductDto
    {
        public virtual ICollection<string> IdCategories { get; set; } = new List<string>();
    }
}
