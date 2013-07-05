using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.CSharp
{
    /// <summary>
    /// Outputs the AST.
    /// </summary>
    public class CSharpOutputVisitor 
    {
        /// <summary>
        /// Gets the escape sequence for the specified character.
        /// </summary>
        /// <remarks>This method does not convert ' or ".</remarks>
        public static string ConvertChar(char ch)
        {
            switch (ch)
            {
                case '\\':
                    return "\\\\";
                case '\0':
                    return "\\0";
                case '\a':
                    return "\\a";
                case '\b':
                    return "\\b";
                case '\f':
                    return "\\f";
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\v':
                    return "\\v";
                default:
                    if (char.IsControl(ch) || char.IsSurrogate(ch) ||
                        // print all uncommon white spaces as numbers
                        (char.IsWhiteSpace(ch) && ch != ' '))
                    {
                        return "\\u" + ((int)ch).ToString("x4");
                    }
                    else
                    {
                        return ch.ToString();
                    }
            }
        }

        /// <summary>
        /// Converts special characters to escape sequences within the given string.
        /// </summary>
        public static string ConvertString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in str)
            {
                if (ch == '"')
                {
                    sb.Append("\\\"");
                }
                else
                {
                    sb.Append(ConvertChar(ch));
                }
            }
            return sb.ToString();
        }
    }
}
