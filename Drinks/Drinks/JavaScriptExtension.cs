using System;
using System.Web;
using System.Web.Mvc;

public static class JavaScriptExtension
{
    public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filePath)
    {
        string dateString = "?d=" + DateTime.Now.ToString("yyyyMMddhh");
        return MvcHtmlString.Create("<script type='text/javascript' src='" + VirtualPathUtility.ToAbsolute(filePath) + dateString + "'></script>");
    }
}