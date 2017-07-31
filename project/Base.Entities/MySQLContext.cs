namespace Base.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
                .Property(e => e.proprietario)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .HasMany(e => e.site_nserver)
                .WithRequired(e => e.site)
                .HasForeignKey(e => e.id_site)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<site_nserver>()
                .Property(e => e.nserver)
                .IsUnicode(false);
        }
    }
}
