namespace Kztek.Data.AccessEvent.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}