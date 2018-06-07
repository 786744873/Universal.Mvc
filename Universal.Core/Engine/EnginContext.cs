using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Universal.Core
{
    /// <summary>
    /// 引擎上下文
    /// </summary>
    public class EnginContext
    {
        private static IEngine _engine;

        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IEngine engine)
        {
            if (_engine==null)
            {
                _engine = engine;
            }
            return _engine;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IEngine Current
        {
            get {
                return _engine;
            }
        }
    }
}
