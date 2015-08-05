using System;
using System.Collections.Generic;
using Elecelf.Sturnus;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Expression plus = Parser.Parse(" 1 + 2 ");
            //double plusRes = plus.Calculate(null);

            //Expression multiply = Parser.Parse(" 1 + 2 * 3");
            //double multiplyRes = multiply.Calculate(null);

            //Expression power = Parser.Parse(" 1 * 2 + 3 * 4 ^ 5");
            //double powerRes = power.Calculate(null);

            //Expression withVarible = Parser.Parse(" 1 + {test} * 3 ");
            //Dictionary<string, double> argu = new Dictionary<string, double>();
            //argu["test"] = 2.2;
            //double withVaribleRes = withVarible.Calculate(argu);

            //Expression uniaryOperator = Parser.Parse(" -1 + 2 * Abs - 3 ");
            //double uniaryOperatorRes = uniaryOperator.Calculate(null);

            //Expression bracket = Parser.Parse(" -(1 * ( 2 + 3 )) ");
            //double bracketRes = bracket.Calculate(null);

            //Expression test = Sturnus.Parse("1+-2*3/4-2^2");
            //double testRes = test.Calculate(null);

            Expression constrantTest = Sturnus.Parse("99");
        }
    }
}
