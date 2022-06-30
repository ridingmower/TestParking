namespace Kztek.Data.LockerEvent.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private KztekLockerEventEntities dataContext;

        public DatabaseFactory()
        {
        }

        public KztekLockerEventEntities Get()
        {
            return dataContext ?? (dataContext = new KztekLockerEventEntities());
        }
    
        public DatabaseFactory(string connectionString)
        {
            if (connectionString == "")
                dataContext = new KztekLockerEventEntities();
            else
                dataContext = new KztekLockerEventEntities(connectionString);
        }


        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}