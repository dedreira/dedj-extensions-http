using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Azure.Core;

namespace Dedj.Extensions.Http.UnitTests;
public class TokenAuthenticationDelegatingHandler : DelegatingHandler
{
    private TokenCredential _tokenCredential;
    private const string scheme = "Bearer";
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
        var token =  _tokenCredential.GetToken(new TokenRequestContext(),cancellationToken);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme,token.Token);
        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenCredential.GetTokenAsync(new TokenRequestContext(),cancellationToken);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme,token.Token);
        return await base.SendAsync(request, cancellationToken);
    }
}
