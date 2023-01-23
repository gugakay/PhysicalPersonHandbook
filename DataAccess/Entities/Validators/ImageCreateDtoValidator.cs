using DataAccess.Dtos.ImageDtos;
using DataAccess.Resources.Image;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DataAccess.Entities.Validators
{
    public class ImageCreateDtoValidator : AbstractValidator<ImageCreateDto>
    {
        private readonly IStringLocalizer<ImageCreateValidationResources> _resourcesLocalizer;
        private readonly IStringLocalizer<ImageSharedValidationResources> _imageResourcesLocalizer;

        public ImageCreateDtoValidator(IStringLocalizer<ImageCreateValidationResources> resourcesLocalizer,
                                       IStringLocalizer<ImageSharedValidationResources> imageResourcesLocalizer)
        {
            _resourcesLocalizer = resourcesLocalizer;
            _imageResourcesLocalizer = imageResourcesLocalizer;

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage(_imageResourcesLocalizer["ImageRequired"])
                .Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png"))
                .WithMessage(_imageResourcesLocalizer["ImageFormatError"]);

            RuleFor(x => x.PersonId)
                .NotNull()
                .WithMessage(_resourcesLocalizer["PersonIdRequired"]);
        }
    }
}
