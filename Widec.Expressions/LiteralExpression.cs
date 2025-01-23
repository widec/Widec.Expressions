// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions
{
    public class LiteralExpression : Expression
    {
        public Token Token { get; }
        public LiteralExpression(Token token)
        {
            Token = token;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitLiteral(this);
        }
    }
}
