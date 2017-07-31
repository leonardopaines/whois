using Base.Data.Model.Entities;
using Base.Services.Uteis;
using System;
using System.Collections.Generic;

namespace Base.Services.ViewModel
{
    public class PesquisaVM
    {
        public string Dominio { get; set; }

        public String Hospedagem { get; set; }

        public string Ip { get; set; }

        public String Titular { get; set; }

        public String Responsavel { get; set; }

        public DateTime DataExpiracao { get; set; }

        public DateTime DataRegistro { get; set; }

        public string DetalhesStatus { get; set; }

        public List<string> Dns { get; set; }

        public String WhoIs { get; set; }

        public StatusPesquisa Status { get; set; }
    }

    public static class ExtensionPesquisaVM
    {
        public static void SiteConvertExtension(this PesquisaVM pesquisaVM, site siteEntity)
        {
            if (siteEntity == null)
            {
                pesquisaVM.Status = StatusPesquisa.DISPONIVEL;
                pesquisaVM.DetalhesStatus = "Domínio disponível para registro";
                return;
            }

            pesquisaVM.Ip = siteEntity.ip;
            pesquisaVM.WhoIs = siteEntity.whois;
            pesquisaVM.Status = StatusPesquisa.ENCONTRADO;
            pesquisaVM.DetalhesStatus = "Domínio encontrado";

            pesquisaVM.DataRegistro = siteEntity.dtregistro;
            pesquisaVM.DataExpiracao = siteEntity.dtexpiracao;
            pesquisaVM.Titular = siteEntity.titular;
            pesquisaVM.Responsavel = siteEntity.responsavel;
            pesquisaVM.Dominio = siteEntity.dominio;
            pesquisaVM.Hospedagem = siteEntity.hospedagem;
            pesquisaVM.Dns = new List<string>();

            if (siteEntity.site_nserver != null)
                foreach (var item in siteEntity.site_nserver)
                    pesquisaVM.Dns.Add(item.dns);
        }
    }

}
