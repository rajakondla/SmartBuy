using SmartBuy.SharedKernel;

namespace SmartBuy.Administration.Domain
{
    public class Product : Entity<int>
    { 
        public Product()
        {
            Name = "";
        }

        public string Name { get; set; }
    }
}
