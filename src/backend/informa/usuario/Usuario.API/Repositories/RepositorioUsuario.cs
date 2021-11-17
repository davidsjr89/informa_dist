using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuario.API.Data;
using Usuario.API.Models;
using Usuario.API.Util;

namespace Usuario.API.Repositories
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly DataContext _context;

        public RepositorioUsuario(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> AtualizarUsuario(User usuario)
        {
            usuario.Senha = Settings.Criptografia(usuario.Senha);
            _context.Update(usuario);
            
            return _context.SaveChangesAsync().Result > 0 ? usuario : null;
        }

        public async Task<User> BuscarPorId(int id)
        {
                var user = await _context.Usuarios.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
                return user ?? null;
        }

        public async Task<User> BuscarPorNome(string nome)
        {
            var user = await _context.Usuarios.AsNoTracking().Where(x => x.Nome == nome).FirstOrDefaultAsync();
            return user ?? null;
        }

        public async Task<User> CadastrarUsuario(User usuario)
        {
            usuario.Senha = Settings.Criptografia(usuario.Senha);
            await _context.AddAsync(usuario);
            return await _context.SaveChangesAsync() > 0 ? usuario : null;
        }

        public async Task<User> DeletarUsuario(User user)
        {
            _context.Remove(user);
            return _context.SaveChangesAsync().Result > 0 ? user : null;
        }

        public async Task<IList<User>> ListaUsuario()
        {
            var user = await _context.Usuarios.AsNoTracking().ToListAsync();
            return user ?? null;
        }

        public async Task<User> Login(string usuario, string senha)
        {
            senha = Settings.Criptografia(senha);
            var user = await _context.Usuarios.AsNoTracking().Where(x => x.Nome == usuario && x.Senha == senha).FirstOrDefaultAsync();
            return user ?? null;
        }
    }
}
