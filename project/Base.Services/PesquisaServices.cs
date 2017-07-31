using Base.Data.Model.Entities;
using Base.DataModel.UnitOfWork;
using Base.Services.Uteis;
using Base.Services.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Base.Services
{
    /// <summary>
    /// Offers services for user specific operations
    /// </summary>
    public class PesquisaServices : IPesquisaServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public PesquisaServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PesquisaVM PesquisarDominio(String pesquisa, String ipRequisicao)
        {

            var pesquisaDominio = new PesquisaVM
            {
                Dominio = pesquisa.Trim().ToLower()
            };
            try
            {
                if (!Regex.IsMatch(pesquisaDominio.Dominio, @"^([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$")
                    || pesquisaDominio.Dominio.Length > 30)
                {
                    pesquisaDominio.Status = StatusPesquisa.ERRO;
                    pesquisaDominio.DetalhesStatus = "Dominio inválido";
                    return pesquisaDominio;
                }

                using (var scope = new TransactionScope())
                {
                    var siteEntity = _unitOfWork.SiteRepository
                        .GetFirst(x => x.dominio == pesquisaDominio.Dominio);

                    if (siteEntity == null)
                    {
                        siteEntity = WhoIs.IdentificarInformacoes(pesquisaDominio.Dominio);
                        if (siteEntity != null)
                        {
                            _unitOfWork.SiteRepository.Insert(siteEntity);
                        }
                    }

                    _unitOfWork.PesquisaRepository
                        .Insert(
                        new pesquisa
                        {
                            conteudo = pesquisaDominio.Dominio,
                            dthrpesquisa = DateTime.Now,
                            iprequisicao = ipRequisicao,
                            site = siteEntity
                        });

                    _unitOfWork.Save();

                    pesquisaDominio.SiteConvertExtension(siteEntity);

                    scope.Complete();
                }
            }
            catch (Exception objException)
            {
                pesquisaDominio.DetalhesStatus = String.Concat(
                    objException.Message, " - Stack:", objException.StackTrace);
                pesquisaDominio.Status = StatusPesquisa.ERRO;
            }

            return pesquisaDominio;
        }
    }
}
