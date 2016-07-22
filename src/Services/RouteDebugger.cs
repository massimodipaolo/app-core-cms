using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Services
{
    public class RouteDebugger : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        public async Task RouteAsync(RouteContext context)
        {            
            var routeValues = string.Join("\r\n", context.RouteData.Values);
            var message = $"{routeValues}";
            await context.HttpContext.Response.WriteAsync(message);            
        }
    }
}
