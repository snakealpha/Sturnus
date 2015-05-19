using System;
using System.Collections.Generic;

namespace Elecelf.Sturnus
{
    public class OperatorContext
    {
        private readonly Dictionary<string, Type> uniaryOperators = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> binaryOperators = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> buildinFunctions = new Dictionary<string, Type>();

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

        public Dictionary<string, Type> BuildinFunctions
        {
            get
            {
                return buildinFunctions;
            }
        }

        public OperatorContext(List<Type> binaryOperators, List<Type> uniaryOperators, List<Type> buildinFunctions)
        {
            foreach (var type in binaryOperators)
            {
                Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                if (instance != null) BinaryOperators[instance.OperatorLiteral] = type;
            }

            foreach (var type in uniaryOperators)
            {
                Operators.Operator instance = Activator.CreateInstance(type) as Operators.Operator;
                if (instance != null) UniaryOperators[instance.OperatorLiteral] = type;
            }
        }
    }
}
