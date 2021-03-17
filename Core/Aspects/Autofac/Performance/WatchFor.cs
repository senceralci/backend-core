using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Core.Aspects.Autofac.Performance
{
    /// <summary>
    /// PerformanceAspect
    /// </summary>
    public class WatchFor : MethodInterception
    {
        private readonly int _interval;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// İlgili metodun çalışma süresi parametre olarak alınan Interval (saniye)'den uzun sürerse uyar.
        /// </summary>
        /// <param name="interval"></param>
        public WatchFor(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }
        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                //todo: uyarı epostası gönder

                Debug.WriteLine($"Performance: {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}