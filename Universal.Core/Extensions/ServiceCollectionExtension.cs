using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Universal.Core
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 初始化注入程序集
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="serviceLifetime">服务的作用域</param>
        public static void AddAssembly(this IServiceCollection services, string assemblyName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services) + "为空");

            if (String.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName) + "为空");
            var assembly = AssemblyHelper.GetAssemblyByName(assemblyName);
            if (assembly == null)
                throw new DllNotFoundException(nameof(assembly) + ".dll不存在");
            var list = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericType).ToList();
            if (list == null && !list.Any())
                return;
            foreach (var type in list)
            {
                var interfacesList = type.GetInterfaces();
                if (interfacesList == null || !interfacesList.Any())
                    continue;
                var inter = interfacesList.First();
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type.GetInterfaces().First(), type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type.GetInterfaces().First(), type);
                        break;
                    case ServiceLifetime.Scoped:
                    default:
                        services.AddScoped(type.GetInterfaces().First(), type);
                        break;
                }
            }
        }
    }
}
