using System;
using System.Collections.Generic;
using System.Linq;

namespace Elecelf.Sturnus.Builtin
{
    /// <summary>
    /// Get the absoluted value of a expression.
    /// </summary>
    public class AbsFunction : Function
    {
        public override string Name
        {
            get { return "Abs"; }
        }

        public override double Excute(List<Expression> arguments, IDictionary<string, double> context)
        {
            return Math.Abs(arguments[0].Calculate(context));
        }
    }

    /// <summary>
    /// Get the sum value of numbers.
    /// </summary>
    public class SumFunction : Function
    {
        public override string Name
        {
            get { return "Sum"; }
        }

        public override double Excute(List<Expression> arguments, IDictionary<string, double> context)
        {
            return arguments.Sum(expression => expression.Calculate(context));
        }
    }

    /// <summary>
    /// Get the maximum value of numbers.
    /// </summary>
    public class MaxFunction : Function
    {
        public override string Name
        {
            get { return "Max"; }
        }

        public override double Excute(List<Expression> arguments, IDictionary<string, double> context)
        {
            return arguments.Select(expression => expression.Calculate(context)).Concat(new[] {double.MaxValue}).Max();
        }
    }

    /// <summary>
    /// Get the Minimum value of numbers.
    /// </summary>
    public class MinFunction : Function
    {
        public override string Name
        {
            get { return "Min"; }
        }

        public override double Excute(List<Expression> arguments, IDictionary<string, double> context)
        {
            return arguments.Select(expression => expression.Calculate(context)).Concat(new[] {double.MaxValue}).Min();
        }
    }

    /// <summary>
    /// If the first expression's value equals or larger than zero, return the second value; otherwise return the third.
    /// </summary>
    public class IfFunction:Function
    {
        public override string Name
        {
            get { return "If"; }
        }

        public override double Excute(List<Expression> arguments, IDictionary<string, double> context)
        {
            if(arguments.Count != 3)
                throw new InvalidOperationException("If Function max has three arguments.");

            if (arguments[0].Calculate(context) > 0)
            {
                return arguments[1].Calculate(context);
            }
            else
            {
                return arguments[2].Calculate(context);
            }
        }
    }
}
