namespace Base.Data.Model.Entities
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
            pesquisa = new HashSet<pesquisa>();
            site_nserver = new HashSet<site_nserver>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(60)]
        public string dominio { get; set; }

        [Required]
        [StringLength(100)]
        public string hospedagem { get; set; }

        [Required]
        [StringLength(30)]
        public string ip { get; set; }

        [StringLength(100)]
        public string titular { get; set; }

        [StringLength(100)]
        public string responsavel { get; set; }

        [Column(TypeName = "date")]
        public DateTime dtregistro { get; set; }

        [Column(TypeName = "date")]
        public DateTime dtexpiracao { get; set; }

        [Required]
        [StringLength(1073741823)]
        public string whois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pesquisa> pesquisa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<site_nserver> site_nserver { get; set; }
    }
}
