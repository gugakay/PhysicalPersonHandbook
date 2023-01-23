using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Resources.PhoneNumber;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DataAccess.Entities.Validators
{
    public class PhoneNumberCreateDtoValidator : AbstractValidator<PhoneNumberCreateDto>
    {
        private readonly IStringLocalizer<PhoneNumberCreateValidationResources> _resourcesLocalizer;

        public PhoneNumberCreateDtoValidator(IStringLocalizer<PhoneNumberCreateValidationResources> resourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;

            RuleFor(x => x.Type)
                .NotNull().WithMessage(_resourcesLocalizer["ConnectionTypeEmptyError"])
                .IsInEnum().WithMessage(_resourcesLocalizer["ConnectionTypeEnumError"]);

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage(_resourcesLocalizer["PhoneNumberEmptyError"])
                .Length(4,50).WithMessage(_resourcesLocalizer["PhoneNumberLengthError"])
                .Matches("^[0-9]*$").WithMessage(_resourcesLocalizer["PhoneNumberNumericError"]);
        }
    }
}
