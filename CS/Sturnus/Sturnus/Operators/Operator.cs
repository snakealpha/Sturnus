using System;

namespace Elecelf.Sturnus.Operators
{
    /// <summary>
    /// The type of a operator.
    /// UniaryOperator only uses its right operand, whereas BinaryOperator uses its all operands.
    /// </summary>
    public enum OperatorType
    {
        UniaryOperator,
        BinaryOperator
    }

    /// <summary>
    /// A operator is a operation that will be executed when a expression with it will be calculate.
    /// </summary>
    /// <typeparam name="T">The type of the delegate will be called.</typeparam>
    public abstract class Operator
    {
        public delegate double OperatorAlgorithm(double leftOperand, double rightOperand);

        public OperatorAlgorithm Algorithm;

        public virtual string OperatorLiteral
        {
            get;
        }

        public virtual OperatorType Type
        {
            get;
        }

        public virtual uint Priority
        {
            get;
        }

        public uint EscalateTime;

        public uint Weight
        {
            get
            {
                return EscalateTime * 100 + Priority;
            }
        }
    }
}
