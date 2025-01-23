// Wim De Cleen licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Widec.Expressions
{
    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor expressionVisitor);
    }
}
