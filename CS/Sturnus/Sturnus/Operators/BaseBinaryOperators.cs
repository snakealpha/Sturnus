using System;

namespace Elecelf.Sturnus.Operators
{
    public class AddOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "+";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 8;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return leftOperand + rightOperand;
        }
    }

    public class MinusOperator : Operator
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
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 8;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return leftOperand - rightOperand;
        }
    }

    public class MultiplyOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "*";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 9;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return leftOperand * rightOperand;
        }
    }

    public class DivideOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "/";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 9;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return leftOperand / rightOperand;
        }
    }

    public class ModOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "%";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 9;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return (int)leftOperand % (int)rightOperand;
        }
    }

    public class PowerOperator : Operator
    {
        public override string OperatorLiteral
        {
            get
            {
                return "^";
            }
        }

        public override OperatorType Type
        {
            get
            {
                return OperatorType.BinaryOperator;
            }
        }

        public override uint Priority
        {
            get
            {
                return 10;
            }
        }

        public override double Execute(double leftOperand, double rightOperand)
        {
            return Math.Pow(leftOperand, rightOperand);
        }
    }
}
