// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions
{
    public class CallExpression : Expression
    {
        public Expression Callee { get; }
        public Token Paren { get; }
        public List<Expression> Arguments { get; }

        public CallExpression(Expression callee, Token paren, List<Expression> arguments)
        {
            Callee = callee;
            Paren = paren;
            Arguments = arguments;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitCall(this);
        }
    }
}
