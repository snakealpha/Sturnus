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

        public List<Type> BinaryOperatorTypes
        {
            set
            {
                foreach(var type in value)
                {
                    Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                    BinaryOperators[instance.OperatorLiteral] = type;
                }
            }
        }

        public List<Type> UniaryOperatorTypes
        {
            set
            {
                foreach (var type in value)
                {
                    Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                    UniaryOperators[instance.OperatorLiteral] = type;
                }
            }
        }
    }
}
