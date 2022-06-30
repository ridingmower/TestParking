namespace Kztek.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private KztekEntities dataContext;

        public DatabaseFactory()
        {
        }

        public KztekEntities Get()
        {
            return dataContext ?? (dataContext = new KztekEntities());
        }
    
        public DatabaseFactory(string connectionString)
        {
            if (connectionString == "")
                dataContext = new KztekEntities();
            else
                dataContext = new KztekEntities(connectionString);
        }


        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}