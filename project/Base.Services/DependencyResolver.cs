using Base.Resolver;
using System.ComponentModel.Composition;

namespace Base.Services
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IPesquisaServices, PesquisaServices>();
        }
    }
}
