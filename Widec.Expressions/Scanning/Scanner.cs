// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions.Scanning
{
    public class Scanner
    {
        private List<Token> _tokens = new List<Token>();
        private int _start;
        private int _current = 0;
        private int _line = 1;
        private string _source = string.Empty;

        public List<Token> Scan(string source)
        {
            _source = source;
            _tokens = new List<Token>();
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }
            AddToken(TokenType.Eof);
            return _tokens;
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '{': AddToken(TokenType.LeftBracket); break;
                case '}': AddToken(TokenType.RightBracket); break;
                case '+': AddToken(TokenType.Plus); break;
                case '-': AddToken(TokenType.Minus); break;
                case '.': AddToken(TokenType.Dot); break;
                case ';': AddToken(TokenType.SemiColon); break;
                case '*': AddToken(TokenType.Star); break;
                case '/':
                    {
                        if (Match('/'))
                        {
                            while (Peek() == '\n' && !IsAtEnd())
                            {
                                Advance();
                            }
                        }
                        else
                        {
                            AddToken(TokenType.Slash);
                        }
                        break;
                    }
                case ',': AddToken(TokenType.Comma); break;
                case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
                case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
                case '<': AddToken(Match('=') ? TokenType.LessOrEqual : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.GreaterOrEqual : TokenType.Greater); break;
                case '|': if (Match('|')) { AddToken(TokenType.Or); } break;
                case '&': if (Match('&')) { AddToken(TokenType.And); } break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    _line++;
                    break;
                case '"':
                    ReadString();
                    break;
                default:
                    if (IsAlpha(c))
                    {
                        ReadIdentifier();
                    }
                    break;
            }
        }

        private void AddToken(TokenType token, string? value = null)
        {
            _tokens.Add(new Token(token, value == null ? _source.Substring(_start, _current - _start) : value, _line));
        }

        private char Advance()
        {
            return _source[_current++];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' ||
                   c >= 'A' && c <= 'Z' ||
                    c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }
            if (_source[_current] != expected)
            {
                return false;
            }
            _current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length)
            {
                return '\0';
            }
            return _source[_current + 1];
        }

        private void ReadString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                }
                Advance();
            }

            if (IsAtEnd())
            {
                throw CreateError("unexpected end of file");
            }

            Advance();

            var value = _source.Substring(_start + 1, _current - 1);
            AddToken(TokenType.String, value);
        }

        private void number()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.Number);
        }

        private void ReadIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }
            var identifier = _source.Substring(_start, _current - _start);
            if (identifier == "true")
            {
                AddToken(TokenType.True);
            }
            else if (identifier == "false")
            {
                AddToken(TokenType.False);
            }
            else if (identifier == "null")
            {
                AddToken(TokenType.Null);
            }
            else
            {
                AddToken(TokenType.Identifier);
            }
        }

        private Exception CreateError(string errorMessage)
        {
            return new Exception(errorMessage);
        }
    }
}
