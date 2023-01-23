using FluentValidation;

namespace DataAccess.Entities.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Id)
                .NotNull();
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Z]+$|^[ა-ჰ]+$")
                .Length(2, 50);
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Matches(@"^[a-zA-Z]+$|^[ა-ჰ]+$")
                .Length(2, 50);
            
            RuleFor(x => x.Gender)
                .NotNull().IsInEnum();
            
            RuleFor(x => x.PrivateNumber)
                .NotNull().NotEmpty()
                .Length(11);
            
            RuleFor(x => x.BirthDate)
                .LessThanOrEqualTo(DateTime.Now.AddYears(-18));
        }
    }
}
