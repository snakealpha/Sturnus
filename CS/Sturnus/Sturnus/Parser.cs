using System;
using System.Collections.Generic;
using System.Text;

namespace Elecelf.Sturnus
{
    /// <summary>
    /// Parser is the class that used to translate a string to a expression tree.
    /// </summary>
    public class Parser
    {
        public const string VariableFirstChars =    "_" +
                                                    "abcdefghijklmnopqrstuvwxyz" +
                                                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string VariableChars = "_" +
                                            "0123456789" +
                                            "abcdefghijklmnopqrstuvwxyz" +
                                            "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
