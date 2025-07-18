using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Events
{
    public class ProductUpdatedEvent : BaseEvent
    {
        public Product Product { get; }
        public ProductUpdatedEvent(Product product)
        {
            Product = product;
        }
    }
} 