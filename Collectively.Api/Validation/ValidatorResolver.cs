using Autofac;

namespace Collectively.Api.Validation
{
    public class ValidatorResolver : IValidatorResolver
    {
        private readonly IComponentContext _context;

        public ValidatorResolver(IComponentContext context)
        {
            _context = context;
        }

        public IValidator<T> Resolve<T>()
        {
            IValidator<T> validator;

            return _context.TryResolve(out validator) ? validator : new EmptyValidator<T>();
        }
    }
}