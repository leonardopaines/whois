namespace Base.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("whois.site_nserver")]
    public partial class site_nserver
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_site { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string nserver { get; set; }

        public virtual site site { get; set; }
    }
}
