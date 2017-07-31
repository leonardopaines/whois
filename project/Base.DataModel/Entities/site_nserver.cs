namespace Base.Data.Model.Entities
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
        public int idsite { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string dns { get; set; }

        public virtual site site { get; set; }
    }
}
