using System;

namespace Elecelf.Sturnus.Operator
{
   /// <summary>
   /// A operator is a operation that will be executed when a expression with it will be calculate.
   /// </summary>
   /// <typeparam name="T">The type of the delegate will be called.</typeparam>
    public class Operator<T>
    {
        public T callback;
    }
}
