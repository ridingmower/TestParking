using Kztek.Model.Models.Event;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Kztek.Data.AccessEvent
{
    public class KztekAccessEventEntities : DbContext
    {
        public KztekAccessEventEntities()
            : base("KztekAccessEventEntities")
        {
            Database.SetInitializer<KztekAccessEventEntities>(null);
            this.Database.CommandTimeout = 180;
        }

        public KztekAccessEventEntities(string conn) : base(conn)
        {
            Database.SetInitializer<KztekAccessEventEntities>(null);
        }

        //Hệ thống

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }
}
