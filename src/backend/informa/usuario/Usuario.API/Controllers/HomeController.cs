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
        private readonly IRepositorioUsuario _repositorioUsuario;
        public HomeController(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<dynamic>> BuscarPorId(int id)
        {
            if (!ModelState.IsValid) NotFound(new { message = "Todos os campos devem ser preenchidos" });
            var user = await _repositorioUsuario.BuscarPorId(id);

            if (user == null) return NotFound(new { message = "Usuário ou senha inválidos" });

            return user;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Login(UserLogin model)
        {
            if (!ModelState.IsValid) NotFound(new { message = "Todos os campos devem ser preenchidos" });
            var user = await _repositorioUsuario.Login(model.Nome, model.Senha);

            if (user == null) return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            var UserToken = new UserToken { User = user, Token = token };
            return UserToken;
        }

        [HttpPost]
        [Route("cadastrar")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult<dynamic>> Cadastrar(User model)
        {
            if (!ModelState.IsValid) return NotFound(new { message = "Todos os campos devem ser preenchidos" });
            var user = await _repositorioUsuario.BuscarPorNome(model.Nome);

            if (user == null)
            {
                var resposta = await _repositorioUsuario.CadastrarUsuario(model);
                var token = TokenService.GenerateToken(model);
                resposta.Senha = "";
                var UserToken = new UserToken { User = resposta, Token = token };
                return Created("", UserToken);
            }
            return NotFound(new { message = "Já existe informação no banco de dados." });
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult<dynamic>> Deletar(int id)
        {
            if(!ModelState.IsValid) return NotFound(new { message = "Todos os campos devem ser preenchidos" });
            var user = await _repositorioUsuario.BuscarPorId(id);

            if (user == null) return NotFound(new { message = "Nâo existe usuário." });

            var resposta = await _repositorioUsuario.DeletarUsuario(user);

            return Ok(resposta);
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult<dynamic>> Atualizar(int id, User model)
        {
            if (!ModelState.IsValid) return NotFound(new { message = "Todos os campos devem ser preenchidos" });
            var user = await _repositorioUsuario.BuscarPorId(id);

            if (user == null) return NotFound(new { message = "Nâo existe usuário." });
            model.Id = user.Id;
            var resposta = await _repositorioUsuario.AtualizarUsuario(model);

            return Ok(resposta);
        }
    }
}
