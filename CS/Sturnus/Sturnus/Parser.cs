using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }

        private static Expression parseVarible(LinkedList<char> signs)
        {
            throw new NotImplementedException();
        }

        private static Expression parseUniaryOperator(LinkedList<char> signs, int escalateTime, OperatorContext context)
        {
            throw new NotImplementedException();
        }

        private static Expression parseBinaryOperator(LinkedList<char>signs, int escalateTime, OperatorContext context)
        {
            throw new NotImplementedException();
        }

    }
}
