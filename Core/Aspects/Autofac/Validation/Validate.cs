using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using FluentValidation;
using System;
using System.Linq;

namespace Core.Aspects.Autofac.Validate
{
    /// <summary>
    /// ValidationAspect
    /// </summary>
    public class Validate : MethodInterception
    {
        private readonly Type _validatorType;

        /// <summary>
        /// İlgili IEntity türevi sınıf için oluşturulan IValidator türevi validasyon sınıfıda tanımlana kural setlerini çalıştırır. Örn: [Validate(typeof(ProductValidator))]
        /// </summary>
        /// <param name="validatorType"></param>
        public Validate(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new ArgumentException(AspectMessages.WrongValidationType);
            }
            _validatorType = validatorType;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);

            foreach (var instanceToValidate in entities)
            {
                ValidationTool.Validate(validator, instanceToValidate);
            }
        }
    }
}
