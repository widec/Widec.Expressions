using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Widec.Expressions.Test
{
    public class TestVisitor : IExpressionVisitor
    {
        private readonly StringBuilder _result = new StringBuilder();
        
        public void VisitBinary(BinaryExpression binary)
        {
            _result.Append("(");
            binary.Left.Accept(this);
            _result.Append($" {binary.Operator.Type} ");
            binary.Right.Accept(this);
            _result.Append(")");
        }

        public void VisitGrouping(GroupingExpression grouping)
        {
            _result.Append("(");
            grouping.Expression.Accept(this);
            _result.Append(")");
        }

        public void VisitLiteral(LiteralExpression literal)
        {
            _result.Append($"{literal.Token.Lexeme}|({literal.Token.Type})");
        }

        public void VisitUnary(UnaryExpression unary)
        {
            _result.Append($" {unary.Operator.Type} ");
            unary.Right.Accept(this);
        }

        public void VisitVariable(VariableExpression variable)
        {
            _result.Append($"{variable.Token.Lexeme}");
        }

        public void VisitCall(CallExpression call)
        {
            call.Callee.Accept(this);
            _result.Append("(");
            var first = true;
            foreach (var argument in call.Arguments)
            {
                if (!first)
                {
                    _result.Append(",");
                }
                else 
                { 
                    first = false;
                }
                argument.Accept(this);
            }
            _result.Append(")");
        }

        public void VisitMember(MemberExpression member)
        {
            member.Container.Accept(this);
            _result.Append(".");
            member.Member.Accept(this);
        }

        public string Result { get { return _result.ToString();  } }
    }
}
