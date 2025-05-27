using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs.Category;

namespace Web_Shop.Application.DTOs.Product
{
    public class GetSingleProductDTO : AbstractProductDto
    {
        public string ProductId { get; set; } = null!;
        public virtual ICollection<GetSingleCategoryDTO>? Category { get; set; } = new List<GetSingleCategoryDTO>();
    }
}
