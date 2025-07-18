using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Events
{
    public class ProductCreatedEvent : BaseEvent
    {
        public Product Product { get; }
        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }
} 