using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sample20130719
{
    class Program
    {
        static void Main()
        {
            var model = new Model();
            var executer = new Executer();
            Test3(executer, model, "Numbers");
            //f(executer, model);
        }

        static Action<Executer, object> Test2(Executer executer, object model, string property)
        {
            var modelType = model.GetType();
            var modelProperty = modelType.GetProperty(property);
            var calleeType = executer.GetType();
            var method = calleeType.GetMethod("Do").MakeGenericMethod(modelType, modelProperty.PropertyType.GetGenericArguments().First());

            // executer
            var executerParameter = Expression.Parameter(calleeType, "executer");

            // (T)x
            var modelParameter = Expression.Parameter(typeof(object), "x");
            var modelConversion = Expression.Convert(modelParameter, modelType);

            // y => y.Property
            var innerParameter = Expression.Parameter(modelType, "y");
            var lambdaParameter = Expression.Lambda(Expression.Property(innerParameter, modelProperty), innerParameter);

            // executer.Do((T)x, y => y.Numbers)
            var call = Expression.Call(executerParameter, method, modelConversion, lambdaParameter);

            // f = (Executer executer, object x) => executer.Do((T)x, y => y.Numbers)
            var lambda = Expression.Lambda(call, executerParameter, modelParameter);

            // f(executer, x)
            var f = (Action<Executer, object>)lambda.Compile();
            return f;
        }
        static void Test3(Executer executer, object model, string property)
        {
            var modelType = model.GetType();
            var modelProperty = modelType.GetProperty(property);
            var calleeType = executer.GetType();
            var method = calleeType.GetMethod("Do").MakeGenericMethod(modelType, modelProperty.PropertyType.GetGenericArguments().First());

            // executer
            var executerParameter = Expression.Parameter(calleeType, "executer");

            // x
            var modelParameter = Expression.Parameter(modelType, "x");

            // y => y.Property
            var innerParameter = Expression.Parameter(modelType, "y");
            var lambdaParameter = Expression.Lambda(Expression.Property(innerParameter, modelProperty), innerParameter);

            // Do(x, y => y.Numbers)
            var call = Expression.Call(executerParameter, method, modelParameter, lambdaParameter);

            // f = (executer, x) => executer.Do(x, y => y.Numbers)
            var lambda = Expression.Lambda(call, executerParameter, modelParameter);

            // f(executer, x)
            lambda.Compile().DynamicInvoke(executer, model);
        }
    }
}
