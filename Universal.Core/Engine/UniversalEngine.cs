using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Universal.Core
{
    public class UniversalEngine : IEngine
    {
        private IServiceProvider _serviceProvider;


        public UniversalEngine(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 构建实例
        /// </summary>
        /// <typeparam name="T">接口类</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return this._serviceProvider.GetService<T>();
        }
    }
}
