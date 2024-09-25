using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace Maroon.Shop.Api.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult RedirectToAction<TController, TResult>(this ControllerBase controller, Expression<Func<TController, ActionResult<TResult>>> actionSelector) where TController : Controller
        {
            var methodInfo = GetMethodInfo(actionSelector);
            var values = GetMethodValues(actionSelector);
            return controller.RedirectToAction(methodInfo.Name, typeof(TController).Name.Replace("Controller", ""), values);
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

        public static IActionResult RedirectToAction<TController>(this ControllerBase controller, string action, object? values = null) where TController : Controller
        {
            return controller.RedirectToAction(action, typeof(TController).Name.Replace("Controller", ""), values);
        }
    }
}