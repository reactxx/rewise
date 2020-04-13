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
      lang.HasMany(l => l.Translations)
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

      var lexEntry = modelBuilder.Entity<Entry>();
      // lexEntry.HasIndex(s => new { s.Language, s.Lemma });
      lexEntry.HasMany(s => s.Senses)
      .WithRequired(c => c.Entry)
      .HasForeignKey(s => s.EntryId)
      .WillCascadeOnDelete(false);


      // m:n LexicalEntry <=> Synset
      modelBuilder.Entity<Sense>()
        .HasKey(bc => new { bc.EntryId, bc.SynsetId });

      modelBuilder.Entity<Synset>()
               .HasMany(s => s.Senses)
               .WithRequired(c => c.Synset)
               .HasForeignKey(s => s.SynsetId)
               .WillCascadeOnDelete(false);

      // m:n SensLexicalEntry <=> Synset
      modelBuilder.Entity<Sense>()
        .HasKey(bc => new { bc.EntryId, bc.SynsetId });

      // m:n Synset <=> Synset
      modelBuilder.Entity<Translation>()
        .HasKey(bc => new { bc.SrcId, bc.TransId });

      var synset = modelBuilder.Entity<Synset>();
      synset.HasMany(s => s.TransTrans)
               .WithRequired(c => c.Trans)
               .HasForeignKey(s => s.TransId)
               .WillCascadeOnDelete(false);

      synset.HasMany(s => s.TransSrc)
               .WithRequired(c => c.Src)
               .HasForeignKey(s => s.SrcId)
               .WillCascadeOnDelete(false);

      synset.HasMany(s => s.RelationTargets)
               .WithRequired(c => c.To)
               .HasForeignKey(s => s.ToId)
               .WillCascadeOnDelete(false);

      synset.HasMany(s => s.RelationSources)
               .WithRequired(c => c.From)
               .HasForeignKey(s => s.FromId)
               .WillCascadeOnDelete(false);

      modelBuilder.Entity<Relation>()
        .HasKey(bc => new { bc.FromId, bc.ToId });

    }

  }

  public class Lang {
    public string Id { get; set; }
    public virtual ICollection<Entry> Entries { get; set; }
    public virtual ICollection<Synset> Synsets { get; set; }
    public virtual ICollection<Example> Examples { get; set; }
    public virtual ICollection<Translation> Translations { get; set; }
    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Relation> Relations { get; set; }
  }
  public class Entry {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    // [MaxLength(5)]
    public string LangId { get; set; }
    public Lang Lang { get; set; }
    public string PartOfSpeech { get; set; } // e.g. "v" as verb
    // [MaxLength(128)]
    public string Lemma { get; set; } // text, e.g. finish
    public virtual ICollection<Sense> Senses { get; set; }
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

  // Gloss
  public class Synset {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Meaning { get; set; } // vyklad, e.g. "an unexpected piece of good luck"
    public string LangId{ get; set; }
    public Lang Lang { get; set; }

    // m:n Synset <=> LexicalEntry by Sense
    public virtual ICollection<Sense> Senses { get; set; }
    public virtual ICollection<Example> Examples { get; set; }
    // m:n Synset <=> Synset by SynsetRelation (with RelType, e.g. ants, hype, hmem, sim, mmem, hprt, hasi, dmnr, dmtc,)
    public virtual ICollection<Relation> RelationSources { get; set; }
    public virtual ICollection<Relation> RelationTargets { get; set; }
    // m:n Synset <=> Synset by Translation. Translation in other language with trans LANG
    public virtual ICollection<Translation> TransSrc { get; set; }
    public virtual ICollection<Translation> TransTrans { get; set; }
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

  // m:n with Language: Synset <=> Synset 
  public class Translation {
    public int SrcId { get; set; }
    public Synset Src { get; set; }
    public int TransId { get; set; }
    public Synset Trans { get; set; }
    public string LangId { get; set; }
    public Lang Lang { get; set; }
  }

}