namespace fulltext.sqlserver
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Source")]
    public partial class Source
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Source()
        {
            WordInStemms = new HashSet<WordInStemm>();
        }

        public int Id { get; set; }

        public int? StemmId { get; set; }

        [Required]
        [StringLength(50)]
        public string Word { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WordInStemm> WordInStemms { get; set; }

        public virtual Stemm Stemm { get; set; }
    }
}
