using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace bom.Services
{

    public static class MvcOptionsExtensions
    {
        public static void UseApiRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteApiConvention(routeAttribute));            
        }
    }

    public class RouteApiConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _prefix;

        public RouteApiConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _prefix = new AttributeRouteModel(routeTemplateProvider);
        }
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(c => c.ControllerType.ToString().Contains(".Api.")))
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_prefix,
                            selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = _prefix;
                    }
                }
            }
        }
    }
}
