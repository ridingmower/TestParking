using Kztek.Model.Models.Event;
using Kztek.Model.Models.LockerEvent;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Kztek.Data.LockerEvent
{
    public class KztekLockerEventEntities : DbContext
    {
        public KztekLockerEventEntities()
            : base("KztekLockerEventEntities")
        {
            Database.SetInitializer<KztekLockerEventEntities>(null);
            this.Database.CommandTimeout = 180;
        }

        public KztekLockerEventEntities(string conn) : base(conn)
        {
            Database.SetInitializer<KztekLockerEventEntities>(null);
        }

        //Hệ thống
        public DbSet<tblLockerEvent> tblLockerEvents { get; set; }

        public DbSet<tblLockerAlarm> tblLockerAlarms { get; set; }

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
