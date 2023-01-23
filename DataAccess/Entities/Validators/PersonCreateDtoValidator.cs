using DataAccess.Dtos.PersonDtos;
using DataAccess.Resources.Image;
using DataAccess.Resources.Person;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace DataAccess.Entities.Validators
{
    public class PersonCreateDtoValidator : AbstractValidator<PersonCreateDto>
    {
        private readonly IStringLocalizer<PersonValidationResources> _resourcesLocalizer;
        private readonly IStringLocalizer<ImageSharedValidationResources> _imageResourcesLocalizer;

        public PersonCreateDtoValidator(IStringLocalizer<PersonValidationResources> resourcesLocalizer,
                                        IStringLocalizer<ImageSharedValidationResources> imageResourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;
            _imageResourcesLocalizer = imageResourcesLocalizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_resourcesLocalizer["NameEmptyError"])
                .Length(2, 50).WithMessage(_resourcesLocalizer["NameLengthError"]);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(_resourcesLocalizer["LastNameEmptyError"])
                .Length(2, 50).WithMessage(_resourcesLocalizer["LastNameLengthError"]);

            var regexEN = "^[a-zA-Z]+$";
            var regexGE = "^[ა-ჰ]+$";

            RuleFor(x => new { x.Name, x.LastName })
                .Must(x => (Regex.Match(x.Name, regexEN).Success && Regex.Match(x.LastName, regexEN).Success)
                || (Regex.Match(x.Name, regexGE).Success && Regex.Match(x.LastName, regexGE).Success))
                .WithMessage(_resourcesLocalizer["NameLastNameLanguageError"]);


            RuleFor(x => x.Gender)
                .NotNull().WithMessage(_resourcesLocalizer["GenderEmptyError"])
                .IsInEnum().WithMessage(_resourcesLocalizer["GenderEnumError"]);

            RuleFor(x => x.PrivateNumber)
                .NotEmpty().WithMessage(_resourcesLocalizer["PrivateNumberEmptyError"])
                .Length(11).WithMessage(_resourcesLocalizer["PrivateNumberLengthError"])
                .Matches("^[0-9]*$").WithMessage(_resourcesLocalizer["PrivateNumberNumericError"]);

            RuleFor(x => x.BirthDate)
                .NotNull().WithMessage(_resourcesLocalizer["BirthDateNullError"])
                .Must(x => x != default(DateTime)).WithMessage(_resourcesLocalizer["BirthDateNotValidDate"])
                .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
                .WithMessage(_resourcesLocalizer["AgeLimitError"]);

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage(_imageResourcesLocalizer["ImageRequired"])
                .Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png"))
                .WithMessage(_imageResourcesLocalizer["ImageFormatError"]);
        }
    }
}
