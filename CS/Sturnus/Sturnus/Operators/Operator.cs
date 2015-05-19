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

    public enum OperatorAssociativity
    {
        Left,
        Right
    }

    /// <summary>
    /// A operator is a operation that will be executed when a expression with it will be calculate.
    /// </summary>
    public abstract class Operator
    {
        public abstract string OperatorLiteral
        {
            get;
        }

        public abstract OperatorType Type
        {
            get;
        }

        public abstract uint Priority
        {
            get;
        }

        public virtual OperatorAssociativity Associativity
        {
            get
            {
                return OperatorAssociativity.Left;
            }
        }

        public int EscalateTime;

        public long Weight
        {
            get
            {
                return EscalateTime * 100 + Priority;
            }
        }

        public abstract double Execute(double leftOperand, double rightOperand);
    }
}
