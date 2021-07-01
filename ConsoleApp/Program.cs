using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var appClientId = config["ConsoleApp:ClientId"];
            var appClientSecret = config["ConsoleApp:Secret"];
            var appAuthority = $"{config["ConsoleApp:Instance"]}/{config["ConsoleApp:TenantId"]}";

            var app = ConfidentialClientApplicationBuilder
                .Create(appClientId)
                .WithClientSecret(appClientSecret)
                .WithAuthority(appAuthority)
                .Build();

            var authResult = await app
                .AcquireTokenForClient(new[] {config["Api:Scope"]})
                .ExecuteAsync();

            var token = authResult.AccessToken;

            Console.WriteLine(token);

            //var http = new HttpClient(new ApiHttpHandler(token))
            //{
            //    BaseAddress = new Uri(config["Api:BaseUri"])
            //};

            //using (http)
            //{
            //    var response = await http.GetAsync($"/weatherforecast");
            //    var content = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(content);
            //}
        }
    }
}
