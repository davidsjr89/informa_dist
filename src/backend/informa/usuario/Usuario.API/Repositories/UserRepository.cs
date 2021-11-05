using System.Collections.Generic;
using Usuario.API.Models;

namespace Usuario.API.Repositories
{
    public class UserRepository
    {
        public static User Get(string nome, string senha)
        {
            var users = new List<User>()
            {
                new User{Id = 1, Nome = "david", Senha = "123456", Cpf = "123456789", Email = "david@david.com", Role = "administrador"},
                new User{Id = 1, Nome = "teste", Senha = "123456", Cpf = "123456789", Email = "teste@teste.com", Role = "visualizacao"},
            };
            return users.Find(x => x.Nome == nome && x.Senha == senha);
        }
    }
}
