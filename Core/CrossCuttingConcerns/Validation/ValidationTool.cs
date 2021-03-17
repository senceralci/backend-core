using FluentValidation;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static void Validate(IValidator validator, object instanceToValidate)
        {
            var context = new ValidationContext<object>(instanceToValidate);
            var validationResult = validator.Validate(context);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}