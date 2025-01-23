// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Token @operator, Expression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitBinary(this);
        }
    }
}
