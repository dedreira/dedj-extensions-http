using System.Net.Http;

namespace Dedj.Extensions.Http.UnitTests;

public class TestService
{
    private readonly HttpClient _client;

    public TestService(HttpClient client)
    {
        _client = client;
    }
}
