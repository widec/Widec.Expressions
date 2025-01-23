// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions
{
    public interface IExpressionVisitor
    {
        void VisitGrouping(GroupingExpression grouping);
        void VisitLiteral(LiteralExpression literal);
        void VisitUnary(UnaryExpression unary);
        void VisitBinary(BinaryExpression binary);
        void VisitVariable(VariableExpression variable);
        void VisitCall(CallExpression call);
        void VisitMember(MemberExpression member);
    }
}
