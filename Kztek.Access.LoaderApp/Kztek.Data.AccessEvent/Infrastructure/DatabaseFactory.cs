namespace Kztek.Data.AccessEvent.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private KztekAccessEventEntities dataContext;

        public DatabaseFactory()
        {
        }

        public KztekAccessEventEntities Get()
        {
            return dataContext ?? (dataContext = new KztekAccessEventEntities());
        }
    
        public DatabaseFactory(string connectionString)
        {
            if (connectionString == "")
                dataContext = new KztekAccessEventEntities();
            else
                dataContext = new KztekAccessEventEntities(connectionString);
        }


        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}