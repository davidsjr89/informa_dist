using Usuario.API.Controllers;
using Usuario.API.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Usuario.Test
{
    public class UsuarioTest
    {
        private HomeController controller;
        public UsuarioTest()
        {
            controller = new HomeController();
        }
        [Fact]
        public void UsuarioLogin_OK()
        {
            var resposta = controller.Login(perfilUsuarioLogin("david", "123456")).Result.Value as UserToken;

            Assert.True(resposta.Token.Length > 0);
        }
        [Fact]
        public void UsuarioLogin_Errado()
        {
            var resposta = controller.Login(perfilUsuarioLogin("david", "1234567")).Result.Result as NotFoundObjectResult;
            Assert.Contains("{ message = Usuário ou senha inválidos }", resposta.Value.ToString());
        }
        private UserLogin perfilUsuarioLogin(string nome, string senha)
        {
            return new UserLogin { Nome = nome, Senha = senha };
        }
    }
}
