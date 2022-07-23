using System.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Azure.Core;
using Moq;
using Xunit;
using FluentAssertions;

namespace Dedj.Extensions.Http.UnitTests;

public class TokenAuthenticationDelegatingHandlerShould
{
    private readonly HttpClient _client;
    private readonly ConfigurableMessageHandler _messageHandler;
    private readonly TokenAuthenticationDelegatingHandler _sut;
    private readonly Mock<TokenCredential> _mockTokenCredential;
    public TokenAuthenticationDelegatingHandlerShould()
    {
        _messageHandler = new ConfigurableMessageHandler();
        _mockTokenCredential = new Mock<TokenCredential>();
        _sut = new TokenAuthenticationDelegatingHandler(_messageHandler,_mockTokenCredential.Object);
        _client = new HttpClient(_sut)
        {
            BaseAddress = new System.Uri("http://tests.com")
        };    
    }   
    [Fact]
    public async Task CallTokenCredentialGetAsync_GivenRequestMessage_WhenSendAsync()
    {
        var request = new HttpRequestMessage();
        _messageHandler.ConfigureReturnMessage(() => new HttpResponseMessage());
        
        await _client.SendAsync(request);
        _mockTokenCredential.Verify(x => x.GetTokenAsync(It.IsAny<TokenRequestContext>(),It.IsAny<CancellationToken>()),Times.Once);
    }
    [Fact]
    public async Task AddAuthorizationHeader_GivenRequestMessage_WhenSendAsync()
    {
        var request = new HttpRequestMessage();
        var expectedToken = "testtoken";
        var expectedSchema = "Bearer";
        var expectedHeaderValue = new AuthenticationHeaderValue(expectedSchema,expectedToken);
        _messageHandler.ConfigureReturnMessage(() => new HttpResponseMessage());
        _mockTokenCredential.Setup(x => x.GetTokenAsync(It.IsAny<TokenRequestContext>(),It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new AccessToken(expectedToken,DateTime.Now.AddMinutes(30)));
                            
        await _client.SendAsync(request);

        var headers = request.Headers;
        headers.Authorization.Should()
                             .Be(expectedHeaderValue);
    }
    [Fact]
    public void CallTokenCredentialGetToken_GivenRequestMessage_WhenSend()
    {
        var request = new HttpRequestMessage();
        _messageHandler.ConfigureReturnMessage(() => new HttpResponseMessage());
        
        _client.Send(request);
        _mockTokenCredential.Verify(x => x.GetToken(It.IsAny<TokenRequestContext>(),It.IsAny<CancellationToken>()),Times.Once);
    }
        [Fact]
    public void AddAuthorizationHeader_GivenRequestMessage_WhenSend()
    {
        var request = new HttpRequestMessage();
        var expectedToken = "testtoken";
        var expectedSchema = "Bearer";
        var expectedHeaderValue = new AuthenticationHeaderValue(expectedSchema,expectedToken);
        _messageHandler.ConfigureReturnMessage(() => new HttpResponseMessage());
        _mockTokenCredential.Setup(x => x.GetToken(It.IsAny<TokenRequestContext>(),It.IsAny<CancellationToken>()))
                            .Returns(new AccessToken(expectedToken,DateTime.Now.AddMinutes(30)));
                            
        _client.Send(request);

        var headers = request.Headers;
        headers.Authorization.Should()
                             .Be(expectedHeaderValue);
    }
}
