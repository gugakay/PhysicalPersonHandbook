using FluentValidation;

namespace DataAccess.Entities.Validators
{
    public class PhoneNumberValidator : AbstractValidator<PhoneNumber>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Type).NotNull().IsInEnum();
            RuleFor(x => x.Number).NotNull().NotEmpty().Length(4, 50);
            RuleFor(x => x.PersonId).NotNull();
            RuleFor(x => x.Person).NotNull();
        }
    }
}
