namespace ECommerceApi.Models
{
    public interface IObserver
    {
        void Update(string email, string message);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string email, string message);
    }
} 