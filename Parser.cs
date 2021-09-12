using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProgLang
{
    class Parser
    {
        public static String Parse(String code)
        {
            code = RemoveSingleLineComments(code);
            code = RemoveMultilineComments(code);
            code = RemoveWhitespaceCharacters(code);

            throw new NotImplementedException();
        }

        private static String GetStringInbetween(String s, String left, String right)
        {
            int indexLeft = s.IndexOf(left);
            int indexRight = s.IndexOf(right, indexLeft + 1);
            return s.Substring(indexLeft + 1, indexRight - (indexLeft + 1));
        }

        private static String RemoveWhitespaceCharacters(String s)
        {
            s = s.Replace("\n", String.Empty).Replace("\r", String.Empty);

            int indexNextOpenQuote = s.IndexOf("\"");

            // Es gibt keine String-Literals.
            if (indexNextOpenQuote == -1)
                return s.Replace(" ", String.Empty);

            // Whitespace nur aueßerhalb von String-Literals entfernen.
            int indexFrom = 0;
            while (indexNextOpenQuote != -1)
            {
                // Leerzeichen entfernen
                s = ReplaceInbetween(s, " ", String.Empty, indexFrom, indexNextOpenQuote);

                // Zum nächsten String-Literal
                indexFrom = s.IndexOf("\"", indexNextOpenQuote + 1) + 1;
                indexNextOpenQuote = s.IndexOf("\"", indexFrom);

                // Der Rest des Strings s hat kein String-Literal mehr.
                if (indexNextOpenQuote == -1)
                {
                    s = ReplaceInbetween(s, " ", String.Empty, indexFrom, s.Length);
                    break;
                }
            }

            return s;
        }

        /// <summary>
        /// Ersetzt in einem Substring von s, der durch die Indizes from und to angegeben wird, den String 
        /// oldString mit newString.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String ReplaceInbetween(String s, String oldString, String newString, int from, int to)
        {
            // Substring mit from und to bestimmen
            String substring = s.Substring(from, to - from);

            // Im Substring oldString mit newString ersetzen
            substring = substring.Replace(" ", String.Empty);

            // Substring wieder in s einfügen
            s = s.Remove(from, to - from);
            s = s.Insert(from, substring);

            return s;
        }

        private static String RemoveSingleLineComments(String s)
        {
            // "//" erkannt => bis zu new line zeichen entfernen.
            int indexNextSingleLineComment = s.IndexOf("//");
            int indexNewLineAfterComment = s.IndexOf("\r\n");
            while (indexNextSingleLineComment != -1)
            {
                s = s.Remove(indexNextSingleLineComment, indexNewLineAfterComment - (indexNextSingleLineComment));

                indexNextSingleLineComment = s.IndexOf("//");
                if (indexNextSingleLineComment == -1)
                    break;
                indexNewLineAfterComment = s.IndexOf("\r\n", indexNextSingleLineComment);
            }

            return s;
        }

        public static String RemoveMultilineComments(String s)
        {
            int indexStartMultilineComment = s.IndexOf("/*");
            int indexEndMultilineComment = s.IndexOf("*/", indexStartMultilineComment + 1);

            while (indexStartMultilineComment != -1 &&
                  indexEndMultilineComment != -1)
            {
                s = s.Remove(indexStartMultilineComment, indexEndMultilineComment - (indexStartMultilineComment - 2));

                if (indexStartMultilineComment + 1 > s.Length)
                    break;

                indexStartMultilineComment = s.IndexOf("/*", indexStartMultilineComment + 1);
                indexEndMultilineComment = s.IndexOf("*/", indexStartMultilineComment + 1);
            }
            return s;
        }
    }
}
