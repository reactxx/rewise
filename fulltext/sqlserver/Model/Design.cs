namespace fulltext.sqlserver
{
  using System;
  using System.Data.Entity;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

  public partial class Design : DbContext
  {
    public Design()
        : base("name=DesignConnectionString")
    {
    }

    public virtual DbSet<Source> Sources { get; set; }
    public virtual DbSet<Stemm> Stemms { get; set; }
    public virtual DbSet<WordInStemm> WordInStemms { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Source>()
          .HasMany(e => e.WordInStemms)
          .WithRequired(e => e.Source)
          .HasForeignKey(e => e.WordId)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Stemm>()
          .HasMany(e => e.WordInStemms)
          .WithRequired(e => e.Stemm)
          .WillCascadeOnDelete(false);
    }
  }
}
