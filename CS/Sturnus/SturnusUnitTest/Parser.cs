using System;
using Elecelf.Sturnus;
using Elecelf.Sturnus.Builtin;
using Elecelf.Sturnus.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SturnusUnitTest
{
    [TestClass]
    public class ParserUnitTest
    {
        class InversedAdd : AddOperator
        {
            public override OperatorAssociativity Associativity { get{return OperatorAssociativity.Right;} }

            public override string OperatorLiteral {
                get { return "~"; }
            }
        }

        [TestMethod]
        public void ParserTestMethod()
        {
            bool excepted = false;

            var wrapped = new Parser.WrappedExpression<ConstantExpression>();
            wrapped.Payload = new ConstantExpression("3.14");
            Assert.AreEqual(wrapped.ToString(), wrapped.Payload.ToString(), "3.14");

            var exp = Parser.Parse("361.");
            Assert.AreEqual(exp.Calculate(), 361);

            try
            {
                Parser.Parse("361.a");
            }
            catch (FormatException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            excepted = false;

            try
            {
                Parser.Parse("9+6*");
            }
            catch (InvalidOperationException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            excepted = false;

            try
            {
                Parser.Parse("9+6`");
            }
            catch (FormatException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);
            excepted = false;

            try
            {
                Parser.Parse("Ann[9.36, 7.28]");
            }
            catch (InvalidOperationException)
            {
                excepted = true;
            }
            Assert.IsTrue(excepted);

            var context = Context.GetDefaultContext();
            context.BinaryOperators["~"] = typeof(InversedAdd);
            var iaExpression = Sturnus.Parse("2*3~10", context);
            Assert.AreEqual(iaExpression.Calculate(), 16);
        }
    }
}
