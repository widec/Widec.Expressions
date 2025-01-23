// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions.Parsing
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public Expression Parse()
        {
            return ReadExpression();
        }

        private Expression ReadExpression()
        {
            return ReadOr();
        }

        private Expression ReadOr()
        {
            var expression = ReadAnd();

            while (Match(TokenType.Or))
            {
                Token @operator = Previous();
                var right = ReadAnd();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadAnd()
        {
            var expression = ReadEquality();

            while (Match(TokenType.And))
            {
                Token @operator = Previous();
                var right = ReadEquality();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadEquality()
        {
            var expression = ReadComparison();

            while (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                var @operator = Previous();
                var right = ReadComparison();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadComparison()
        {
            var expression = ReadTerm();

            while (Match(TokenType.Greater, TokenType.GreaterOrEqual, TokenType.Less, TokenType.LessOrEqual))
            {
                Token @operator = Previous();
                var right = ReadTerm();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadTerm()
        {
            var expression = ReadFactor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                var @operator = Previous();
                var right = ReadFactor();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadFactor()
        {
            var expression = ReadUnary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                var @operator = Previous();
                var right = ReadUnary();
                expression = new BinaryExpression(expression, @operator, right);
            }

            return expression;
        }

        private Expression ReadUnary()
        {
            if (Match(TokenType.Bang, TokenType.Minus))
            {
                Token @operator = Previous();
                var right = ReadUnary();
                return new UnaryExpression(@operator, right);
            }

            return ReadCall();
        }

        private Expression ReadPrimary()
        {
            if (Match(TokenType.False))
            {
                return new LiteralExpression(Previous());
            }
            if (Match(TokenType.True))
            {
                return new LiteralExpression(Previous());
            }
            if (Match(TokenType.Null))
            {
                return new LiteralExpression(Previous());
            }

            if (Match(TokenType.Number, TokenType.String))
            {
                return new LiteralExpression(Previous());
            }

            if (Match(TokenType.Identifier))
            {
                var expression = new VariableExpression(Previous());
                if (Match(TokenType.Dot))
                {
                    var member = ReadExpression();
                    return new MemberExpression(expression, member);
                }
                return expression;
            }

            if (Match(TokenType.LeftParen))
            {
                var expr = ReadExpression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new GroupingExpression(expr);
            }

            throw CreateError(Peek(), "Unexpected token");
        }

        private Expression ReadCall()
        {
            var expression = ReadPrimary();

            while (true)
            {
                if (Match(TokenType.LeftParen))
                {
                    expression = ReadFinishCall(expression);
                }
                else
                {
                    break;
                }
            }

            return expression;
        }

        private Expression ReadFinishCall(Expression callee)
        {
            var arguments = new List<Expression>();
            if (!Check(TokenType.RightParen))
            {
                do
                {
                    arguments.Add(ReadExpression());
                } while (Match(TokenType.Comma));
            }

            Token paren = Consume(TokenType.RightParen,
                                  "Expect ')' after arguments.");

            return new CallExpression(callee, paren, arguments);
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
            {
                return false;
            }
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                _current++;
            }
            return Previous();
        }

        private bool IsAtEnd()
        {
            return _tokens[_current].Type == TokenType.Eof;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                return Advance();
            }

            throw CreateError(Peek(), message);
        }

        private Exception CreateError(Token token, string message)
        {
            return new Exception(message);
        }
    }
}
