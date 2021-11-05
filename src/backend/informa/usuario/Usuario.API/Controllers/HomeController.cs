using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Usuario.API.Models;
using Usuario.API.Repositories;
using Usuario.API.Services;

namespace Usuario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Login(UserLogin model)
        {
            var user = UserRepository.Get(model.Nome, model.Senha);

            if (user == null) return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            var UserToken = new UserToken { User = user, Token = token };
            return UserToken;
        }

        [HttpPost]

        [HttpGet]
        [Route("anonima")]
        [AllowAnonymous]
        public string Anonima() => "Anônimo";
        [HttpGet]
        [Route("autenticado")]
        [Authorize]
        public string Autenticado() => "Autenticado";
        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "administrador")]
        public string Funcionario() => User.Identity.Name;
    }
}
