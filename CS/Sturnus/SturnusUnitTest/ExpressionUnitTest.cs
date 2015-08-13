using System;
using System.Collections.Generic;
using Elecelf.Sturnus;
using Elecelf.Sturnus.Builtin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SturnusUnitTest
{
    [TestClass]
    public class ExpressionUnitTest
    {
        [TestMethod]
        public void ExpressionUseCases()
        {
            bool excepted = false;

            var constant = new ConstantExpression("19.95");
            Assert.AreEqual(constant.Value, constant.Calculate(null), 19.95);
            Assert.IsTrue(constant.Calculated);
            Assert.IsTrue(constant.ToString() == "19.95");
            try
            {
                constant.Value = 16.65;
            }
            catch (InvalidOperationException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            excepted = false;

            var varible = new VaribleExpression("{test}");
            Assert.AreEqual(varible.ToString(), varible.Literal, "{test}");
            Assert.IsFalse(varible.Calculated);
            var varValue = varible.Calculate(new Dictionary<string, double>() {{"test", 15.5}});
            Assert.IsTrue(varible.Calculated);
            Assert.AreEqual(varValue, varible.Value, 15.5);
            varible.Reset();
            Assert.IsFalse(varible.Calculated);
            varible.Value = 15.5;
            Assert.IsTrue(varible.Calculated);
            Assert.AreEqual(varValue, varible.Value, 15.5);

            var formula = new FormulaExpression();
            formula.ExpressionOperator = new AddOperator();
            formula.LeftOperand = new ConstantExpression("14.5");
            formula.RightOperand = new ConstantExpression("15.5");
            Assert.AreEqual(formula.ToString(), formula.ExpressionOperator.OperatorLiteral);
            Assert.IsFalse(formula.Calculated);
            var formulaValue = formula.Calculate(null);
            Assert.IsTrue(formula.Calculated);
            Assert.AreEqual(formulaValue, formula.Value, 20);
            try
            {
                formula.Value = 16.65;
            }
            catch (InvalidOperationException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            excepted = false;
            formula.Reset();
            Assert.IsFalse(formula.Calculated);

            var function = new FunctionExpression();
            function.Function = new MaxFunction();
            function.Operands.Add(new ConstantExpression("15.5"));
            function.Operands.Add(new ConstantExpression("14.5"));
            Assert.AreEqual(function.ToString(), function.Function.Name);
            Assert.IsFalse(function.Calculated);
            var functionValue = function.Calculate(null);
            Assert.IsTrue(function.Calculated);
            Assert.AreEqual(functionValue, function.Value, 20);
            try
            {
                function.Value = 16.65;
            }
            catch (InvalidOperationException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            function.Reset();
            Assert.IsFalse(function.Calculated);
        }
    }
}
