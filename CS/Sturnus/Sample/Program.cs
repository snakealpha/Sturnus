using System;
using Elecelf.Sturnus;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression plus = Parser.Parse(" 1 + 2 ");

            Expression multiply = Parser.Parse(" 1 + 2 * 3");

            Expression power = Parser.Parse(" 1 * 2 + 3 * 4 ^ 5");

            Expression withVarible = Parser.Parse(" 1 + {test} * 3 ");

            Expression uniaryOperator = Parser.Parse(" -1 + 2 * Abs - 3 ");

            Expression bracket = Parser.Parse(" -(1 * ( 2 + 3 )) ");
        }
    }
}
