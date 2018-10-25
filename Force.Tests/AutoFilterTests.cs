using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Extensions;
using Xunit;

namespace Force.Tests
{
    public class AutoFilterTests : DbContextTestsBase
    {
        public class DbContextForTesting
        {
            public IQueryable<TestProduct> Products;

            public DbContextForTesting()
            {
                Products = Enumerable.Range(1, 100).Select(x =>
                        new TestProduct
                        {
                            Price = 20 * x,
                            t = x % 9,
                            Name = $"name {x}"
                        })
                    .AsQueryable();
            }
        }

        class ProductStringDto
        {
            public string Name { get; set; }
        }

        class ProductCollectionDto
        {
            public int[] t { get; set; }
        }

        class ProductComposeFilter
        {
            public int[] t { get; set; }
            public int Price { get; set; }
        }

        public class TestProduct
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int t { get; set; }
        }

        class Product1Dto
        {
            public int Price { get; set; }
        }

        public class SpecExtentionsTests
        {
            [Fact]
            public void EntityTest()
            {
                var tc = new DbContextForTesting();

                var product = new Product1Dto
                {
                    Price = 200
                };
                var f = tc.Products.AutoFilter(product);
                Assert.Equal(1, f.Count());
            }

            [Fact]
            public void StringEntitytest()
            {
                var tc = new DbContextForTesting();

                var product = new ProductStringDto
                {
                    Name = "name 12"
                };
                var f = tc.Products.AutoFilter(product);
                Assert.Equal(1, f.Count());
            }


            [Fact]
            public void CollectionSortTest()
            {
                var tc = new DbContextForTesting();

                var product = new ProductCollectionDto
                {
                    t = Enumerable.Range(1, 2).ToArray(),
                };
                var f = tc.Products.AutoFilter(product);
                Assert.True(f.ToList().Any());
            }

            [Fact]
            public void ComposeFilter()
            {
                var tc = new DbContextForTesting();
                var filter = new ProductComposeFilter
                {
                    Price = 2000,
                    t = Enumerable.Range(0, 10).ToArray()
                };

                var s = tc.Products.First(x => x.Price == 2000 && filter.t.Contains(x.t));
                var ss = tc.Products.AutoFilter(filter).ToList();
                Assert.True(ss.ToList().Any());
            }

            /*[Fact]
            public void Filter()
            {
                var filter = new AutoFilter<Product1>(new ProductFilter()
                {
                    Name = "1",
                });

                var DbContext = new TestDbContext1();
                var products = filter.Filter(DbContext.Products).ToList();
                Assert.Single(products);
            }*/
        }
    }
}