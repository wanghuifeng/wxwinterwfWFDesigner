using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace Sonic
{
    /// <summary>
    /// A default implementation of IQueryable for use with QueryProvider
    /// </summary>
    public class Query<T> : IQueryable<T>, IQueryable, 
        IEnumerable<T>, IEnumerable, 
        IOrderedQueryable<T>, IOrderedQueryable
    {
        IQueryProvider provider;
        Expression expression;

        public Query(IQueryProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        Expression IQueryable.Expression
        {
            get { return this.expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)
                this.provider.Execute(this.expression))
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)
                this.provider.Execute(this.expression))
                    .GetEnumerator();
        }

        public override string ToString()
        {
            if (this.expression.NodeType == ExpressionType.Constant &&
                ((ConstantExpression)this.expression).Value == this)
            {
                return "Query(" + typeof(T) + ")";
            }
            else
            {
                return this.expression.ToString();
            }
        }


    }

}
