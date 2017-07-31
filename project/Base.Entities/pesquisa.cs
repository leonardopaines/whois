namespace Base.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("whois.pesquisa")]
    public partial class pesquisa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime dthrpesquisa { get; set; }

        [Required]
        [StringLength(30)]
        public string conteudo { get; set; }

        [Required]
        [StringLength(15)]
        public string iprequisicao { get; set; }

        public int status { get; set; }
    }
}
