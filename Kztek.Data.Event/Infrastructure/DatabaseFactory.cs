namespace Kztek.Data.Event.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private KztekEventEntities dataContext;

        public DatabaseFactory()
        {
        }

        public KztekEventEntities Get()
        {
            return dataContext ?? (dataContext = new KztekEventEntities());
        }
    
        public DatabaseFactory(string connectionString)
        {
            if (connectionString == "")
                dataContext = new KztekEventEntities();
            else
                dataContext = new KztekEventEntities(connectionString);
        }


        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}