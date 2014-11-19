using System;
using System.Collections.Generic;

namespace Elecelf.Sturnus
{
    public class OperatorContext
    {
        private Dictionary<string, Type> uniaryOperators = new Dictionary<string, Type>();
        private Dictionary<string, Type> binaryOperators = new Dictionary<string, Type>();

        public Dictionary<string, Type> UniaryOperators
        {
            get
            {
                return uniaryOperators;
            }
        }

        public Dictionary<string, Type> BinaryOperators
        {
            get
            {
                return binaryOperators;
            }
        }

        public OperatorContext(List<Type> binaryOperators, List<Type> uniaryOperators)
        {
            foreach (var type in binaryOperators)
            {
                Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                BinaryOperators[instance.OperatorLiteral] = type;
            }

            foreach (var type in uniaryOperators)
            {
                Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                UniaryOperators[instance.OperatorLiteral] = type;
            }
        }
    }
}
