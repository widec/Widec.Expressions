// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions
{
    public class GroupingExpression : Expression
    {
        public Expression Expression { get; }
        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitGrouping(this);
        }
    }
}
