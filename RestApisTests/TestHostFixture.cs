using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestApisTests
{
    public class TestHostFixture : IDisposable
    {
        public HttpClient Client { get; }//Http client used to send requests to the contoller
        public IServiceProvider ServiceProvider { get; }//Service provider used to provide services that registered in the API
        public TestHostFixture()
        {
            var builder = WebApplicationProject.Program.CreateHostBuilder(null)
    .ConfigureWebHost(webHost =>//Configure the web host to use test server and test environament
                {
        webHost.UseTestServer();
        webHost.UseEnvironment("Test");//If we have different configuration file for the
                                       //test environment we will be able to check with simple if statement
                                       //I did an example of this in the program class in remarks                                               
                });
            var host = builder.Start();//Start the host
            ServiceProvider = host.Services;//Get the services from the host
            Client = host.GetTestClient();//Get the test client from the host
        }
        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
