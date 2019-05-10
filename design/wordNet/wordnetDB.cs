namespace wordNetDB {
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity;

  public class Context : DbContext {
    public Context()
        : base("name=wordnetDB") {
    }

    public static Context getContext(bool recreate = false) {
      var ctx = new Context();
      if (!ctx.Database.Exists())
        ctx.Database.Create();
      else if (recreate) {
        ctx.Database.Delete();
        ctx.Database.Create();
      }
      return ctx;
    }


    public virtual DbSet<LexicalEntry> LexicalEntries { get; set; }
    public virtual DbSet<Sense> Senses { get; set; }
    public virtual DbSet<SynsetRelation> SynsetRelations { get; set; }
    public virtual DbSet<Synset> Synsets { get; set; }
    public virtual DbSet<Statement> Statements { get; set; }
    public virtual DbSet<Translation> Translations { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {

      modelBuilder.Entity<Sense>()
        .HasKey(bc => new { bc.LexicalEntryId, bc.SynsetId });

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.Senses)
               .WithRequired(c => c.Synset)
               .HasForeignKey(s => s.SynsetId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<LexicalEntry>()
               .HasMany(s => s.Senses)
               .WithRequired(c => c.LexicalEntry)
               .HasForeignKey(s => s.LexicalEntryId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<Translation>()
        .HasKey(bc => new { bc.SynsetFromId, bc.SynsetToId });

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.TranslationTargets)
               .WithRequired(c => c.SynsetTo)
               .HasForeignKey(s => s.SynsetToId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.TranslationSources)
               .WithRequired(c => c.SynsetFrom)
               .HasForeignKey(s => s.SynsetFromId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<SynsetRelation>()
        .HasKey(bc => new { bc.SynsetFromId, bc.SynsetToId });

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.RelationTargets)
               .WithRequired(c => c.SynsetTo)
               .HasForeignKey(s => s.SynsetToId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.RelationSources)
               .WithRequired(c => c.SynsetFrom)
               .HasForeignKey(s => s.SynsetFromId)
               .WillCascadeOnDelete(false);

    }

  }

  public class LexicalEntry {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string PartOfSpeech { get; set; }
    public string Lemma { get; set; }
    public virtual ICollection<Sense> Senses { get; set; }
  }

  public class Synset {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Gloss { get; set; }
    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Statement> Statements { get; set; }
    public virtual ICollection<SynsetRelation> RelationSources { get; set; }
    public virtual ICollection<SynsetRelation> RelationTargets { get; set; }
    public virtual ICollection<Translation> TranslationSources { get; set; }
    public virtual ICollection<Translation> TranslationTargets { get; set; }
  }

  public class Statement {
    public int Id { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
    public string Example { get; set; }
  }

  public class Sense {
    public int LexicalEntryId { get; set; }
    public LexicalEntry LexicalEntry { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
  }

  public class SynsetRelation {
    public int SynsetFromId { get; set; }
    public Synset SynsetFrom { get; set; }
    public int SynsetToId { get; set; }
    public Synset SynsetTo { get; set; }
    public string RelType { get; set; }
  }

  public class Translation {
    public int SynsetFromId { get; set; }
    public Synset SynsetFrom { get; set; }
    public int SynsetToId { get; set; }
    public Synset SynsetTo { get; set; }
    public string Language { get; set; }
  }

}