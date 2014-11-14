using System;
using System.Collections.Generic;

namespace Elecelf.Sturnus
{
    public class OperatorContext
    {
        private Dictionary<string, Operators.Operator> uniaryOperators = new Dictionary<string, Operators.Operator>();
        private Dictionary<string, Operators.Operator> binaryOperators = new Dictionary<string, Operators.Operator>();

        public Dictionary<string, Operators.Operator> UniaryOperators
        {
            get
            {
                return uniaryOperators;
            }
        }

        public Dictionary<string, Operators.Operator> BinaryOperators
        {
            get
            {
                return binaryOperators;
            }
        }
    }
}
