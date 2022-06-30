namespace Kztek.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}