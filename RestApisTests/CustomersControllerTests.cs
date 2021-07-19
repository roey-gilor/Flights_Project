using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApisTests
{
    [TestClass]
    public class CustomersControllerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();// Initializes the webHost
        private HttpClient _httpClient;//Http client used to send requests to the contoller

        [TestInitialize]
        public async Task SetUp()
        {
            _httpClient = _testHostFixture.Client;
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
