using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop.Application.DTOs.Category
{
    public class GetSingleCategoryDTO
    {
        public uint IdCategory { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}