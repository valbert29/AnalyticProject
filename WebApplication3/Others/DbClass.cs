using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VSZANAL.Controllers;
using VSZANAL.Models;

namespace VSZANAL
{
    public class LoggingDecorator<T> : DispatchProxy
    {
        readonly RUNContext db;
        readonly HttpContext HtContext;
        LoggingDecorator(RUNContext context, HttpContext httpContext)
        {
            db = context;
            HtContext = httpContext;
        }
        private T _decorated;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                var result = targetMethod.Invoke(_decorated, args);

                //LogAfter(targetMethod, args, result);
                return result;
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
                throw ex.InnerException ?? ex;
            }
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, LoggingDecorator<T>>();
            ((LoggingDecorator<T>)proxy).SetParameters(decorated);

            return (T)proxy;
        }

        private void SetParameters(T decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }
            _decorated = decorated;
        }

        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} threw exception:\n{exception}");
        }

        private void LogAfter(MethodInfo methodInfo, object[] args, object result)
        {
            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} executed, Output: {result}");
        }

        private void LogBefore(MethodInfo methodInfo, object[] args)
        {
            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} is executing");
        }

        /*private bool Checker()
        {//тут надо посмотреть, есть ли у пользователя подписка с именем == имени метода, который тут вызывается.
            var user = HomeController.GetUser(db, HtContext);
            var sub = GetSubscription();
            user.Subscriptions.Contains();
        }*/

        private Subscription GetSubscription(string subName)
        {
            return db.Subscriptions.FirstOrDefault(u => u.Name == subName);
        }


    }

}
