using Base.Data.Model.Entities;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Base.Services.Uteis
{
    public class WhoIs
    {
        public static site IdentificarInformacoes(String strDominio)
        {

            var siteEntity = new site
            {
                dominio = strDominio
            };

            using (var request = new WebRequestHelp())
            {
                var urlWhoIs = String.Concat("https://www.whois.com/whois/", siteEntity.dominio);
                request.KeepSession = false;
                request.Request(urlWhoIs);

                if (request.httpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Erro ao verificar informações em " + urlWhoIs);
                }

                var html = request.LoadHtml();

                var htmlNode = html.DocumentNode.SelectSingleNode("//*[@id='registryData' or @id='registrarData']");

                if (htmlNode == null)
                    return null;

                siteEntity.whois = htmlNode.InnerText;

                var split = htmlNode.InnerText.Split('\n');

                #region Identificar Responsavel
                var person = split.FirstOrDefault(x =>
                 x.Contains("Registrant Name") || x.Contains("person"));
                if (!String.IsNullOrEmpty(person))
                {
                    var splitName = person.Split(':');
                    if (splitName.Length == 2)
                        siteEntity.responsavel = splitName[1].Trim();
                    else
                        siteEntity.responsavel = "Não identificado";
                }
                #endregion

                #region Identificar Titular
                var owner = split.FirstOrDefault(x =>
                 x.Contains("Registrant Organization") || x.Contains("owner"));
                if (!String.IsNullOrEmpty(owner))
                {
                    var splitOwner = owner.Split(':');
                    if (splitOwner.Length == 2)
                        siteEntity.titular = splitOwner[1].Trim();
                    else
                        siteEntity.titular = "Não identificado";
                }
                #endregion

                #region Identificar DNS
                var nservers = split.Where(x =>
                   x.Contains("Name Server") || x.Contains("nserver"));
                if (nservers != null)
                {
                    foreach (var nserver in nservers)
                    {
                        var splitNServer = nserver.Split(':');

                        if (splitNServer.Length == 2)
                        {
                            siteEntity.site_nserver.Add(
                                new site_nserver
                                {
                                    dns = splitNServer[1].Trim().ToUpper(),
                                    site = siteEntity
                                });
                        }
                    }

                }
                #endregion

                #region Data do registro
                try
                {
                    var created = split.FirstOrDefault(x =>
                         x.Contains("Creation") || x.Contains("created"));
                    if (created != null)
                    {
                        var strDtAuxCreate = created.Split(':')[1];
                        strDtAuxCreate = strDtAuxCreate.Split('#')[0].Split('T')[0].Trim();

                        if (!DateTime.TryParse(strDtAuxCreate, out DateTime dtAuxCreate))
                        {
                            strDtAuxCreate = new String(strDtAuxCreate.Where(Char.IsDigit).ToArray());

                            var ano = int.Parse(strDtAuxCreate.Substring(0, 4));
                            var mes = int.Parse(strDtAuxCreate.Substring(4, 2));
                            var dia = int.Parse(strDtAuxCreate.Substring(6, 2));
                            dtAuxCreate = new DateTime(ano, mes, dia);
                        }
                        siteEntity.dtregistro = dtAuxCreate;
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Data de termino
                try
                {
                    var expires = split.FirstOrDefault(x =>
                x.Contains("Expiration") || x.Contains("expires"));
                    if (expires != null)
                    {
                        var strDtAuxExpires = expires.Split(':')[1];
                        strDtAuxExpires = strDtAuxExpires.Split('#')[0].Split('T')[0].Trim();

                        if (!DateTime.TryParse(strDtAuxExpires, out DateTime dtAuxExpires))
                        {
                            strDtAuxExpires = new String(strDtAuxExpires.Where(Char.IsDigit).ToArray());

                            var ano = int.Parse(strDtAuxExpires.Substring(0, 4));
                            var mes = int.Parse(strDtAuxExpires.Substring(4, 2));
                            var dia = int.Parse(strDtAuxExpires.Substring(6, 2));
                            dtAuxExpires = new DateTime(ano, mes, dia);
                        }
                        siteEntity.dtexpiracao = dtAuxExpires;
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Identificar IP

                var ip = String.Empty;
                try
                {
                    ip = Dns.GetHostAddresses(siteEntity.dominio).FirstOrDefault(addr =>
                    addr.AddressFamily == AddressFamily.InterNetwork)?.ToString();
                }
                catch (Exception)
                {
                    ip = "Não identificado";
                }

                siteEntity.ip = ip;

                #endregion

                #region Identificar hospedagem

                try
                {
                    var whoishostingthis = string.Concat("http://www.whoishostingthis.com/?q=", siteEntity.dominio);
                    request.KeepSession = true;

                    request.Request(whoishostingthis);
                    if (request.httpStatusCode != HttpStatusCode.OK)
                        throw new Exception("Erro ao verificar informações em " + whoishostingthis);

                    html = request.LoadHtml();

                    var regex = new Regex(string.Format("q={0}&h=(.*?)', function", siteEntity.dominio));
                    var v = regex.Match(html.DocumentNode.InnerText);
                    var redirect = v.Groups[1].ToString();

                    whoishostingthis = string.Format("http://www.whoishostingthis.com/ajax/site-info/?q={0}&h={1}", siteEntity.dominio, redirect);

                    request.AddHeader("X-Requested-With", "XMLHttpRequest");

                    request.Request(whoishostingthis);

                    if (request.httpStatusCode != HttpStatusCode.OK)
                        throw new Exception("Erro ao verificar informações em " + whoishostingthis);

                    html = request.LoadHtml();
                    htmlNode = html.DocumentNode.SelectSingleNode("//span[contains(@class, 'normal') and contains(@class, 'blue')]");
                    var hospedagem = htmlNode?.InnerText.Trim();
                    siteEntity.hospedagem = string.IsNullOrWhiteSpace(hospedagem) ? "Não identificado" : hospedagem;

                }
                catch (Exception)
                {
                    siteEntity.hospedagem = "Não identificado";
                }

                #endregion
            }

            return siteEntity;
        }

    }
}
