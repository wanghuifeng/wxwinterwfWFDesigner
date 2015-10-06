// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sonic
{
    /// <summary>
    /// A basic abstract LINQ query provider
    /// </summary>
    public abstract class QueryProvider : IQueryProvider
    {
        #region Ctor
        protected QueryProvider()
        {
        }
        #endregion

        #region IQueryProvider Members

        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new Query<T>(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public T Execute<T>(Expression expression)
        {
            return (T)this.Execute(expression);
        }

        public abstract object Execute(Expression expression);

        #endregion
    }
}
