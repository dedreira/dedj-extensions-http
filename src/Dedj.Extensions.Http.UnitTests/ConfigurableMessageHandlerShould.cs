using System;
using System.Net.Http;
using System.Transactions;
using Xunit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;

namespace Dedj.Extensions.Http.UnitTests;
public class ConfigurableMessageHandlerShould : ConfigurableMessageHandler
{
    [Fact]
    public async Task ThrowNotImplementedException_GivenActionIsNotConfigured_WhenSendAsync()    
    {        
        var request = new HttpRequestMessage();

        var sendAsync = async () => await SendAsync(request,CancellationToken.None);

        await sendAsync.Should().ThrowAsync<NotImplementedException>();        
    }

    [Fact]
    public async Task ThrowException_GivenActionIsConfiguredToThrow_WhenSendAsync()
    {
        var request = new HttpRequestMessage();

        Func<HttpResponseMessage> action = () => throw new ArgumentNullException();

        ConfigureReturnMessage(action);

        var sendAsync = async () => await SendAsync(request,CancellationToken.None);

        await sendAsync.Should().ThrowAsync<ArgumentNullException>();         
    }

    [Fact]
    public async Task ReturnExpectedResponseMessage_GivenResponseMessageIsConfigured_WhenSendAsync()
    {
        var request = new HttpRequestMessage();
        var expectedResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);        
        var action = () => {return expectedResponseMessage;};  

        var response = await SendAsync(request,CancellationToken.None);

        response.Should().Be(expectedResponseMessage);

    }
}