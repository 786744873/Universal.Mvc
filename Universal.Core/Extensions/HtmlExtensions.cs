using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    public static class HtmlExtensions
    {
        public static HtmlString RequiredSpan(this IHtmlHelper htmlHelper)
        {
            return new HtmlString(@"<span class='required-span text-danger'>*</span>");
        }
    }
}
