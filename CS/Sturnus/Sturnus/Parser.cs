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

        public class WrappedExpression<T> where T:Expression
        {
            public T Payload;

            public bool CaptureLeft = false;
            public bool CaptureRight = false;

            public WrappedExpression<T> Capture()
            {
                return this;
            }

            public override string ToString()
            {
                return Payload.ToString();
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
                    },
                    
                    new List<Type>()
                    );

        /// <summary>
        /// Parse a string into a expression tree.
        /// </summary>
        /// <param name="expression">Raw string of the expression tree.</param>
        /// <param name="context"> Context of operators.</param>
        /// <returns></returns>
        public static Expression Parse(string expression, OperatorContext context = null)
        {
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
            Expression mainExpression = ParseExpression(expressionChars, 0, opContext);
            if(expressionChars.Count > 0)
            {
                throw new FormatException("Cannot parse the string.");
            }

            return mainExpression;
        }

        #region Parsers
        public static Expression ParseExpression(LinkedList<char> expressionChars, int baseBracketDepth, OperatorContext opContext)
        {
            // raw expressions that has not been captured by other expressions.
            Queue<Expression> operatandsQueue = new Queue<Expression>();
            // raw expressions that has not captured enough operatands.
            Queue<Expression> subexpressionQueue = new Queue<Expression>();

            // Do parse.
            int bracketDepth = baseBracketDepth;
            ExpectType expect = ExpectType.LeftBracket | ExpectType.UniaryOperator | ExpectType.Operand;
            Expression currentExpression;
            do
            {
                if ((expect & ExpectType.LeftBracket) != 0)
                {
                    if (expressionChars.First.Value == '(')
                    {
                        bracketDepth++;
                        expressionChars.RemoveFirst();
                        expect = ExpectType.LeftBracket | ExpectType.UniaryOperator | ExpectType.Operand;
                        continue;
                    }
                }

                if ((expect & ExpectType.RightBracket) != 0 && bracketDepth > 0)
                {
                    if (expressionChars.First.Value == ')')
                    {
                        bracketDepth--;
                        expressionChars.RemoveFirst();
                        expect = ExpectType.RightBracket | ExpectType.BinaryOperator;
                        continue;
                    }
                }

                if ((expect & ExpectType.UniaryOperator) != 0)
                {
                    currentExpression = ParseUniaryOperator(expressionChars, bracketDepth, opContext);
                    if (currentExpression != null)
                    {
                        expect = ExpectType.UniaryOperator | ExpectType.LeftBracket | ExpectType.Operand;
                        subexpressionQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                if ((expect & ExpectType.BinaryOperator) != 0)
                {
                    currentExpression = ParseBinaryOperator(expressionChars, bracketDepth, opContext);
                    if (currentExpression != null)
                    {
                        expect = ExpectType.LeftBracket | ExpectType.Operand | ExpectType.UniaryOperator;
                        subexpressionQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                if ((expect & ExpectType.Operand) != 0)
                {
                    currentExpression = ParseConstant(expressionChars);
                    if (currentExpression != null)
                    {
                        expect = ExpectType.BinaryOperator | ExpectType.RightBracket;
                        operatandsQueue.Enqueue(currentExpression);
                        continue;
                    }

                    currentExpression = ParseVarible(expressionChars);
                    if (currentExpression != null)
                    {
                        expect = ExpectType.BinaryOperator | ExpectType.RightBracket;
                        operatandsQueue.Enqueue(currentExpression);
                        continue;
                    }
                }

                break;
            } while (expressionChars.Count > 0);

            return Assemble(operatandsQueue, subexpressionQueue);
        }

        public static Expression ParseConstant(LinkedList<char> signs)
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

        public static Expression ParseVarible(LinkedList<char> signs)
        {
            char peekChar = signs.First.Value;
            StringBuilder varRawStr = new StringBuilder();

            while(true)
            {
                signs.RemoveFirst();
                if (peekChar == '{')
                {
                    peekChar = signs.First.Value;
                    continue;
                }
                else if (peekChar == '}')
                {
                    peekChar = signs.First.Value;
                    break;
                }

                varRawStr.Append(peekChar);
                peekChar = signs.First.Value;
            }

            VaribleExpression varible = new VaribleExpression(varRawStr.ToString());
            return varible;
        }

        public static Expression ParseUniaryOperator(LinkedList<char> signs, int escalateTime, OperatorContext context)
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

        public static Expression ParseBinaryOperator(LinkedList<char>signs, int escalateTime, OperatorContext context)
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
        #endregion

        public static Expression Assemble(Queue<Expression> operands, Queue<Expression> operators)
        {
            List<WrappedExpression<FormulaExpression>> wrappedOperators = new List<WrappedExpression<FormulaExpression>>
                (
                from oper in operators
                select new WrappedExpression<FormulaExpression>() { Payload = oper as FormulaExpression }
                );

            Expression result = Capture(wrappedOperators);

            for (int i = 0; i != wrappedOperators.Count; i++)
            {
                FormulaExpression expression = wrappedOperators[i].Payload;
                if (expression.ExpressionOperator.Type != OperatorType.UniaryOperator && expression.LeftOperand == null)
                    expression.LeftOperand = operands.Dequeue();
                if (expression.RightOperand == null)
                    expression.RightOperand = operands.Dequeue();
            }

            return result;
        }

        protected static FormulaExpression Capture(List<WrappedExpression<FormulaExpression>> wrappedOperators)
        {
            LinkedList<WrappedExpression<FormulaExpression>> operators = new LinkedList<WrappedExpression<FormulaExpression>>(wrappedOperators);
            List<LinkedListNode<WrappedExpression<FormulaExpression>>> captureList = new List<LinkedListNode<WrappedExpression<FormulaExpression>>>();

            LinkedListNode<WrappedExpression<FormulaExpression>> current = operators.First;
            for (int i = 0; i != operators.Count; i++)
            {
                if(current.Previous != null)
                {
                    if( current.Value.Payload.ExpressionOperator.Associativity == OperatorAssociativity.Left &&
                        current.Value.Payload.ExpressionOperator.Weight <= current.Previous.Value.Payload.ExpressionOperator.Weight)
                    {
                        current.Value.CaptureLeft = true;
                    }
                    else if (current.Value.Payload.ExpressionOperator.Associativity == OperatorAssociativity.Right &&
                            current.Value.Payload.ExpressionOperator.Weight < current.Previous.Value.Payload.ExpressionOperator.Weight)
                    {
                        current.Value.CaptureLeft = true;
                    }
                }

                if(current.Next != null)
                {
                    if (current.Value.Payload.ExpressionOperator.Associativity == OperatorAssociativity.Left &&
                        current.Value.Payload.ExpressionOperator.Weight < current.Next.Value.Payload.ExpressionOperator.Weight)
                    {
                        current.Value.CaptureRight = true;
                    }
                    else if (current.Value.Payload.ExpressionOperator.Associativity == OperatorAssociativity.Right &&
                            current.Value.Payload.ExpressionOperator.Weight <= current.Next.Value.Payload.ExpressionOperator.Weight)
                    {
                        current.Value.CaptureRight = true;
                    }
                }

                for(int j = 0; j <= captureList.Count ; j++)
                {
                    if(j == captureList.Count || current.Value.Payload.ExpressionOperator.Weight > captureList[j].Value.Payload.ExpressionOperator.Weight)
                    {
                        captureList.Insert(j, current);
                        break;
                    }
                }

                current = current.Next;
            }

            for (int i = 0; i != captureList.Count; i++ )
            {
                LinkedListNode<WrappedExpression<FormulaExpression>> currentNode = captureList[i];
                if (currentNode.Value.CaptureLeft)
                {
                    currentNode.Value.Payload.LeftOperand = currentNode.Previous.Value.Capture().Payload;
                    operators.Remove(currentNode.Previous);
                }
                if (currentNode.Value.CaptureRight)
                {
                    currentNode.Value.Payload.RightOperand = currentNode.Next.Value.Capture().Payload;
                    operators.Remove(currentNode.Next);
                }
            }

            return operators.First.Value.Payload;
        }
    }
}
