using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ModularizationDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static bool TryMatchTemplate(string requestPath, string routeTemplate)
        {
            var routeValues = new RouteValueDictionary();
            var templateMatcher = new TemplateMatcher(TemplateParser.Parse(routeTemplate), routeValues);
            return templateMatcher.TryMatch(requestPath, new RouteValueDictionary());
        }

        /// <summary>
        /// 
        /// </summary>
        public static Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }
    }
}
