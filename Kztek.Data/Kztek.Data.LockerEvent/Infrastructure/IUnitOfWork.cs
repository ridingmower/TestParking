namespace Kztek.Data.LockerEvent.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}