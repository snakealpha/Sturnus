using System;
using Elecelf.Sturnus;
using Elecelf.Sturnus.Builtin;
using Elecelf.Sturnus.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SturnusUnitTest
{
    [TestClass]
    public class BuiltinUnitTest
    {
        [TestMethod]
        public void FunctionsUseCases()
        {
            FunctionExpression expression = new FunctionExpression();

            expression.Function = new IfFunction();
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("2"));
            Assert.AreEqual(expression.Calculate(), 1);
            expression.Operands[0] = new ConstantExpression("-1");
            Assert.AreEqual(expression.Calculate(), 2);

            expression.Operands.RemoveAt(0);
            bool exceptioned = false;
            try
            {
                expression.Calculate();
            }
            catch (InvalidOperationException)
            {
                exceptioned = true;
            }
            Assert.IsTrue(exceptioned);

            Assert.AreEqual(expression.Function.Name, expression.Function.ToString());

            expression.Function = new MaxFunction();
            expression.Operands.Clear();
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("2"));
            expression.Operands.Add(new ConstantExpression("3"));
            Assert.AreEqual(expression.Calculate(), 3);

            expression.Function = new MinFunction();
            expression.Operands.Clear();
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("1"));
            expression.Operands.Add(new ConstantExpression("2"));
            expression.Operands.Add(new ConstantExpression("3"));
            Assert.AreEqual(expression.Calculate(), 1);

            expression.Function = new AbsFunction();
            expression.Operands.Clear();
            expression.Operands.Add(new ConstantExpression("1"));
            Assert.AreEqual(expression.Calculate(), 1);
            expression.Operands.Clear();
            expression.Operands.Add(new ConstantExpression("-1"));
            Assert.AreEqual(expression.Calculate(), 1);
        }

        [TestMethod]
        public void OperatorUseCases()
        {
            FormulaExpression expression = new FormulaExpression();

            ModOperator mod = new ModOperator();
            Assert.IsTrue(mod.Type == OperatorType.BinaryOperator);
            Assert.IsTrue(mod.Priority == 9);
            Assert.AreEqual(mod.OperatorLiteral, "%");

            expression.ExpressionOperator = mod;
            expression.LeftOperand = new ConstantExpression("5");
            expression.RightOperand = new ConstantExpression("3");
            Assert.AreEqual(5%3, expression.Calculate());
        }
    }
}
