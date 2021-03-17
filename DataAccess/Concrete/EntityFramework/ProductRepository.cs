using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class ProductRepository : EfEntityRepositoryBase<Product, AppDbContext>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public List<ProductDetailDto> GetProductDetails()
        {
            var res = Context.Products.Join(Context.Categories, p => p.CategoryId, c => c.CategoryId,
                (p, c) => new ProductDetailDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = c.CategoryName,
                    UnitsInStock = p.UnitsInStock
                });

            //var result = from p in context.Products
            //             join c in context.Categories
            //             on p.CategoryId equals c.CategoryId
            //             select new ProductDetailDto
            //             {
            //                 ProductId = p.ProductId,
            //                 ProductName = p.ProductName,
            //                 CategoryName = c.CategoryName,
            //                 UnitsInStock = p.UnitsInStock
            //             };

            return res.ToList();
        }
    }
}