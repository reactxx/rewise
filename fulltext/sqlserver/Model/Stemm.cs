namespace fulltext.sqlserver
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stemm")]
    public partial class Stemm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stemm()
        {
            Sources = new HashSet<Source>();
            WordInStemms = new HashSet<WordInStemm>();
        }

        public int Id { get; set; }

        [Required]
        public string Stemms { get; set; }

        public long Hash { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Source> Sources { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WordInStemm> WordInStemms { get; set; }
    }
}
