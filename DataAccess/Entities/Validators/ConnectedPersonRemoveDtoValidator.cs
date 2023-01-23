using DataAccess.Dtos.ConnectedPersonDtos;
using DataAccess.Resources.Person;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace DataAccess.Entities.Validators
{
    public class ConnectedPersonRemoveDtoValidator : AbstractValidator<ConnectedPersonRemoveDto>
    {
        private readonly IStringLocalizer<PersonValidationResources> _resourcesLocalizer;

        public ConnectedPersonRemoveDtoValidator(IStringLocalizer<PersonValidationResources> resourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;

            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage(_resourcesLocalizer["PersonIdRequired"]);

            RuleFor(x => x.ConnectedPersonId)
                .NotEmpty().WithMessage(_resourcesLocalizer["ConnectedPersonIdRequired"]);

            RuleFor(x => x.Type)
                .NotNull().WithMessage(_resourcesLocalizer["ConnectionTypeEmptyError"])
                .IsInEnum().WithMessage(_resourcesLocalizer["ConnectionTypeEnumError"]);
        }
    }
}
