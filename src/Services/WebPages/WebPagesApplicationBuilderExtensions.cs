using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace bom.Services.WebPages
{
    public static class WebPagesApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebPages(this IApplicationBuilder app, WebPagesOptions opts)
        {
            var renderer = app.ApplicationServices.GetRequiredService<RazorViewToStringRenderer>();
            var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            return app.UseRouter(new WebPagesRouter(hostingEnvironment, renderer, opts));
        }
    }
}