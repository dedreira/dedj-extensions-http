using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace System.Net.Http;

public class ConfigurableMessageHandler : HttpMessageHandler
{
    Func<HttpResponseMessage> _action = () => throw new NotImplementedException();
    public HttpRequestMessage? RequestMessage {get;private set;}
    public void ConfigureReturnMessage(Func<HttpResponseMessage> action)
    {
        _action  = action;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(_action.Invoke());

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) => _action.Invoke();
}
