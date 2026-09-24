using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Infrastructure.Gateway;

public sealed class GatewayExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (GatewayApiException exception) when (!context.Response.HasStarted)
        {
            context.Response.Clear();
            if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var returnUrl = HttpMethods.IsGet(context.Request.Method)
                    ? context.Request.PathBase + context.Request.Path + context.Request.QueryString
                    : context.Request.PathBase + "/";
                await context.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = returnUrl });
                return;
            }

            var status = exception.StatusCode == HttpStatusCode.Forbidden ? 403
                : exception.StatusCode == HttpStatusCode.NotFound ? 404 : 503;
            var model = new GatewayErrorViewModel { StatusCode = status };
            var result = new ViewResult
            {
                ViewName = "~/Views/Shared/GatewayError.cshtml",
                StatusCode = status,
                ViewData = new ViewDataDictionary<GatewayErrorViewModel>(
                    context.RequestServices.GetRequiredService<IModelMetadataProvider>(), new ModelStateDictionary())
                { Model = model }
            };
            await result.ExecuteResultAsync(new ActionContext(context, context.GetRouteData(), new ActionDescriptor()));
        }
    }
}
