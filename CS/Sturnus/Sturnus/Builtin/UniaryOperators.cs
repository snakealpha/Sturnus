using System;
using Elecelf.Sturnus.Operators;

namespace Elecelf.Sturnus.Builtin
{
    public class NegativeOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "-";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.UniaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 12;
            }
        }

        public override OperatorAssociativity Associativity
        {
            get
            {
                return OperatorAssociativity.Right;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return - rightOperand;
        }
    }

    public class AbsoluteOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "Abs";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.UniaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 12;
            }
        }

        public override OperatorAssociativity Associativity
        {
            get
            {
                return OperatorAssociativity.Right;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return Math.Abs(rightOperand);
        }
    }
}
