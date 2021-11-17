using Bogus;
using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Usuario.API.Models;
using Xunit;

namespace Usuario.Teste
{
    public class HomeControllerTest : TestContext
    {
        private readonly string URL;
        private readonly Faker _faker;
        public HomeControllerTest()
        {
            URL = $"{TestClient.BaseAddress}api/Home/";
            _faker = new Faker("pt_BR");

        }
        [Fact]
        public async void Login_OK()
        {
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);

            Assert.Equal("administrador", retorno.User.Nome);
        }

        [Fact]
        public async void Login_Error_Usuario()
        {
            var response = await TestClient.PostAsJsonAsync(URL + "login", new UserLogin { Nome = "testes", Senha = "123456" });

            Assert.Equal(404, ((int)response.StatusCode));
        }
        [Fact]
        public async void Cadastrar_OK()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var resposta = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);
            var resultado = JsonConvert.DeserializeObject<UserToken>(resposta.Content.ReadAsStringAsync().Result);
            await TestClient.DeleteAsync(URL + $"deletar/{resultado.User.Id}");
            Assert.Equal(usuario.Nome, resultado.User.Nome);
        }
        [Fact]
        public async void Cadastrar_Error_JaExisteUsuario()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var rest = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);
            var resposta = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);
            var user = JsonConvert.DeserializeObject<UserToken>(rest.Content.ReadAsStringAsync().Result);
            await TestClient.DeleteAsync(URL + $"deletar/{user.User.Id}");
            Assert.Equal(404, (int)resposta.StatusCode);
        }

        [Fact]
        public async void Cadastrar_CamposNaoPreenchidos()
        {
            var login = UserLogin();
            login.Senha = "";
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);

            Assert.Equal(400, (int)response.StatusCode);
        }

        [Fact]
        public async void Deletar_OK()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var rest = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);
            var user = JsonConvert.DeserializeObject<UserToken>(rest.Content.ReadAsStringAsync().Result);
            await TestClient.DeleteAsync(URL + $"deletar/{user.User.Id}");
            Assert.Equal(201, (int)rest.StatusCode);
        }
        [Fact]
        public async void Deletar_Erro_NaoExisteID()
        {
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);
            var resultado = await TestClient.DeleteAsync(URL + $"deletar/7894544");
            Assert.Equal(404, (int)resultado.StatusCode);
        }

        [Fact]
        public async void Atualizar_OK()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var rest = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);

            var user = JsonConvert.DeserializeObject<UserToken>(rest.Content.ReadAsStringAsync().Result);

            User novoUsuario = Usuario();

            await TestClient.PutAsJsonAsync(URL + $"atualizar/{user.User.Id}", novoUsuario);

            await TestClient.DeleteAsync(URL + $"deletar/{user.User.Id}");
            Assert.Equal(201, (int)rest.StatusCode);
        }

        [Fact]
        public async void Atualizar_Erro_CamposNaoPreenchidos()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var rest = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);

            var user = JsonConvert.DeserializeObject<UserToken>(rest.Content.ReadAsStringAsync().Result);

            var novoUsuario = Usuario();
            novoUsuario.Cpf = "";
            novoUsuario.Email = "";
            var result = await TestClient.PutAsJsonAsync(URL + $"atualizar/{user.User.Id}", novoUsuario);

            var resultado = await TestClient.DeleteAsync(URL + $"deletar/{user.User.Id}");
            Assert.Equal(400, (int)result.StatusCode);
        }

        private static UserLogin UserLogin()
        {
            return new UserLogin { Nome = "administrador", Senha = "Teste789456123!" };
        }
        private User Usuario()
        {
            return new User
            {
                Nome = _faker.Name.FirstName(),
                Cpf = _faker.Person.Cpf(),
                Email = _faker.Person.Email,
                Role = "administrador",
                Senha = _faker.Internet.Password()
            };
        }

        [Fact]
        public async void BuscarPorId_OK()
        {
            User usuario = Usuario();
            var login = UserLogin();
            var response = await TestClient.PostAsJsonAsync(URL + "login", login);
            var registrationResponse = await response.Content.ReadAsStringAsync();

            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", retorno.Token);

            var resposta = await TestClient.PostAsJsonAsync(URL + "cadastrar", usuario);
            var resultado = JsonConvert.DeserializeObject<UserToken>(resposta.Content.ReadAsStringAsync().Result);

            var usuarioPorId = await TestClient.GetAsync(URL + $"buscar/{resultado.User.Id}");
            var usuarioPorIdResposta = JsonConvert.DeserializeObject<User>(usuarioPorId.Content.ReadAsStringAsync().Result);
            await TestClient.DeleteAsync(URL + $"deletar/{resultado.User.Id}");
            Assert.Equal(resultado.User.Nome, usuarioPorIdResposta.Nome);
        }
    }
}