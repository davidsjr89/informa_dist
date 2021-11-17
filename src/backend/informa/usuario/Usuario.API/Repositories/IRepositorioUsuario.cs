using System.Collections.Generic;
using System.Threading.Tasks;
using Usuario.API.Models;

namespace Usuario.API.Repositories
{
    public interface IRepositorioUsuario
    {
        Task<IList<User>> ListaUsuario();
        Task<User> Login(string usuario, string senha);
        Task<User> BuscarPorId(int id);
        Task<User> BuscarPorNome(string nome);
        Task<User> AtualizarUsuario(User usuario);
        Task<User> CadastrarUsuario(User usuario);
        Task<User> DeletarUsuario(User usuario);
    }
}
