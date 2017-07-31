namespace Base.Data.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("whois.pesquisa")]
    public partial class pesquisa
    {
        public int id { get; set; }

        public DateTime dthrpesquisa { get; set; }

        [Required]
        [StringLength(30)]
        public string conteudo { get; set; }

        [Required]
        [StringLength(30)]
        public string iprequisicao { get; set; }

        public int? idsite { get; set; }

        public virtual site site { get; set; }
    }
}
