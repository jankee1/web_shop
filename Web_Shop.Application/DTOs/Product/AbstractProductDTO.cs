using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop.Application.DTOs.Product
{
    public abstract class AbstractProductDto
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; } = 0;

        public string Sku { get; set; } = null!;
    }
}
