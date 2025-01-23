// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions
{
    public class UnaryExpression : Expression
    {
        public Token Operator { get; }
        public Expression Right { get; }
        public UnaryExpression(Token @operator, Expression right)
        {
            Right = right;
            Operator = @operator;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitUnary(this);
        }
    }
}
