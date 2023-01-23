using DataAccess.Dtos.ImageDtos;
using PhysicalPersonHandbook.Infrastructure.Result;

namespace PhysicalPersonHandBook.Services
{
    public interface IFileService
    {
        Task<ServiceResult> AddImageAsync(ImageCreateDto image, CancellationToken cancellationToken);
        Task<ServiceResult> DeleteImageAsync(int personId, CancellationToken cancellationToken);
    }
}