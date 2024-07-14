using System.ComponentModel.DataAnnotations;

namespace ProductsGuradution.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(250)]

        public string ProductName { get; set; }

        public int Price { get; set; }

        public int OldPrice { get; set; }

        public string Category { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }

        public byte[] CatImg { get; set; }

       

        
    }
}
