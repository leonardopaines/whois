namespace Base.Data.Model.Entities
{
    using MySql.Data.Entity;
    using System.Data.Entity;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class MySQLContext : DbContext
    {
        public MySQLContext()
            : base("name=MySQLContext")
        {
        }

        public virtual DbSet<pesquisa> pesquisa { get; set; }
        public virtual DbSet<site> site { get; set; }
        public virtual DbSet<site_nserver> site_nserver { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<pesquisa>()
                .Property(e => e.conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<pesquisa>()
                .Property(e => e.iprequisicao)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.dominio)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.hospedagem)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.titular)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.responsavel)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.whois)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .HasMany(e => e.pesquisa)
                .WithOptional(e => e.site)
                .HasForeignKey(e => e.idsite);

            modelBuilder.Entity<site>()
                .HasMany(e => e.site_nserver)
                .WithRequired(e => e.site)
                .HasForeignKey(e => e.idsite)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<site_nserver>()
                .Property(e => e.dns)
                .IsUnicode(false);
        }
    }
}
