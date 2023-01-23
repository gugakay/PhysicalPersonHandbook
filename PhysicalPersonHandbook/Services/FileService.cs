using DataAccess;
using DataAccess.Dtos.ImageDtos;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using PhysicalPersonHandbook.Infrastructure.Result;
using ILogger = Serilog.ILogger;


namespace PhysicalPersonHandBook.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger _logger;

        public FileService(IUnitOfWork unitOfWork, IWebHostEnvironment environment, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = environment;
            _logger = logger;
        }

        public async Task<ServiceResult> AddImageAsync(ImageCreateDto image, CancellationToken cancellationToken)
        {
            var personImage = await GetImageByPersonIdAsync(image.PersonId, cancellationToken);

            if (personImage != null)
            {
                _logger.Warning("Image for person with id {PersonId} already exists", image.PersonId);
                return new ServiceResult(false, "Image for person already exists");
            }

            try
            {
                var newImage = new Image()
                {
                    PersonId = image.PersonId,
                    Path = Path.Combine(_hostEnvironment.WebRootPath, "Images", image.PersonId+image.Image.FileName)
                };

                using (var stream = File.Create(newImage.Path))
                {
                    await image.Image.CopyToAsync(stream, cancellationToken);
                }

                await _unitOfWork.ImageRepository().AddAsync(newImage, cancellationToken);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResult(true, "Image added successfully");
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(ex, "Directory not found while adding image");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        public async Task<ServiceResult> DeleteImageAsync(int personId, CancellationToken cancellationToken)
        {
            var personImage = await GetImageByPersonIdAsync(personId, cancellationToken);

            if (personImage == null)
            {
                _logger.Warning("Image for person with id {PersonId} not found", personId);
                return new ServiceResult(false, "Image for person not found");
            }

            try
            {
                if (!File.Exists(personImage.Path))
                {
                    _logger.Warning("Image for person with id {PersonId} not found", personId);
                    return new ServiceResult(false, "Image for person not found");
                }

                File.Delete(personImage.Path);
                _unitOfWork.ImageRepository().Delete(personImage);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResult(true, "Image deleted successfully");
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(ex, "Directory not found while deleting image");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        public async Task<Image> GetImageByPersonIdAsync(int personId, CancellationToken cancellationToken)
        {
            var image = await _unitOfWork.ImageRepository().GetAllQueryable()
                .Where(x => x.PersonId == personId)
                .SingleOrDefaultAsync(cancellationToken);

            return image;
        }
    }
}
