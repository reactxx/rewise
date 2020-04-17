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


    public virtual DbSet<Ids> Ids { get; set; }
    public virtual DbSet<Lang> Langs { get; set; }
    public virtual DbSet<Entry> Entries { get; set; }
    public virtual DbSet<Sense> Senses { get; set; }
    public virtual DbSet<Relation> SynsetRelations { get; set; }
    public virtual DbSet<Synset> Synsets { get; set; }
    public virtual DbSet<Example> Examples { get; set; }
    public virtual DbSet<Translation> Translations { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {

      var lang = modelBuilder.Entity<Lang>();

      lang.HasMany(l => l.Entries)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      lang.HasMany(l => l.Synsets)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      lang.HasMany(l => l.Examples)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      lang.HasMany(l => l.Relations)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      lang.HasMany(l => l.Senses)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      lang.HasMany(l => l.Translations)
        .WithRequired(e => e.Lang)
        .HasForeignKey(e => e.LangId)
        .WillCascadeOnDelete(false);

      var lexEntry = modelBuilder.Entity<Entry>();
      var synset = modelBuilder.Entity<Synset>();

      lexEntry.HasMany(s => s.Senses)
        .WithRequired(c => c.Entry)
        .HasForeignKey(s => s.EntryId)
        .WillCascadeOnDelete(false);
      synset.HasMany(s => s.Senses)
        .WithRequired(c => c.Synset)
        .HasForeignKey(s => s.SynsetId)
        .WillCascadeOnDelete(false);

      lexEntry.HasMany(s => s.TransTrans)
        .WithRequired(c => c.TransEntry)
        .HasForeignKey(s => s.TransEntryId)
        .WillCascadeOnDelete(false);
      synset.HasMany(s => s.TransSrc)
        .WithRequired(c => c.EngSynset)
        .HasForeignKey(s => s.EngSynsetId)
        .WillCascadeOnDelete(false);

      synset.HasMany(s => s.Examples)
        .WithRequired(c => c.Synset)
        .HasForeignKey(s => s.SynsetId)
        .WillCascadeOnDelete(false);

      synset.HasMany(s => s.RelationTargets)
        .WithRequired(c => c.To)
        .HasForeignKey(s => s.ToId)
        .WillCascadeOnDelete(false);
      synset.HasMany(s => s.RelationSources)
        .WithRequired(c => c.From)
        .HasForeignKey(s => s.FromId)
        .WillCascadeOnDelete(false);

      // m:n entry LexicalEntry <=> Synset
      modelBuilder.Entity<Sense>()
        .HasKey(bc => new { bc.EntryId, bc.SynsetId });

      // m:n relation Synset <=> Synset
      modelBuilder.Entity<Relation>()
        .HasKey(bc => new { bc.FromId, bc.ToId });

      // m:n translation ENG Synset <=> LANG Entry
      modelBuilder.Entity<Translation>()
        .HasKey(bc => new { bc.EngSynsetId, bc.TransEntryId });

    }

  }

  public class Ids {
    public int Id { get; set; }
    public string Text { get; set; }
  }
  public class Lang {
    public string Id { get; set; }
    public bool OriginNoWikt { get; set; }
    public virtual ICollection<Entry> Entries { get; set; }
    public virtual ICollection<Synset> Synsets { get; set; }
    public virtual ICollection<Example> Examples { get; set; }
    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Relation> Relations { get; set; }
    public virtual ICollection<Translation> Translations { get; set; }
  }
  public class Entry {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string LangId { get; set; }
    public Lang Lang { get; set; }
    public string PartOfSpeech { get; set; } // e.g. "v" as verb
    public string Lemma { get; set; } // text, e.g. finish
    public bool OriginNoWikt { get; set; }
    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Translation> TransTrans { get; set; }
  }

  // Gloss
  public class Synset {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Meaning { get; set; } // vyklad, e.g. "an unexpected piece of good luck"
    public string LangId { get; set; }
    public Lang Lang { get; set; }
    public bool Top5000 { get; set; }

    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Translation> TransSrc { get; set; }
    public virtual ICollection<Example> Examples { get; set; }
    public virtual ICollection<Relation> RelationSources { get; set; }
    public virtual ICollection<Relation> RelationTargets { get; set; }

  }

  // m:n LexicalEntry <=> Synset 
  public class Sense {
    public int EntryId { get; set; }
    public Entry Entry { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
    public string LangId { get; set; }
    public Lang Lang { get; set; }
  }

  // 1:m with Statement: Synset => Statement
  public class Example {
    public int Id { get; set; }
    public int SynsetId { get; set; }
    public Synset Synset { get; set; }
    public string Text { get; set; } // e.g. "he finally got his big break"
    public string LangId { get; set; }
    public Lang Lang { get; set; }
  }

  public class Translation {
    public int EngSynsetId { get; set; }
    public Synset EngSynset { get; set; }
    public int TransEntryId { get; set; }
    public Entry TransEntry { get; set; }
    public bool OriginNoWikt { get; set; }
    public string LangId { get; set; }
    public Lang Lang { get; set; }
  }

  // m:n witn RelType: Synset <=> Synset 
  public class Relation {
    public int FromId { get; set; }
    public Synset From { get; set; }
    public int ToId { get; set; }
    public Synset To { get; set; }
    // e.g. ants, hype, hmem, sim, mmem, hprt, hasi, dmnr, dmtc, ...
    // special: RelType=self - self referencing, has MonolingualExternalRefs
    public string Type { get; set; }
    public string LangId { get; set; }
    public Lang Lang { get; set; }
  }


}