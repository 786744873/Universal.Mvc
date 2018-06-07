using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Universal.Framework.Menu
{
    public class FunctionManager
    {
        /// <summary>
        /// 获取 action 特性
        /// </summary>
        /// <returns></returns>
        public static List<FunctionAttribute> GetFunctionLists()
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Universal.Mvc"));
            List<FunctionAttribute> result = new List<FunctionAttribute>();
            Type[] types = assembly.GetTypes();
            if (types!=null)
            {
                foreach (Type type in types)
                {
                    string typeName = type.FullName.ToLower();
                    if (typeName.EndsWith("controller"))
                    {
                        IEnumerable<FunctionAttribute> funAttlist = type.GetCustomAttributes<FunctionAttribute>(false);
                        FunctionAttribute father = null;
                        if (funAttlist!=null&&funAttlist.Any())
                        {
                            foreach (FunctionAttribute fun in funAttlist)
                            {
                                if (string.IsNullOrEmpty(fun.SysResource))
                                {
                                    fun.SysResource = type.FullName;
                                }
                                father = fun;
                                result.Add(fun);
                                break;
                            }
                        }
                        //获取action方法
                        MemberInfo[] members = type.FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, Type.FilterName, "*");
                        if (members!=null&&members.Any())
                        {
                            foreach (MemberInfo member in members)
                            {
                                IEnumerable<FunctionAttribute> funs = member.GetCustomAttributes<FunctionAttribute>(false);
                                foreach (FunctionAttribute fun in funs)
                                {
                                    if (string.IsNullOrEmpty(fun.SysResource))
                                    {
                                        fun.SysResource = type.FullName + "." + member.Name;
                                    }
                                    fun.Controller = type.Name.Replace("Controller", "");
                                    fun.Action = member.Name;
                                    //如果父级未指定
                                    if (string.IsNullOrEmpty(fun.FatherResource))
                                    {
                                        if (father!=null)
                                        {
                                            fun.FatherResource = father.SysResource;
                                        }
                                    }
                                    object[] routes = member.GetCustomAttributes(typeof(RouteAttribute), false);
                                    if (routes!=null&&routes.Any())
                                    {
                                        var route = routes.First() as RouteAttribute;
                                        fun.RouteName = route.Name;
                                    }
                                    result.Add(fun);
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            return result;
        }
    }
}
