using Microsoft.AspNetCore.Http;

namespace DataAccess.Dtos.ImageDtos
{
    public class ImageCreateDto
    {
        public int PersonId { get; set; }
        public IFormFile Image { get; set; }
    }
}
