namespace CleanArchitecture.Application.Products.Common
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int Inventory { get; set; }
        public int CategoryId { get; set; }
    }
} 