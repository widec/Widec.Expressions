// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions.Scanning
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public int Line { get; set; }

        public Token(TokenType type, string lexime, int line)
        {
            Type = type;
            Lexeme = lexime;
            Line = line;
        }
    }
}
