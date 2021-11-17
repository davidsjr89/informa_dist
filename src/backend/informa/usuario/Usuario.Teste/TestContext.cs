using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Usuario.API;
using Usuario.API.Data;
using Usuario.API.Models;
using Newtonsoft.Json;

namespace Usuario.Teste
{
    public class TestContext
    {
        protected readonly HttpClient TestClient;
        private readonly string URL = "";
        protected TestContext()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DataContext));
                    services.AddDbContext<DataContext>(x => x.UseSqlServer
                    ("Server=localhost;Database=Informa_Teste;User Id=sa;Password=123456;"));
                });
            });
            TestClient = appFactory.CreateClient();
            URL = $"{TestClient.BaseAddress}api/Home/";
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(URL+"login", new UserLogin
            {
                Nome = "david",
                Senha = "123456"
            });

            var registrationResponse = await response.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<UserToken>(registrationResponse);
            return retorno.Token;
        }


    }
}
