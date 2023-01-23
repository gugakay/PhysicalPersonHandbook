using DataAccess.Dtos.PersonDtos;
using DataAccess.Resources.Image;
using DataAccess.Resources.Person;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace DataAccess.Entities.Validators
{
    public class PersonFilterDtoValidator : AbstractValidator<PersonFilterDto>
    {
        private readonly IStringLocalizer<PersonValidationResources> _resourcesLocalizer;

        public PersonFilterDtoValidator(IStringLocalizer<PersonValidationResources> resourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;

            RuleFor(x => x.Name)
                .Length(2, 50).WithMessage(_resourcesLocalizer["NameLengthError"]);

            RuleFor(x => x.LastName)
                .Length(2, 50).WithMessage(_resourcesLocalizer["LastNameLengthError"]);

            RuleFor(x => x.PrivateNumber)
                .Matches("^[0-9]*$").WithMessage(_resourcesLocalizer["PrivateNumberNumericError"]);

        }
    }
}
