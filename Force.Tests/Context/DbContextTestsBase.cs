using Microsoft.EntityFrameworkCore;

namespace Force.Tests
{
    public abstract class DbContextTestsBase
    {
        public static volatile bool IsInitialized = false;

        public static object Locker = new object();

        protected readonly TestDbContext DbContext;

        protected DbContextTestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");

            DbContext = new TestDbContext(optionsBuilder.Options);


            if (!IsInitialized)
            {
                lock (Locker)
                {
                    //TODO:убрать new Category(3,"3") и new Category(4,"4"), была ошибка компилятора
                    if (!IsInitialized)
                    {
                        DbContext.Products.Add(new Product(2, "2", new Category(3, "3")));
                        DbContext.Products.Add(new Product(1, "1", new Category(4, "4")));
                        DbContext.SaveChanges();
                    }

                    IsInitialized = true;
                }
            }
        }
    }
}