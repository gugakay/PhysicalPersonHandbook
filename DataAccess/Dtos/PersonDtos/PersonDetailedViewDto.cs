using DataAccess.Dtos.ConnectedPersonDtos;
using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Enums;

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonDetailedViewDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string PrivateNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int CityCode { get; set; }

        public byte[] Image { get; set; }

        public ICollection<PhoneNumberViewDto> PhoneNumbers { get; set; }

        public ICollection<ConnectedPersonViewDto> ConnectedPersons { get; set; }
    }
}
