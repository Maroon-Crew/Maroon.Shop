using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace Maroon.Shop.Api.Controllers
{
    public static class UrlHelperExtensions
    {
        public static string? ActionLink<TController>(this IUrlHelper urlHelper, string action, object? values = null) where TController : Controller
        {
            return urlHelper.ActionLink(action, typeof(TController).Name.Replace("Controller", ""), values);
        }

        // does not work
        public static string? ActionLink<TController, TResult>(this IUrlHelper urlHelper, Expression<Func<TController, ActionResult<TResult>>> actionSelector) where TController : Controller
        {
            var methodInfo = GetMethodInfo(actionSelector);
            var values = GetMethodValues(actionSelector);
            return urlHelper.ActionLink(methodInfo.Name, typeof(TController).Name.Replace("Controller", ""), values);
        }

        private static MemberInfo GetMethodInfo<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            return callExpression.Method;
        }

        private static object? GetMethodValues<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            var query = from memberExpression in callExpression.Arguments.OfType<MemberExpression>()
                        where memberExpression.Expression.NodeType == ExpressionType.Constant
                        let constantExpression = (ConstantExpression)memberExpression.Expression
                        let parameterName = memberExpression.Member.Name
                        let constantValue = constantExpression.Value
                        select memberExpression.Member.ReflectedType.GetField(parameterName).GetValue(constantValue);

            return query.FirstOrDefault();
        }
    }
}