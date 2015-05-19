using System.Collections.Generic;

namespace Elecelf.Sturnus
{
    public abstract class Sturnus
    {
        public static Expression Parse(string expression, OperatorContext operatorContext = null)
        {
            return Parser.Parse(expression, operatorContext);
        }

        public static double Calculate(string expression, OperatorContext operatorContext = null, IDictionary<string, double> context = null)
        {
            return Parse(expression, operatorContext).Calculate(context);
        }

        public static double Calculate( string expression, 
                                        OperatorContext operatorContext, 
                                        IDictionary<string, Expression> expressionContext, 
                                        IDictionary<string, double> globalContext)
        {
            IDictionary<string, double> context = globalContext ?? new Dictionary<string, double>();
            foreach(var expKey in expressionContext)
            {
                context[expKey.Key] = expKey.Value.Calculate(context);
            }

            return Calculate(expression, operatorContext, context);
        }
    }
}
