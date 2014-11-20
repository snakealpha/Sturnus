using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Elecelf.Sturnus.Operators;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// Parser is the class that used to translate a string to a expression tree.
    /// </summary>
    public abstract class Parser
    {
        [Flags]
        enum ExpectType
        {
            None = 0,
            LeftBracket = 1,
            RightBracket = 2,
            UniaryOperator = 4,
            BinaryOperator = 8,
            Operand = 16
        }

        class WrappedExpression<T> where T:Expression
        {
            public T Payload;
            public bool IsCaptured = false;

            public WrappedExpression<T> Capture()
            {
                IsCaptured = true;
                return this;
            }
        }

        public const string VariableFirstChars =    "_" +
                                                    "abcdefghijklmnopqrstuvwxyz" +
                                                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string VariableChars = "_" +
                                            "0123456789" +
                                            "abcdefghijklmnopqrstuvwxyz" +
                                            "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string NumberChars = "0123456789";

        public const string escalateUpgradation = "(";
        public const string escalateDowngradation = ")";

        public static OperatorContext DefaultOperatorContext = new OperatorContext(
                    new List<Type>()
                    {
                        typeof(AddOperator),
                        typeof(MinusOperator),
                        typeof(MultiplyOperator),
                        typeof(DivideOperator),
                        typeof(ModOperator),
                        typeof(PowerOperator)
                    },

                    new List<Type>() 
                    {
                        typeof(NegativeOperator),
                        typeof(AbsoluteOperator)
                    });

        /// <summary>
        /// Parse a string into a expression tree.
        /// </summary>
        /// <param name="expression">Raw string of the expression tree.</param>
        /// <param name="context"> Context of operators.</param>
        /// <returns></returns>
        public static Expression Parse(string expression, OperatorContext context = null)
        {
            // raw expressions that has not been captured by other expressions.
            Queue<Expression> operatandsQueue = new Queue<Expression>();
            // raw expressions that has not captured enough operatands.
            Queue<Expression> subexpressionQueue = new Queue<Expression>();

            // chars of expression raw chars.
            LinkedList<char> expressionChars = new LinkedList<char>();
            // expression string without space chars.
            string processedExpression = expression.Replace(" ", "");
            foreach(var sign in processedExpression)
            {
                expressionChars.AddLast(sign);
            }

            OperatorContext opContext = context == null ? DefaultOperatorContext : context;

             // Do parse.
            int bracketDepth = 0;
            ExpectType expect = ExpectType.LeftBracket | ExpectType.UniaryOperator | ExpectType.Operand;
            Expression currentExpression;
            do
            {
                if((expect & ExpectType.LeftBracket) != 0)
                {
                    if(expressionChars.First.Value == '(')
                    {
                        bracketDepth++;
                        expressionChars.RemoveFirst();
                        expect = ExpectType.LeftBracket | ExpectType.UniaryOperator | ExpectType.Operand;
                        continue;
                    }
                }

                if((expect & ExpectType.RightBracket) != 0 && bracketDepth > 0)
                {
                    if(expressionChars.First.Value == ')')
                    {
                        bracketDepth--;
                        expressionChars.RemoveFirst();
                        expect = ExpectType.RightBracket | ExpectType.BinaryOperator;
                        continue;
                    }
                }

                if((expect & ExpectType.UniaryOperator) != 0)
                {
                    currentExpression = parseUniaryOperator(expressionChars, bracketDepth, opContext);
                    if(currentExpression != null)
                    {
                        expect = ExpectType.UniaryOperator | ExpectType.LeftBracket | ExpectType.Operand;
                        subexpressionQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                if((expect & ExpectType.BinaryOperator) != 0)
                {
                    currentExpression = parseBinaryOperator(expressionChars, bracketDepth, opContext);
                    if(currentExpression != null)
                    {
                        expect = ExpectType.LeftBracket | ExpectType.Operand | ExpectType.UniaryOperator;
                        subexpressionQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                if((expect & ExpectType.Operand) != 0)
                {
                    currentExpression = parseConstant(expressionChars);
                    if(currentExpression != null)
                    {
                        expect = ExpectType.BinaryOperator | ExpectType.RightBracket;
                        operatandsQueue.Enqueue(currentExpression);
                        continue;
                    }

                    currentExpression = parseVarible(expressionChars);
                    if(currentExpression != null)
                    {
                        expect = ExpectType.BinaryOperator | ExpectType.RightBracket;
                        operatandsQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                throw new FormatException("Cannot parse the expression.");
            } while (expressionChars.Count > 0);

            return Assemble(operatandsQueue, subexpressionQueue);
        }

        public static Expression parseConstant(LinkedList<char> signs)
        {
            bool dotAppeared = false;
            char peekChar;
            StringBuilder numRawStr = new StringBuilder();

            while (true)
            {
                if (signs.Count == 0)
                    break;

                peekChar = signs.First.Value;

                if (NumberChars.IndexOf(peekChar) != -1)
                {
                    numRawStr.Append(peekChar);
                    signs.RemoveFirst();
                }
                else if (!dotAppeared && peekChar == '.')
                {
                    dotAppeared = true;
                    numRawStr.Append(peekChar);
                    signs.RemoveFirst();
                    peekChar = signs.First.Value;
                    if (NumberChars.IndexOf(peekChar) == -1)
                        throw new FormatException("Cannot parse a Number.");
                }
                else
                {
                    break;
                }
            }

            if (numRawStr.Length == 0)
                return null;

            ConstantExpression constant = new ConstantExpression(numRawStr.ToString());
            return constant;
        }

        public static Expression parseVarible(LinkedList<char> signs)
        {
            char peekChar = signs.First.Value;
            StringBuilder varRawStr = new StringBuilder();

            while(true)
            {
                signs.RemoveFirst();
                if (peekChar == '{')
                    continue;
                else if (peekChar == '}')
                    break;

                varRawStr.Append(peekChar);
                peekChar = signs.First.Value;
            }

            VaribleExpression varible = new VaribleExpression(varRawStr.ToString());
            return varible;
        }

        public static Expression parseUniaryOperator(LinkedList<char> signs, int escalateTime, OperatorContext context)
        {
            Operators.Operator lastOperator = null;
            IEnumerable<string> operatorNames = context.UniaryOperators.Keys;
            StringBuilder operatorRawStr = new StringBuilder();

            do
            {
                operatorRawStr.Append(signs.First.Value);
                string currentStr = operatorRawStr.ToString();

                var lastMatches =
                    (
                    from name in operatorNames
                    where name.StartsWith(currentStr)
                    select name
                    ).ToArray();

                if(lastMatches.Length == 0)
                {
                    break;
                }
                else
                {
                    if (context.UniaryOperators.ContainsKey(currentStr))
                    {
                        lastOperator = Activator.CreateInstance(context.UniaryOperators[currentStr]) as Operators.Operator;
                        lastOperator.EscalateTime = escalateTime;
                    }

                    signs.RemoveFirst();
                }
            }
            while (true);

            if (lastOperator != null)
                return new FormulaExpression(null)
                {
                    ExpressionOperator = lastOperator
                };
            else
                return null;
        }

        public static Expression parseBinaryOperator(LinkedList<char>signs, int escalateTime, OperatorContext context)
        {
            Operators.Operator lastOperator = null;
            IEnumerable<string> operatorNames = context.BinaryOperators.Keys;
            StringBuilder operatorRawStr = new StringBuilder();

            do
            {
                operatorRawStr.Append(signs.First.Value);
                string currentStr = operatorRawStr.ToString();

                var lastMatches =
                    (
                    from name in operatorNames
                    where name.StartsWith(currentStr)
                    select name
                    ).ToArray();

                if (lastMatches.Length == 0)
                {
                    break;
                }
                else
                {
                    if (context.BinaryOperators.ContainsKey(currentStr))
                    {
                        lastOperator = Activator.CreateInstance(context.BinaryOperators[currentStr]) as Operators.Operator;
                        lastOperator.EscalateTime = escalateTime;
                    }

                    signs.RemoveFirst();
                }
            }
            while (true);

            if (lastOperator != null)
                return new FormulaExpression(null)
                {
                    ExpressionOperator = lastOperator
                };
            else
                return null;
        }

        public static Expression Assemble(Queue<Expression> operands, Queue<Expression> operators)
        {
            Stack<WrappedExpression<Expression>> rawOperands = new Stack<WrappedExpression<Expression>>
                (
                    (
                    from operand in operands
                    select new WrappedExpression<Expression>() { Payload = operand }
                    ).Reverse()
                );
            LinkedList<WrappedExpression<FormulaExpression>> wrappedOperators = new LinkedList<WrappedExpression<FormulaExpression>>
                (
                from oper in operators
                select new WrappedExpression<FormulaExpression>() { Payload = oper as FormulaExpression }
                );

            LinkedListNode<WrappedExpression<FormulaExpression>> currentNode = wrappedOperators.First;
            FormulaExpression currentExpression = currentNode.Value.Payload;

            while(wrappedOperators.Count > 0)
            {
                bool captureRight = false;

                if( currentNode.Next != null &&
                    (    
                        wrappedOperators.Count > 0 &&
                        currentExpression.ExpressionOperator.Associativity == OperatorAssociativity.Left && 
                        currentExpression.ExpressionOperator.Weight < (currentNode.Next.Value.Payload as FormulaExpression).ExpressionOperator.Weight
                    ) || (
                        wrappedOperators.Count > 0 &&
                        currentExpression.ExpressionOperator.Associativity == OperatorAssociativity.Right &&
                        currentExpression.ExpressionOperator.Weight <= (currentNode.Next.Value.Payload as FormulaExpression).ExpressionOperator.Weight
                    )
                )
                {
                    captureRight = true;
                }

                if(captureRight)
                {
                    currentExpression.RightOperand = currentNode.Next.Value.Capture().Payload;
                }

                if(currentExpression.ExpressionOperator.Type == OperatorType.BinaryOperator)
                {
                    currentExpression.LeftOperand = rawOperands.Pop().Payload;
                }

                if(currentExpression.RightOperand == null)
                {
                    currentExpression.RightOperand = rawOperands.Pop().Payload;
                }

                if (currentNode.Next != null)
                {
                    currentNode = currentNode.Next;
                    currentExpression = currentNode.Value.Payload;
                }
                else
                {
                    rawOperands.Push(new WrappedExpression<Expression>() { Payload = currentNode.Value.Payload });
                    wrappedOperators.Remove(currentNode);
                }
               

                if(currentNode.Previous != null)
                {
                    if (currentNode.Previous.Value.IsCaptured)
                        wrappedOperators.Remove(currentNode.Previous);

                    if (currentNode.Previous.Value.Payload.ExpressionOperator.Weight <= currentNode.Value.Payload.ExpressionOperator.Weight)
                    {
                        rawOperands.Push(new WrappedExpression<Expression>() { Payload = currentNode.Previous.Value.Payload });
                        wrappedOperators.Remove(currentNode.Previous);
                    }
                }
            }

            return rawOperands.Pop().Payload;
        }
    }
}
