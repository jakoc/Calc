using Calculator;
using Microsoft.Extensions.Configuration;

namespace Tests.tests
{
    public class ApplicationConfiguratorTests
    {
        private ApplicationConfigurator _configurator;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                { "Application:BaseUrl", "http://129.151.223.141" }  // Her bruger vi den rigtige base URL
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _configurator = new ApplicationConfigurator(configuration);
        }

        [Test]
        public void GetBaseUrl_ShouldReturnConfiguredBaseUrl()
        {
            var result = _configurator.GetBaseUrl();
            Assert.That(result, Is.EqualTo("http://129.151.223.141"));
        }

        [Test]
        public void GetBaseUrl_ShouldThrowException_WhenNoBaseUrlConfigured()
        {
            var inMemorySettings = new Dictionary<string, string?>(); // Ingen baseurl
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings) // Tilføj en tom indsamling
                .Build();

            _configurator = new ApplicationConfigurator(configuration);

            // Tester, at en InvalidOperationException kastes, når der ikke er nogen BaseUrl
            Assert.Throws<InvalidOperationException>(() => _configurator.GetBaseUrl());
        }
    }
}