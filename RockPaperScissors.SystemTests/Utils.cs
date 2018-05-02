using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace RockPaperScissors.SystemTests
{
    public class TestUtils
    {
        public static HttpClient GetNewTestClient()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("SystemTest")
                .UseStartup<Startup>();

            var testServer = new TestServer(builder);
            return testServer.CreateClient();
        }
    }
}
