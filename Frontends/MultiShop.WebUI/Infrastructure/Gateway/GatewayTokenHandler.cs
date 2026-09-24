using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MultiShop.WebUI.Infrastructure.Gateway;

public sealed class GatewayTokenHandler(IHttpContextAccessor contextAccessor, IConfiguration configuration)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var gateway = new Uri(configuration["GatewayApi:BaseUrl"]!);
        var destination = request.RequestUri;
        if (destination is null || destination.Scheme != gateway.Scheme || destination.Host != gateway.Host ||
            destination.Port != gateway.Port || !destination.AbsolutePath.StartsWith(gateway.AbsolutePath, StringComparison.Ordinal))
            throw new InvalidOperationException("Gateway istemcisi yalnızca yapılandırılan Gateway adresini çağırabilir.");

        var context = contextAccessor.HttpContext;
        request.Headers.Authorization = null;
        if (context?.User.Identity?.IsAuthenticated == true)
        {
            var token = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");
            if (string.IsNullOrWhiteSpace(token))
                throw new GatewayApiException(HttpStatusCode.Unauthorized);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        HttpResponseMessage response;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new GatewayApiException(HttpStatusCode.ServiceUnavailable);
        }
        catch (OperationCanceledException) when (context?.RequestAborted.IsCancellationRequested != true)
        {
            throw new GatewayApiException(HttpStatusCode.GatewayTimeout);
        }

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden ||
            (request.Method == HttpMethod.Get && response.StatusCode == HttpStatusCode.NotFound) ||
            (int)response.StatusCode >= 500)
        {
            var status = response.StatusCode;
            response.Dispose();
            throw new GatewayApiException(status);
        }

        return response;
    }
}
