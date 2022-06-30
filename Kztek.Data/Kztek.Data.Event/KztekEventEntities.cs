using Kztek.Model.Models.Event;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Kztek.Data.Event
{
    public class KztekEventEntities : DbContext
    {
        public KztekEventEntities()
            : base("KztekEventEntities")
        {
            Database.SetInitializer<KztekEventEntities>(null);
            this.Database.CommandTimeout = 180;
        }

        public KztekEventEntities(string conn) : base(conn)
        {
            Database.SetInitializer<KztekEventEntities>(null);
        }

        //Hệ thống
        public DbSet<tblAlarm> tblAlarms { get; set; }

        public DbSet<tblCardEventHistory> tblCardEventHistories { get; set; }

        public DbSet<tblCardEvent> tblCardEvents { get; set; }

        public DbSet<tblChangeEvent> tblChangeEvents { get; set; }

        public DbSet<tblDispenser> tblDispensers { get; set; }

        public DbSet<tblLoopEvent> tblLoopEvents { get; set; }

        public DbSet<tblPrintIndex> tblPrintIndexs { get; set; }

        public DbSet<tblVoucher> tblVouchers { get; set; }
        public DbSet<PublicEvent> PublicEvents { get; set; }
        public DbSet<tblCardEventDelete> tblCardEventDeletes { get; set; }
        public DbSet<PayIn> PayIns { get; set; }
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
