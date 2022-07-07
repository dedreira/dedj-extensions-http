using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Azure.Core;
namespace Dedj.Extensions.Http.UnitTests;

public class TokenAuthenticationDelegatingHandler : DelegatingHandler
{
    private TokenCredential _tokenCredential;
    public TokenAuthenticationDelegatingHandler(TokenCredential tokenCredential)
    {
        _tokenCredential = tokenCredential;
    }

    public TokenAuthenticationDelegatingHandler(HttpMessageHandler innerHandler, TokenCredential tokenCredential) : base(innerHandler)
    {
        _tokenCredential = tokenCredential;
    }


    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.Send(request, cancellationToken);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken);
    }
}
