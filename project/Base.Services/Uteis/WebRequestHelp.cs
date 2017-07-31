using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Base.Services.Uteis
{
    public class WebRequestHelp : IDisposable
    {
        #region atributos
        private const int REQUEST_TIMEOUT = 300000;

        private CookieCollection cookies;

        private WebHeaderCollection headers;

        private bool keepSession;

        private HttpWebRequest httpRequest;

        private HttpWebResponse response;

        public HttpStatusCode httpStatusCode;

        private string descriptionError;

        private string content;

        private byte[] responseStream;

        private string redirectUrl;
        #endregion

        #region propriedades

        public void AddHeader(string name, string value)
        {
            this.headers.Add(name, value);
        }

        public void ClearHeaders()
        {
            this.headers.Clear();
        }

        public bool KeepSession
        {
            get
            {
                return this.keepSession;
            }
            set
            {
                this.keepSession = value;
            }
        }

        #endregion

        #region construtor
        public WebRequestHelp()
        {
            this.cookies = new CookieCollection();
            this.headers = new WebHeaderCollection();
        }
        #endregion

        #region metodos
        public virtual void Dispose()
        {
            response = null;
            responseStream = null;
            httpRequest = null;
            GC.SuppressFinalize(this);
        }

        public void Request(string url, string postString = "", bool processContent = true)
        {
            this.response = null;
            this.redirectUrl = "";
            this.content = "";
            try
            {
                try
                {
                    this.httpRequest = GetRequest(url, postString);
                    try
                    {
                        this.response = (HttpWebResponse)this.httpRequest.GetResponse();
                        this.responseStream = this.response.GetResponseStream().ReadToEnd();
                    }
                    catch (Exception exception)
                    {
                        this.descriptionError = string.Concat("Http Request ", exception.Message);
                    }

                    if (this.response == null)
                    {
                        this.httpStatusCode = HttpStatusCode.BadRequest;
                        return;
                    }

                    this.httpStatusCode = this.response.StatusCode;
                    if (this.response.StatusCode == HttpStatusCode.Found
                        || this.response.StatusCode == HttpStatusCode.MovedPermanently
                        || this.response.StatusCode == HttpStatusCode.MovedPermanently
                        || this.response.StatusCode == HttpStatusCode.Found)
                    {
                        this.redirectUrl = this.response.Headers["Location"];
                        while (!string.IsNullOrEmpty(this.redirectUrl))
                        {
                            this.httpRequest = GetRequest(this.redirectUrl, "");
                            try
                            {
                                this.response = (HttpWebResponse)this.httpRequest.GetResponse();
                                this.responseStream = this.response.GetResponseStream().ReadToEnd();
                                if (this.response != null)
                                {
                                    this.httpStatusCode = this.response.StatusCode;
                                }
                                this.redirectUrl = this.response.Headers["Location"];
                            }
                            catch (Exception objException)
                            {
                                this.descriptionError = string.Concat("Http Request ", objException.Message);
                            }
                        }
                    }

                    if (this.keepSession)
                    {
                        foreach (Cookie cooky in this.response.Cookies)
                        {
                            if (this.cookies[cooky.Name] == null)
                            {
                                this.cookies.Add(cooky);
                            }
                            else
                            {
                                this.cookies[cooky.Name].Comment = cooky.Comment;
                                this.cookies[cooky.Name].CommentUri = cooky.CommentUri;
                                this.cookies[cooky.Name].Discard = cooky.Discard;
                                this.cookies[cooky.Name].Domain = cooky.Domain;
                                this.cookies[cooky.Name].Expired = cooky.Expired;
                                this.cookies[cooky.Name].Expires = cooky.Expires;
                                this.cookies[cooky.Name].HttpOnly = cooky.HttpOnly;
                                this.cookies[cooky.Name].Path = cooky.Path;
                                this.cookies[cooky.Name].Port = cooky.Port;
                                this.cookies[cooky.Name].Secure = cooky.Secure;
                                this.cookies[cooky.Name].Value = cooky.Value;
                                this.cookies[cooky.Name].Version = cooky.Version;
                            }
                        }
                    }

                    if (this.response.StatusCode == HttpStatusCode.OK && processContent)
                    {
                        this.content = Encoding.UTF8.GetString(this.responseStream);
                    }
                }
                catch (WebException objWebException)
                {
                    this.descriptionError = string.Concat("Web Error: ", objWebException.Message, "Status: ", objWebException.Status.ToString());
                    this.httpStatusCode = HttpStatusCode.BadRequest;
                }
                catch (Exception objException)
                {
                    this.descriptionError = string.Concat("General Error: ", objException.Message);
                    this.httpStatusCode = HttpStatusCode.BadRequest;
                }
            }
            finally
            {
                if (this.response != null)
                {
                    this.response.Close();
                }
            }
        }

        private HttpWebRequest GetRequest(string url, string postString)
        {
            url = WebUteis.VerifyURL(url);
            var item = WebRequest.Create(url) as HttpWebRequest;

            item.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";
            item.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            item.ContentType = "application/x-www-form-urlencoded";
            item.Headers.Add("Accept-Language", "pt-br,pt;q=0.8,en-us;q=0.5,en;q=0.3");
            item.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");

            if (this.headers.HasKeys())
            {
                foreach (var key in this.headers.AllKeys)
                    item.Headers.Add(key, this.headers[key]);
            }

            item.Timeout = 300000;

            if (this.keepSession)
            {
                item.CookieContainer = new CookieContainer();
                item.CookieContainer.Add(this.cookies);
            }
            item.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (string.IsNullOrEmpty(postString))
            {
                item.Method = "GET";
            }
            else
            {
                var bytes = Encoding.ASCII.GetBytes(postString);
                item.Method = "POST";
                item.ContentLength = (long)((int)bytes.Length);
                var requestStream = item.GetRequestStream();
                requestStream.Write(bytes, 0, (int)bytes.Length);
                requestStream.Close();
            }

            return item;
        }

        public HtmlDocument LoadHtml()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(this.content);
            return doc;
        }
        #endregion
    }
}
