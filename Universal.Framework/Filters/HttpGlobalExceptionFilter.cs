using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Universal.Entities;
using Universal.Services;

namespace Universal.Framework.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private ILoggerFactory _loggerFactory;
        private IHostingEnvironment _hostingEnvironment;
        private ISysLogService _sysLogService;

        public HttpGlobalExceptionFilter(ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, ISysLogService sysLogService)
        {
            _loggerFactory = loggerFactory;
            _hostingEnvironment = hostingEnvironment;
            _sysLogService = sysLogService;
        }

        public void OnException(ExceptionContext context)
        {
            var logger = _loggerFactory.CreateLogger(context.Exception.TargetSite.ReflectedType);
            logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);
            var json = new ErrorResponse("未知异常，请重试");
            //if (_hostingEnvironment.IsDevelopment())
            //{
            //    json.DeveloperMessage = context.Exception;
            //}
            //context.Result =new ApplicationErrorResult(json);
            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.ExceptionHandled = false;

            SysLog sysLog = new SysLog()
            {
                Level = 40,
                ShortMessage = context.Exception.Message,
                FullMessage = context.Exception.StackTrace,
                IpAddress = context.HttpContext.Connection.LocalIpAddress.ToString(),
                PageUrl = context.HttpContext.Request.Host+context.HttpContext.Request.Path,
                ReferrerUrl = context.HttpContext.Request.Headers["Referer"].ToString(),
                CreationTime = DateTime.Now
            };
            _sysLogService.InsertSysLog(sysLog);

        }

        public class ErrorResponse
        {
            public ErrorResponse(string message)
            {
                this.Message = message;
            }

            public string Message { get; set; }
            public object DeveloperMessage { get; set; }
        }

        public class ApplicationErrorResult : ObjectResult
        {
            public ApplicationErrorResult(object value) : base(value)
            {
                base.StatusCode = (int)HttpStatusCode.InternalServerError;

            }
        }
    }
}
