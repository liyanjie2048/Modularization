using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModularizationModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<bool> TryMatchRequestingAsync(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        Task HandleResponsingAsync(HttpContext httpContext);
    }
}
