using Widec.Expressions.Parsing;
using Widec.Expressions.Scanning;
using Xunit.Sdk;

namespace Widec.Expressions.Test
{
    public class ParserTest
    {
        [Fact]
        public void NoTokens()
        {
            var parser = new Parser(new List<Token>());
            Assert.Throws<Exception>(() => parser.Parse());
        }

        [Fact]
        public void SingleEof()
        {
            var parser = new Parser(new List<Token>([new Token(TokenType.Eof, "", 0)]));
            Assert.Throws<Exception>(() => parser.Parse());
        }

        [Fact]
        public void EqualsEquals()
        {
            var parser = new Parser(new List<Token>([new Token(TokenType.Identifier,"x",1), new Token(TokenType.EqualEqual, "==", 1), new Token(TokenType.Identifier, "y", 1), new Token(TokenType.Eof, "", 0)]));
            var expression = parser.Parse();
            var visitor = new TestVisitor();
            expression.Accept(visitor);
            Assert.Equal("", visitor.Result);

        }

        [Fact]
        public void IdentifierEqualsEqualsMember()
        {
            var parser = new Parser(new List<Token>([new Token(TokenType.Identifier, "x", 1), new Token(TokenType.EqualEqual, "==", 1), new Token(TokenType.Identifier, "y", 1), new Token(TokenType.Dot, ".", 1), new Token(TokenType.Identifier, "z", 1), new Token(TokenType.Eof, "", 0)]));
            var expression = parser.Parse();
            var visitor = new TestVisitor();
            expression.Accept(visitor);
            Assert.Equal("", visitor.Result);
        }

        [Fact]
        public void IdentifierEqualsEqualsMemberMember()
        {
            var parser = new Parser(new List<Token>([
                new Token(TokenType.Identifier, "x", 1), 
                new Token(TokenType.EqualEqual, "==", 1), 
                new Token(TokenType.Identifier, "y", 1), new Token(TokenType.Dot, ".", 1), new Token(TokenType.Identifier, "z", 1), new Token(TokenType.Dot, ".", 1), new Token(TokenType.Identifier, "m", 1), 
                new Token(TokenType.Eof, "", 0)]));
            var expression = parser.Parse();
            var visitor = new TestVisitor();
            expression.Accept(visitor);
            Assert.Equal("", visitor.Result);
        }

        [Theory()]
        [InlineData("x == z", "(x EqualEqual z)")]
        [InlineData("x == z.y", "(x EqualEqual z.y)")]
        [InlineData("x == z.y.m", "(x EqualEqual z.y.m)")]
        [InlineData("x == z.y.m()", "(x EqualEqual z.y.m())")]
        [InlineData("x == z.y.m(a+b)", "(x EqualEqual z.y.m((a Plus b)))")]
        [InlineData("a+b*c", "(a Plus (b Star c))")]
        [InlineData("a*b+c", "((a Star b) Plus c)")]
        [InlineData("a+b/c", "(a Plus (b Slash c))")]
        [InlineData("a&&b", "(a And b)")]
        [InlineData("a && b", "(a And b)")]
        [InlineData("a||b", "(a Or b)")]
        [InlineData("a || b", "(a Or b)")]
        [InlineData("a || b && c", "(a Or (b And c))")]
        public void ScanAndParse(string source, string expected)
        {
            var scanner = new Scanner();
            var tokens = scanner.Scan(source);
            var parser = new Parser(tokens);
            var expression = parser.Parse();
            var visitor = new TestVisitor();
            expression.Accept(visitor);
            Assert.Equal(expected, visitor.Result);
        }
    }
}