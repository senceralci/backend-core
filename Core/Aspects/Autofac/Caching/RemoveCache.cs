using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    public class RemoveCache : MethodInterception
    {
        private readonly string _pattern;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Metot sonlandığında pattern' a uyan cache değerlerini siler.
        /// </summary>
        /// <param name="pattern"></param>
        public RemoveCache(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}