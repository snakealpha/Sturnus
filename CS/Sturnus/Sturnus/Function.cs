using System.Collections.Generic;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// The public baseclass of all Sturnus functions, which can be used in a expression.
    /// </summary>
    public abstract class Function
    {
        /// <summary>
        /// The Name to use in a expression.
        /// Notice: In a Sturnus expression, a function starts it's name with a char '_', but this '_' should not be included in the function's Name property.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// The logical body of the function.
        /// This method will be called when a FunctionExpression is calculated.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public abstract double Excute(List<Expression> arguments, IDictionary<string, double> context);

        public override string ToString()
        {
            return Name;
        }
    }
}
