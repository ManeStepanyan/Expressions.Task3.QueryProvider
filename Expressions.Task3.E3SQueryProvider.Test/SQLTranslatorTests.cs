using Expressions.Task3.E3SQueryProvider.Models.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Expressions.Task3.E3SQueryProvider.Test
{
    public class SQLTranslatorTests
    {
        [Fact]
        public void TestWhereEqualQueryable()
        {
            var translator = new ExpressionToSQLQueryTranslator();
            Expression<Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>>> expression
                = query => query.Where(e => e.Name == "ProductName");

            string translated = translator.Translate(expression);
            Assert.Equal("SELECT * FROM db.Products WHERE Name=ProductName", translated);
        }
        [Fact]
        public void TestWhereGreaterThanQueryable()
        {
            var translator = new ExpressionToSQLQueryTranslator();
            Expression<Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>>> expression
                = query => query.Where(e => e.Price > 500);

            string translated = translator.Translate(expression);
            Assert.Equal("SELECT * FROM db.Products WHERE Price>500", translated);
        }
        [Fact]
        public void TestWhereLessThanQueryable()
        {
            var translator = new ExpressionToSQLQueryTranslator();
            Expression<Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>>> expression
                = query => query.Where(e => e.Price < 1000);

            string translated = translator.Translate(expression);
            Assert.Equal("SELECT * FROM db.Products WHERE Price<1000", translated);
        }

    }
}
