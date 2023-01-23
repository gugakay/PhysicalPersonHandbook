using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Enums;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonCreateDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string PrivateNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int CityCode { get; set; }
        public IFormFile Image { get; set; }

        public ICollection<PhoneNumberCreateDto> PhoneNumbers { get; set; } = new HashSet<PhoneNumberCreateDto>();
    }
}
