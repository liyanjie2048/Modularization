using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.TemplateMatching;

namespace Liyanjie.Modularization.AspNet
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
            return templateMatcher.TryMatch(requestPath, routeValues);
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
