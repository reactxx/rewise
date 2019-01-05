namespace fulltext.sqlserver
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WordInStemm")]
    public partial class WordInStemm
    {
        public int Id { get; set; }

        public int WordId { get; set; }

        public int StemmId { get; set; }

        public virtual Source Source { get; set; }

        public virtual Stemm Stemm { get; set; }
    }
}
