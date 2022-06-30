namespace Kztek.Data.Event.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}