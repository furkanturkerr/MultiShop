using System.Net;

namespace MultiShop.WebUI.Infrastructure.Gateway;

public sealed class GatewayApiException(HttpStatusCode statusCode) : Exception("Gateway request failed.")
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
