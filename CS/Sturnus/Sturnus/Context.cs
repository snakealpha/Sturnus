using System;
using System.Collections.Generic;
using Elecelf.Sturnus.Builtin;
using Elecelf.Sturnus.Operators;

namespace Elecelf.Sturnus
{
    public class Context
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

        public Context(List<Type> binaryOperators, List<Type> uniaryOperators, List<Type> buildinFunctions)
        {
            foreach (var type in binaryOperators)
            {
                var instance = Activator.CreateInstance(type) as Operator;
                if (instance != null) BinaryOperators[instance.OperatorLiteral] = type;
            }

            foreach (var type in uniaryOperators)
            {
                var instance = Activator.CreateInstance(type) as Operator;
                if (instance != null) UniaryOperators[instance.OperatorLiteral] = type;
            }

            foreach (var type in buildinFunctions)
            {
                Function instance = Activator.CreateInstance(type) as Function;
                if (instance != null) BuildinFunctions[instance.Name] = type;
            }
        }

        public static Context GetDefaultContext()
        {
            return new Context(
                    new List<Type>()
                    {
                        typeof(AddOperator),
                        typeof(MinusOperator),
                        typeof(MultiplyOperator),
                        typeof(DivideOperator),
                        typeof(ModOperator),
                        typeof(PowerOperator),
                    },

                    new List<Type>() 
                    {
                        typeof(NegativeOperator),
                        typeof(AbsoluteOperator),
                    },

                    new List<Type>()
                    {
                        typeof(AbsFunction),
                        typeof(SumFunction),
                        typeof(MaxFunction),
                        typeof(MinFunction),
                        typeof(IfFunction),
                    }
                    );
        }
    }
}
