// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions.Scanning
{
    public enum TokenType
    {
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        Comma,
        Dot,
        Minus,
        Plus,
        SemiColon,
        Star,
        Slash,
        Bang,
        BangEqual,
        EqualEqual,
        Equal,
        LessOrEqual,
        GreaterOrEqual,
        Less,
        Greater,
        String,
        Number,
        Identifier,
        True,
        False,
        Null,
        Eof,
        Or,
        And
    }
}
