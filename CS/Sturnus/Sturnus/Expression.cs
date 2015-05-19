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

    public class ConstantExpression : Expression
    {
        private readonly double value;

        public ConstantExpression(string literal)
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
                throw new InvalidOperationException("Cannot change the value of a constant expression.");
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

    public class VaribleExpression : Expression
    {
        private bool calculated;
        public override bool Calculated
        {
            get
            {
                return calculated;
            }
        }

        private readonly string literal;
        public string Literal
        {
            get
            {
                return literal;
            }
        }

        public VaribleExpression(string literal)
        {
            this.literal = literal;
        }

        private double value;
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

    public class FormulaExpression : Expression
    {
        private bool calculated;

        public Operator ExpressionOperator;

        public Expression LeftOperand;

        public Expression RightOperand;

        private double value;
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
}
