using DataAccess.Dtos.PersonDtos;
using DataAccess.Resources.Person;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DataAccess.Entities.Validators
{
    public class PersonUpdateDtoValidator : AbstractValidator<PersonUpdateDto>
    {
        private readonly IStringLocalizer<PersonValidationResources> _resourcesLocalizer;

        public PersonUpdateDtoValidator(IStringLocalizer<PersonValidationResources> resourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;

            RuleFor(x => x.Name)
                .Length(2, 50).WithMessage(_resourcesLocalizer["NameLengthError"]);

            RuleFor(x => x.LastName)
                .Length(2, 50).WithMessage(_resourcesLocalizer["LastNameLengthError"]);

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(_resourcesLocalizer["GenderEnumError"]);

            RuleFor(x => x.PrivateNumber)
                .Length(11).WithMessage(_resourcesLocalizer["PrivateNumberLengthError"])
                .Matches("^[0-9]*$").WithMessage(_resourcesLocalizer["PrivateNumberNumericError"]);

            RuleFor(x => x.BirthDate)
                .Must(x => x != default(DateTime)).WithMessage(_resourcesLocalizer["BirthDateNotValidDate"])
                .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
                .WithMessage(_resourcesLocalizer["AgeLimitError"]);
        }
    }
}
