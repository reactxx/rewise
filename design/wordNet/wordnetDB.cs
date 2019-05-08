namespace wordNetDB {
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity;

  public class Context : DbContext {
    // Your context has been configured to use a 'wordnetDB' connection string from your application's 
    // configuration file (App.config or Web.config). By default, this connection string targets the 
    // 'wordNet.wordnetDB' database on your LocalDb instance. 
    // 
    // If you wish to target a different database and/or database provider, modify the 'wordnetDB' 
    // connection string in the application configuration file.
    public Context()
        : base("name=wordnetDB") {
    }

    // Add a DbSet for each entity type that you want to include in your model. For more information 
    // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

    public virtual DbSet<LexicalEntry> LexicalEntries { get; set; }

  }

  public class LexicalEntry {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string PartOfSpeech { get; set; }
    public string Lemma { get; set; }
  }

  public class Sense {
    [Key]
    public int EntryId { get; set; }
    [Key]
    public int SynsetId { get; set; }
  }

  public class SynsetRelation {
    [Key]
    public int FromId { get; set; }
    [Key]
    public int ToId { get; set; }
    public string RelType { get; set; }
  }

  public class Synset {
    public int Id { get; set; }
    public string Gloss { get; set; }
  }

  public class Statement {
    public int Id { get; set; }
    public string Example { get; set; }
  }

  public class Translation {
    [Key]
    public int FromId { get; set; }
    [Key]
    public int EngId { get; set; }
    public string Language { get; set; }
  }

}