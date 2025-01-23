// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions
{
    public class MemberExpression : Expression
    {
        public Expression Container { get; }

        public Expression Member { get; }

        public MemberExpression(Expression container, Expression member)
        {
            Container = container;
            Member = member;
        }

        public override void Accept(IExpressionVisitor expressionVisitor)
        {
            expressionVisitor.VisitMember(this);
        }
    }
}
