﻿using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, params ICoreModule[] modules)
        {
            foreach (ICoreModule item in modules)
            {
                item.Load(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
