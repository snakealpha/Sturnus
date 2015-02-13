using System;
using System.Collections.Generic;
using Elecelf.Sturnus.Operators;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// A Expression is a description of a calculating process.
    /// For a uniary expression, it's constructed by a uniary operator and only one operand;
    /// for a binary expression, it's constructed by a binary operator and two operands: left operand and right operand, which are both expressions.
    /// A Operand is another express, which may be a constant, varible or a formula.
    /// </summary>
    public abstract class Expression
    {
        public Expression(string literal)
        {

        }

        protected double value;
        public abstract double Value
        {
            set;
            get;
        }

        public virtual double Calculate(IDictionary<string, double> context)
        {
            throw new NotImplementedException("The base class of Expression should not be used.");
        }

        public abstract bool Calculated
        {
            get;
        }

        public abstract void Reset();
    }

    /// <summary>
    /// ConstantExpression is a expression that describe a constant.
    /// </summary>
    public class ConstantExpression : Expression
    {
        public ConstantExpression(string literal)
            : base(literal)
        {
            value = double.Parse(literal);
        }

        public override double Value
        {
            get
            {
                return value;
            }
            set
            {
                throw new System.InvalidOperationException("Cannot change the value of a constant expression.");
            }
        }

        public override double Calculate(IDictionary<string, double> context)
        {
            return value;
        }

        public override bool Calculated
        {
            get
            {
                return true;
            }
        }

        public override void Reset() { }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// VaribleExpression is a expression that describe a varible, which can be changed by passing arguments when a expression is excuted.
    /// </summary>
    public class VaribleExpression : Expression
    {
        private bool calculated = false;
        public override bool Calculated
        {
            get
            {
                return calculated;
            }
        }

        private string literal;
        public string Literal
        {
            get
            {
                return literal;
            }
        }

        public VaribleExpression(string literal)
            : base(literal)
        {
            this.literal = literal;
        }

        public override double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                calculated = true;
            }
        }

        public override double Calculate(IDictionary<string, double> context)
        {
            if (context.ContainsKey(literal))
                value = context[literal];

            return value;
        }

        public override void Reset()
        {
            calculated = false;
        }

        public override string ToString()
        {
            return Literal;
        }
    }

    /// <summary>
    /// FormulaExpression is a expression that contains operators and operands.
    /// In most cases, FormulaExpression is the main body of a whole expression.
    /// </summary>
    public class FormulaExpression : Expression
    {
        public FormulaExpression(string literal)
            : base(literal)
        {
            
        }

        private bool calculated = false;

        public Operator ExpressionOperator;

        public Expression LeftOperand;

        public Expression RightOperand;

        public override double Value
        {
            get
            {
                return value;
            }
            set
            {
                throw new InvalidOperationException("Cannot set a value for a FormulaExpression. Just calculate it.");
            }
        }

        public override double Calculate(IDictionary<string, double> context)
        {
            value = ExpressionOperator.Type == OperatorType.UniaryOperator ?
                    ExpressionOperator.Execute(0, RightOperand.Calculate(context)) :
                    ExpressionOperator.Execute(LeftOperand.Calculate(context), RightOperand.Calculate(context));

            return value;
        }

        public override bool Calculated
        {
            get
            {
                return calculated;
            }
        }

        public override void Reset()
        {
            if (LeftOperand != null)
                LeftOperand.Reset();
            if (RightOperand != null)
                RightOperand.Reset();

            calculated = false;
        }

        public override string ToString()
        {
            return ExpressionOperator.OperatorLiteral;
        }
    }

    /// <summary>
    /// FunctionExpression is a expression that contains a function object and its arguments.
    /// </summary>
    public class FunctionExpression : Expression
    {

    }
}
