using System;
using System.Collections.Generic;
using Elecelf.Sturnus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SturnusUnitTest
{
    [TestClass]
    public class SturnusUnitTest
    {
        [TestMethod]
        public void ParseUseCase()
        {
            Expression plus = Parser.Parse(" 1 + 2 ");
            double plusRes = plus.Calculate();
            Assert.AreEqual(plusRes, 3, "Plus Operator");

            Expression multiply = Parser.Parse(" 1 + 2 * 3");
            double multiplyRes = multiply.Calculate();
            Assert.AreEqual(multiplyRes, 7, "Plus and multi Operator");

            Expression power = Parser.Parse(" 1 * 2 + 3 * 4 ^ 5");
            double powerRes = power.Calculate();
            Assert.AreEqual(powerRes, 3074, "Plus, multi and power Operator");

            Expression withVarible = Parser.Parse(" 1 + {test} * 3 ");
            Dictionary<string, double> argu = new Dictionary<string, double>()
            {
                {"test", 2.2}
            };
            double withVaribleRes = withVarible.Calculate(argu);
            Assert.IsTrue(Math.Abs(withVaribleRes - 7.6) <= 7.6 * .00001, "Varible Operator");

            Dictionary<string, double> calculateArgu = new Dictionary<string, double>()
            {
                {"test", 2.2}
            };
            double calculateVaribleRes = Sturnus.Calculate(" 1 + {test} * 3 ", null, calculateArgu);
            Assert.IsTrue(Math.Abs(calculateVaribleRes - 7.6) <= 7.6 * .00001, "Calculate Method");

            Dictionary<string, double> calculateArgu2 = new Dictionary<string, double>()
            {
                {"test", 2.2}
            };
            Dictionary<string, Expression> calculateExpressions = new Dictionary<string, Expression>()
            {
                {"test2", Sturnus.Parse("9")}
            };
            double calculateVarible2Res = Sturnus.Calculate(" 1 + {test} * 3 + {test2}", null, calculateExpressions, calculateArgu2);
            Assert.IsTrue(Math.Abs(calculateVarible2Res - 16.6) <= 16.6 * .00001, "Calculate Method 2");

            Expression uniaryOperator = Parser.Parse(" -1 + 2 * Abs - 3 ");
            double uniaryOperatorRes = uniaryOperator.Calculate();
            Assert.AreEqual(uniaryOperatorRes, 5, "Uniary Operator");

            Expression bracket = Parser.Parse(" -(1 * ( 2 + 3 )) ");
            double bracketRes = bracket.Calculate();
            Assert.AreEqual(bracketRes, -5, "Bracket Operator");

            Expression test = Sturnus.Parse("1+-2*3/4-2^2");
            double testRes = test.Calculate();
            Assert.AreEqual(testRes, -4.5, "Mix Operator");

            Expression functionTest = Sturnus.Parse("Sum[{argu1}+1, 3+5, 7*{argu2}]");
            var functionRes = functionTest.Calculate(new Dictionary<string, double>()
            {
                {"argu1", 1},
                {"argu2", 2},
            });
            Assert.AreEqual(functionRes, 24, "Function Operator");
        }
    }
}
