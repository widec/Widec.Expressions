// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Widec.Expressions.Scanning;

namespace Widec.Expressions
{
    public class VariableExpression : Expression
    {
        public Token Token { get; }

        public VariableExpression(Token token)
        {
            Token = token;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitVariable(this);
        }
    }
}
