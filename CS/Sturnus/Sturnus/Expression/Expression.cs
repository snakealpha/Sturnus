using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// A Expression is a description of a calculating process.
    /// For a uniary expression, it's constructed by a uniary operator and only one operand;
    /// for a binary expression, it's constructed by a binary operator and two operands: left operand and right operand, which are both expressions.
    /// A Operand is another express, which may be a constant, varible or a formula.
    /// </summary>
    public class Expression
    {
        protected double value;
        public virtual double Value
        {
            set;
            get;
        }

        public virtual double Calculate()
        {
            throw new NotImplementedException("The base class of Expression should not be used.");
        }
    }
}
