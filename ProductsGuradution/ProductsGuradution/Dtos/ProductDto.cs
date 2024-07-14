using System.ComponentModel.DataAnnotations;

namespace ProductsGuradution.Dtos
{
    public class ProductDto
    {
        [MaxLength(250)]

        public string ProductName { get; set; }

        public int Price { get; set; }

        public int OldPrice { get; set; }

        public double Rate { get; set; }

        public string Category { get; set; }

        public IFormFile? CatImg { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
