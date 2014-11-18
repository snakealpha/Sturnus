using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// Parser is the class that used to translate a string to a expression tree.
    /// </summary>
    public abstract class Parser
    {
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

        /// <summary>
        /// Parse a string into a expression tree.
        /// </summary>
        /// <param name="expression">Raw string of the expression tree.</param>
        /// <param name="context"> Context of operators.</param>
        /// <returns></returns>
        public Expression Parse(string expression, OperatorContext context = null)
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

            throw new NotImplementedException();
        }

        private static Expression parseConstant(LinkedList<char> signs)
        {
            bool dotAppeared = false;
            char peekChar;
            StringBuilder numRawStr = new StringBuilder();

            while (true)
            {
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

            ConstantExpression constant = new ConstantExpression(numRawStr.ToString());
            return constant;
        }

        private static Expression parseVarible(LinkedList<char> signs)
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

        private static Expression parseUniaryOperator(LinkedList<char> signs, int escalateTime, OperatorContext context)
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
                        lastOperator = context.UniaryOperators[currentStr];

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

        private static Expression parseBinaryOperator(LinkedList<char>signs, int escalateTime, OperatorContext context)
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
                        lastOperator = context.BinaryOperators[currentStr];

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
    }
}
