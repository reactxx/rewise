using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace wordNetDB {

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
    public virtual DbSet<Example> Statements { get; set; }
    public virtual DbSet<Translation> Translations { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {

      modelBuilder.Entity<LexicalEntry>()
               .HasMany(s => s.Senses)
               .WithRequired(c => c.LexicalEntry)
               .HasForeignKey(s => s.LexicalEntryId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<Synset>() 
               .HasMany(s => s.Senses)
               .WithRequired(c => c.Synset)
               .HasForeignKey(s => s.SynsetId)
               .WillCascadeOnDelete(false);

      // m:n LexicalEntry <=> Synset
      modelBuilder.Entity<Sense>()
        .HasKey(bc => new { bc.LexicalEntryId, bc.SynsetId });

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
    public string PartOfSpeech { get; set; } // e.g. "v" as verb
    public string Lemma { get; set; } // e.g. finish
    public virtual ICollection<Sense> Senses { get; set; }
  }

  // m:n LexicalEntry <=> Synset 
  public class Sense {
    public int LexicalEntryId { get; set; }
    public LexicalEntry LexicalEntry { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
  }

  // Gloss
  public class Synset {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Gloss { get; set; } // vyklad, e.g. "an unexpected piece of good luck"

    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Example> Examples { get; set; }
    // m:n Synset <=> Synset (with RelType, e.g. ants, hype, hmem, sim, mmem, hprt, hasi, dmnr, dmtc,)
    public virtual ICollection<SynsetRelation> RelationSources { get; set; }
    public virtual ICollection<SynsetRelation> RelationTargets { get; set; }
    // m:n Synset <=> Synset in other language (= translation) with trans LANG
    public virtual ICollection<Translation> TranslationSources { get; set; }
    public virtual ICollection<Translation> TranslationTargets { get; set; }
  }

  // 1:m with Statement: Synset => Statement
  public class Example {
    public int Id { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
    public string Text { get; set; } // e.g. "he finally got his big break"
  }

  // m:n witn RelType: Synset <=> Synset 
  public class SynsetRelation {
    public int SynsetFromId { get; set; }
    public Synset SynsetFrom { get; set; }
    public int SynsetToId { get; set; }
    public Synset SynsetTo { get; set; }
    // e.g. ants, hype, hmem, sim, mmem, hprt, hasi, dmnr, dmtc, ...
    // special: RelType=self - self referencing, has MonolingualExternalRefs
    public string RelType { get; set; } 
  }

  // m:n with Language: Synset <=> Synset 
  public class Translation {
    public int SynsetFromId { get; set; }
    public Synset SynsetFrom { get; set; }
    public int SynsetToId { get; set; }
    public Synset SynsetTo { get; set; }
    public string Language { get; set; }
  }

}