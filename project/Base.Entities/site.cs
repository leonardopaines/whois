namespace Base.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("whois.site")]
    public partial class site
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public site()
        {
            site_nserver = new HashSet<site_nserver>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string dominio { get; set; }

        [Required]
        [StringLength(90)]
        public string proprietario { get; set; }

        [Column(TypeName = "date")]
        public DateTime dtregistro { get; set; }

        [Column(TypeName = "date")]
        public DateTime dtexpiracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<site_nserver> site_nserver { get; set; }
    }
}
