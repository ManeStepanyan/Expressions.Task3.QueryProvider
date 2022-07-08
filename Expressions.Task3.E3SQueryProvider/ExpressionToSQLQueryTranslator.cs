using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToSQLQueryTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToSQLQueryTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            _resultStringBuilder.Append("SELECT * FROM db.Products ");
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                _resultStringBuilder.Append("WHERE ");
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left.NodeType != ExpressionType.MemberAccess)
                throw new NotSupportedException($"One of the operands should be property or field: {node.NodeType}");

            if (node.Right.NodeType != ExpressionType.Constant)
                throw new NotSupportedException($"Right operand should be constant: {node.NodeType}");
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        Visit(node.Left);
                        _resultStringBuilder.Append("=");
                        Visit(node.Right);
                    } 

                    break;
                case ExpressionType.GreaterThan:
                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        Visit(node.Left);
                        _resultStringBuilder.Append(">");
                        Visit(node.Right);
                    }
                    break;

                case ExpressionType.LessThan:
                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        Visit(node.Left);
                        _resultStringBuilder.Append("<");
                        Visit(node.Right);
                    }
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
