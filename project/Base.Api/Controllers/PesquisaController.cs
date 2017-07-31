using Base.Services;
using Base.Services.ViewModel;
using System;
using System.Web;
using System.Web.Http;

namespace Base.Api.Controllers
{
    public class PesquisaController : ApiController
    {
        private IPesquisaServices _pesquisaServices;

        public PesquisaController(IPesquisaServices pesquisaService)
        {
            _pesquisaServices = pesquisaService;
        }

        /// <summary>
        /// Serviço utilizado para consultar informações sobre dominio
        /// </summary>
        /// <param name="dominio">Dominio (google.com, g1.com.br, etc...)</param>
        /// <returns>PesquisaVM - informações gerais da consulta</returns>
        [Route("api/pesquisa/{dominio=dominio}")]
        public PesquisaVM Get(String dominio)
        {
            var pesquisaVM = _pesquisaServices.PesquisarDominio(dominio, HttpContext.Current?.Request.UserHostAddress);
            return pesquisaVM;
        }
    }
}