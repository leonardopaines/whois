
using Base.Services.ViewModel;

namespace Base.Services
{
    public interface IPesquisaServices
    {
        PesquisaVM PesquisarDominio(string pesquisaDominio, string ipRequest);
    }
}
