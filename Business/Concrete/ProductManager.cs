using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validate;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    [Authorize(roles: "user,admin", Priority = 0)]
    public class ProductManager : IProductService
    {
        #region members

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion

        #region constructor

        public ProductManager(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        #endregion

        //[Authorize(roles: "user,admin", Priority = 0)]
        [Validate(validatorType: typeof(ProductValidator), Priority = 1)]
        [RemoveCache(pattern: "IProductService.Get", Priority = 2)]//metot başarılı olursa get ile başlayan keyleri cache'ten temizle
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.CheckAll(
                CheckIfProductNameExists(product.ProductName),
                CheckIfProductCategoryDoesNotExists(product.CategoryId));

            if (result != null)
            {
                return result;
            }

            _productRepository.Add(product);
            _productRepository.SaveChanges();//todo:! uow uygulanmalı mı mantıklı mı?

            return new SuccessResult(Messages.Added);
        }

        [AddGetCache(duration: 30)]
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>
                (_productRepository.GetList().ToList());
        }

        [AddGetCache(duration: 30)]
        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>
                (_productRepository.GetList(x => x.CategoryId == categoryId).ToList());
        }

        [AddGetCache(duration: 30)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>
                (_productRepository.Get(x => x.ProductId == productId));
        }

        [AddGetCache(duration: 2)]
        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            throw new System.NotImplementedException();
        }

        [AddGetCache(duration: 30)]
        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>
                (_productRepository.GetProductDetails());
        }

        [Validate(validatorType: typeof(ProductValidator), Priority = 1)]
        [RemoveCache(pattern: "IProductService.Get", Priority = 2)]
        public IResult Update(Product product)
        {
            _productRepository.Update(product);
            _productRepository.SaveChanges();//todo: !uow uygulanabilir

            return new SuccessResult(Messages.Updated);
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productRepository.GetCount(p => p.ProductName == productName) > 0;

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductCategoryDoesNotExists(int categoryId)
        {
            var result = _categoryRepository.GetCount(c => c.CategoryId == categoryId) == 0;

            if (result)
            {
                return new ErrorResult(Messages.ProductCategoryDoesNotExists);
            }

            return new SuccessResult();
        }
    }
}